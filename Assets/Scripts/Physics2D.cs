using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics2D : MonoBehaviour {
    [SerializeField]
    private float mass = 1000;
    [SerializeField]
    private float f_gravity = 9.80665f;

    [SerializeField]
    private Vector3 StartAngle;
    [SerializeField]
    private Vector3 acceleration;
    [SerializeField]
    private Vector3 velocity;
    private Vector3 position;

    [SerializeField]
    private float cD = 1.0f, density = 1.0f, area = 2.0f;

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
        if (debug)
            DrawForces();
    }

    // Update is called once per frame
    void FixedUpdate () {
        this.position = transform.position;
        Gravity(ref acceleration);
        AirResistance(ref velocity);

        velocity += acceleration * this.mass;
        position += velocity * Time.deltaTime;
        
        transform.position = this.position;
	} 

    void Gravity(ref Vector3 acceleration)
    {
        acceleration += Vector3.down * (this.f_gravity * Mathf.Pow(Time.deltaTime, 2))/2;        
    }

    void AirResistance(ref Vector3 acceleration)
    {
        velocity += (velocity.normalized * -1.0f) * 0.5f * cD * density * area * velocity.magnitude * Time.deltaTime;
        
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

    void OnTriggerEnter(Collider other)
    {

        print(other.gameObject);
    }
}
