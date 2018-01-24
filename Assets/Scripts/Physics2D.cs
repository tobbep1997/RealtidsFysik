using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics2D : MonoBehaviour {
    [SerializeField]
    private float mass = 1000;
    [SerializeField]
    private float f_gravity = 9.80665f;

    [SerializeField]
    private Vector2 StartAngle;
    [SerializeField]
    private Vector2 acceleration;
    [SerializeField]
    private Vector2 velocity;
    private Vector2 position;

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
        Gravity(ref acceleration);

        velocity += (acceleration * Mathf.Pow(Time.deltaTime, 2))/2;
        position += velocity * Time.deltaTime;
        
        transform.position = position;
	} 

    void Gravity(ref Vector2 acceleration)
    {
        acceleration += Vector2.down * (this.f_gravity * this.mass * Mathf.Pow(Time.deltaTime, 2))/2;
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
}
