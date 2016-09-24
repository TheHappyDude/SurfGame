using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]

public class VectorBasedController : MonoBehaviour {

	public float speed = 10.0f;
	public float gravityLo = 9.81f;
	public float gravityHi = 20f;
	public float maxVelocityChange = 10.0f;
	public bool canJump = true;
	public float jumpHeight = 2.0f;
	private bool grounded = false;
	public float flyControl = 4;
	private float downVelocity = 0;
	private float rampAngle = 0;
	private Vector3 playerDirection = Vector3.zero;
	private Vector3 rampDirection = Vector3.zero;

	void Awake () {
		GetComponent<Rigidbody>().freezeRotation = true;
		GetComponent<Rigidbody>().useGravity = false;
	}

	void FixedUpdate () {
		// Get player direction. Ramp direction is handled in collision
		playerDirection = transform.forward;


		// Calculate how fast we should be moving
		Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		targetVelocity = transform.TransformDirection(targetVelocity);
		targetVelocity *= speed;

		// Apply a force that attempts to reach our target velocity
		Vector3 velocity = GetComponent<Rigidbody>().velocity;
		Vector3 velocityChange = (targetVelocity - velocity);
		velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
		velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
		velocityChange.y = 0;

		if (grounded) {
			if (Input.GetAxis ("Horizontal") == 0) {
				downVelocity -= gravityHi * rampAngle / 10;
			} 
			GetComponent<Rigidbody> ().AddForce (velocityChange, ForceMode.VelocityChange);
			GetComponent<Rigidbody>().AddForce (new Vector3 (0, downVelocity * GetComponent<Rigidbody>().mass, 0));			// Gravity
			// Jump
			if (canJump && Input.GetButton ("Jump")) {
				GetComponent<Rigidbody> ().velocity = new Vector3 (velocity.x, CalculateJumpVerticalSpeed (), velocity.z);
			}
		} else {
			downVelocity = 0;
			// Make direction control harder if not grounded
			GetComponent<Rigidbody>().AddForce (velocityChange / flyControl, ForceMode.VelocityChange);
			GetComponent<Rigidbody>().AddForce (new Vector3 (0, -gravityLo * GetComponent<Rigidbody>().mass, 0));			// Gravity
		}

		grounded = false;
	}

	void OnCollisionStay (Collision collisionInfo) {
		grounded = true;    
		rampAngle = collisionInfo.transform.rotation.z;
		rampDirection = collisionInfo.transform.eulerAngles;
		Debug.Log (rampDirection);
	}

	float CalculateJumpVerticalSpeed () {
		// From the jump height and gravity we deduce the upwards speed 
		// for the character to reach at the apex.
		return Mathf.Sqrt(2 * jumpHeight * gravityLo);
	}
}