using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {
	public float speedMultiplier;
	public float mouseSensitivity;
	public float headRange = 90;
	public float rotY = 0;
	public float jumpSpeed = 20;
	private Vector3 velocity = Vector3.zero;
	CharacterController cc;

	// Use this for initialization
	void Start () {
		Cursor.visible = false;
		cc = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		// Camera
		float rotX = Input.GetAxis ("Mouse X") * mouseSensitivity;
		transform.Rotate (0, rotX, 0);
		rotY -= Input.GetAxis ("Mouse Y") * mouseSensitivity;
		rotY = Mathf.Clamp (rotY, -headRange, headRange);
		Camera.main.transform.localRotation = Quaternion.Euler (rotY, 0, 0);

		// Movement
		float strafe = Input.GetAxis ("Horizontal") * speedMultiplier;
		float forward = Input.GetAxis ("Vertical") * speedMultiplier;
		velocity.y += Physics.gravity.y * Time.deltaTime;
		Vector3 speed = new Vector3 (strafe, velocity.y, forward);

		if (cc.isGrounded) {
			if (Input.GetButtonDown ("Jump")) {
				velocity.y = jumpSpeed;
			}
	
		}

		speed = transform.rotation * speed;
		cc.Move (speed * Time.deltaTime);
	}
}
