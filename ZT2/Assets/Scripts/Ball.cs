using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	public Vector2 Direction { get; set;}
	public Vector2 Velocity { get; private set;}
	public float Speed;

	public bool hasBeenThrown;
	public bool hasBeenServed;

	// Use this for initialization
	void Start () { 
	
	}
	
	// Update is called once per frame
	void Update () {
		determineIfBallIsActive();
		determineBallVelocity();

	}

	public void determineIfBallIsActive(){
		if(transform.position.y > 5.2 || transform.position.y < -5.2){
			destroyBall();
		}
	}

	public void determineBallVelocity(){
		transform.Translate ((Direction + new Vector2 (0, Velocity.y)) * Speed * Time.deltaTime, Space.World);
	}

	public void destroyBall(){

	}
}
