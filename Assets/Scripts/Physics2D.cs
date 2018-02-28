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
    public Vector3 acceleration;
    [SerializeField]
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
    private List<Vector2> points;

	void Start () {
        position = transform.position;
        velocity = StartAngle;

        timer = pointIteration;
        points = new List<Vector2>();
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
        this.position = transform.position;

        Gravity(ref acceleration);
        AirResistance();        
        acceleration += ViktorEffekten(rotation);

        velocity += acceleration;
        position += velocity * Time.deltaTime;

        rotation += rotAcceleration;

        transform.rotation = Quaternion.Euler(rotation);
       

        transform.position = this.position;
	} 

    void applyRot()
    {

    }

    void Gravity(ref Vector3 acceleration)
    {
        acceleration += Vector3.down * (this.f_gravity * mass * Mathf.Pow(Time.deltaTime, 2))/2;        
    }

    void AirResistance()
    {
        velocity += (velocity.normalized * -1.0f) * ((cD * density * area * velocity.magnitude) / 2) * Time.deltaTime;
    }

    Vector3 ViktorEffekten(Vector3 rotation)
    {
        //print(((density * cM * area * velocity.magnitude) / 2) * Vector3.Cross(rotation, velocity) * Time.deltaTime);
        return Vector3.zero;
        //return (((density * cM * area * velocity.magnitude) / 2) * Vector3.Cross(rotation, velocity) * Time.deltaTime)/mass;
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
                points.Add(transform.position);
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
