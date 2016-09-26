using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class VectorMath : MonoBehaviour {

    private Vector3 acceleration;
    private Vector3 velocity;
    private float moveHorizontal;
    private float moveVertical;
    private float proj;
    public float speedLimit;

	// Use this for initialization
	void Start () {
	
	}
	
	void FixedUpdate () {
        // Get key inputs
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        velocity = GetComponent<Rigidbody>().velocity;
        acceleration = new Vector3(moveHorizontal, 0, moveVertical);
        acceleration = Vector3.Normalize(acceleration);
        proj = Vector3.Project(velocity, acceleration).magnitude;

        if (proj <= speedLimit)
        {
            // Accelerate
            velocity += acceleration * Time.fixedDeltaTime;
        }

    }
}
