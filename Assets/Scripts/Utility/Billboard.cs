using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {

	public Transform target;

	private bool firstRun = true;

	void Start() {
	}

	void Update() {
		if (firstRun && target == null) {
			target = Camera.main.transform;
			firstRun = false;
		}

		transform.LookAt(target);
	}

}