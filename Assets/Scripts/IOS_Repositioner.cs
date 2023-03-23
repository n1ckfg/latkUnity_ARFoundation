using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOS_Repositioner : MonoBehaviour {

	public float xShift = 0.5f;

	void Start() {
		#if UNITY_IOS
		transform.Translate(xShift, 0f, 0f);
		#endif
	}

}
