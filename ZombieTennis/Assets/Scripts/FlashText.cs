using UnityEngine;
using System.Collections;

public class FlashText : MonoBehaviour {

	public Animator animator;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void flashDoubleKill(){
		animator.SetTrigger("DoubleKill");
	}

	public void flashTripleKill(){
		animator.SetTrigger("TripleKill");
	}

	public void flashQuadroupleKill(){
		animator.SetTrigger("QuadroupleKill");
	}
}
