using UnityEngine;
using System.Collections;

public class HideOnStart : MonoBehaviour {

	public Renderer[] renderer;

	void Start() {
		for (int i=0; i<renderer.Length; i++) {
			renderer[i].enabled = false; 
		}
	}

}
