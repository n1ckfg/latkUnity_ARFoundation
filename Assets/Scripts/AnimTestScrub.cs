using UnityEngine;
using System.Collections;

public class AnimTestScrub : MonoBehaviour {

	public Animator animator;
	public string clipName = "Take 001";
	public int clipLayer = 0;
	public float frameInterval = 12f;

	private float val = 0f;
	private float markTime = 0f;
	private float normalizedFrameInterval = 0f;

	void Awake() {
		if (!animator) animator = GetComponent<Animator>();
	}

	void Start() {
		frameInterval = 1f / frameInterval;
		normalizedFrameInterval = frameInterval / animator.GetCurrentAnimatorStateInfo(clipLayer).length;
		markTime = Time.realtimeSinceStartup;
	}

	void Update () {
		if (Time.realtimeSinceStartup > markTime + frameInterval) {
			val += normalizedFrameInterval;
			markTime = Time.realtimeSinceStartup;
			if (val > 1f) val = 0f;
		}
		animator.Play(clipName, clipLayer, val);
	}

}
