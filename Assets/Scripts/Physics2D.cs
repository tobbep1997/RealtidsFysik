using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics2D : MonoBehaviour {


    [SerializeField]
    public float mass = 1000;
    [SerializeField]
    public float f_gravity = 9.80665f;

    [SerializeField]
    public Vector3 StartAngle;
    [SerializeField]
    public Vector3 force;
    public Vector3 acceleration;
    public Vector3 velocity;
    public Vector3 position;

    [SerializeField]
    public Vector3 rotation, rotAcceleration;
    

    [SerializeField]
    private float cD = 1.0f, density = 1.0f, area = 2.0f;
    [SerializeField]
    private float cM = 1.0f;

    [SerializeField]
    public float boncyness = 1.0f;

    [SerializeField]
    private bool staticObject = false;

    [SerializeField]
    private bool debug = true, arch;
    [SerializeField]
    [Range(0, 10.0f)]
    private float rayDuration = 1.0f, pointIteration = 1.0f;

    private float timer;
    private List<Vector3> points;

	void Start () {
        position = transform.position;
        velocity = StartAngle;

        timer = pointIteration;
        points = new List<Vector3>();
	}

    private void Update()
    {
        if (debug && !staticObject)
            DrawForces();
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (staticObject)
            return;
        force = Vector3.zero;

        Gravity();
        AirResistance();
        ViktorEffekten();
        applyRot();

        acceleration = force / mass;
        velocity += acceleration;
        position += velocity;

        

        Debug.DrawRay(position, force, Color.yellow);
       

        transform.position = this.position;
	} 

    void applyRot()
    {
        rotation += rotAcceleration * Time.deltaTime;

        if (rotation.x > Mathf.PI * 2)
            rotation.x -= Mathf.PI * 2;
        if (rotation.y > Mathf.PI * 2)
            rotation.y -= Mathf.PI * 2;
        if (rotation.z > Mathf.PI * 2)
            rotation.z -= Mathf.PI * 2;

        if (rotation.x < 0)
            rotation.x += Mathf.PI * 2;
        if (rotation.y < 0)
            rotation.y += Mathf.PI * 2;
        if (rotation.z < 0)
            rotation.z += Mathf.PI * 2;

        transform.rotation = Quaternion.Euler(rotation * Mathf.Rad2Deg);
    }

    void Gravity()
    {
        force += Vector3.down * (this.f_gravity * mass * Time.deltaTime * Time.deltaTime)/2;        
    }

    void AirResistance()
    {
        force.x -= ((cD * density * area * velocity.x) / 2) * Time.deltaTime;
        force.y -= ((cD * density * area * velocity.y) / 2) * Time.deltaTime;
        force.z -= ((cD * density * area * velocity.z) / 2) * Time.deltaTime;
    }

    void ViktorEffekten()
    {
        Debug.DrawRay(position, (((cM * density * area * velocity.magnitude) / 2) * Vector3.Cross(rotation, force)), Color.magenta);
        force += (((cM * density * area * velocity.magnitude) / 2) * Vector3.Cross(rotation, velocity.normalized)) * Time.deltaTime;        
    }

    void DrawForces()
    {
        timer += Time.deltaTime;
        if (rayDuration <= 0)
        {
            Debug.DrawRay(position, velocity, Color.green);
            Debug.DrawRay(position, acceleration, Color.red);
        }
        else
        {
            Debug.DrawRay(position, velocity, Color.green, rayDuration);
            Debug.DrawRay(position, acceleration, Color.red, rayDuration);
        }

        if (arch)
        {
            if (timer >= pointIteration)
            {
                timer = 0;
                points.Add(position);
            }
            for (int i = 0; i < points.Count - 1; i++)
            {
                Debug.DrawLine(points[i], points[i + 1], Color.blue);
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
       
        Physics2D physics2D = other.gameObject.GetComponent<Physics2D>();

        physics2D.velocity.x += ((mass * (velocity.x - (velocity.x * boncyness)) + (physics2D.mass * physics2D.velocity.x)) / physics2D.mass) *
                                (-other.contacts[0].normal.x * Mathf.Min(mass / physics2D.mass, 1));
        physics2D.velocity.y += ((mass * (velocity.y - (velocity.y * boncyness)) + (physics2D.mass * physics2D.velocity.y)) / physics2D.mass) *
                                (-other.contacts[0].normal.y * Mathf.Min(mass / physics2D.mass, 1));
        physics2D.velocity.z += ((mass * (velocity.z - (velocity.z * boncyness)) + (physics2D.mass * physics2D.velocity.z)) / physics2D.mass) *
                                (-other.contacts[0].normal.z * Mathf.Min(mass / physics2D.mass, 1));
        
    }
}
