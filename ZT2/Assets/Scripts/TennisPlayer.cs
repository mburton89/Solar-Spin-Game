using UnityEngine;
using System.Collections;

public class TennisPlayer : MonoBehaviour {

	public Ball ball;
	
	void Start () {
	
	}

	void Update () {
	
	}

	public void handleKeyboard(){
		if (Input.GetKeyDown(KeyCode.DownArrow)){
			if(!ball.hasBeenThrown){
				throwBallUp();
			}else if(ball.hasBeenThrown && !ball.hasBeenServed){
				serveBall();
			}
		}

	}

	public void throwBallUp(){
		ball.hasBeenThrown = true;
	}

	public void serveBall(){
		ball.hasBeenServed = true;
	}
}
