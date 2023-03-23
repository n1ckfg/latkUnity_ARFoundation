using UnityEngine;
using System.Collections;

public class FakeGravity : MonoBehaviour {

	public Transform target;
	public float floor = 0f;
	public float force = 0.01f;
	public float offset = 3f;
	public bool isGrounded = false;

	void Start() {
		if (target == null)	target = transform;
		floor += offset;
	}

	void LateUpdate () {
		Vector3 p = target.position;
		if (p.y > floor) {
			p.y -= force;
		} else if (p.y < floor) {
			p.y = floor;
		}
		target.position = p;

		if (target.position.y == floor) {
			isGrounded = true;
		} else {
			isGrounded = false;
		}
	}

}
