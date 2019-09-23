using UnityEngine;
using System.Collections;

public class StuffMenu : MonoBehaviour {

	public Ball ball;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		HandleKeyboard();
	}

	private void HandleKeyboard(){
		if (Input.GetKeyDown(KeyCode.P)){
			if(Time.timeScale == 1){
				openStuffMenu();
			}else{
				closeStuffMenu();
			}
		}
	}
	
	private void HandleUserTouches(){
		for (int i = 0; i < Input.touchCount; i++){
			Touch touch = Input.GetTouch(i);
			if (touch.phase == TouchPhase.Began){
				Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
				if(touchPosition.y < 3.5){  
					if(Time.timeScale == 1){
						openStuffMenu();
					}else{
						closeStuffMenu();
					}
				}
			}
		}
	}

	public void openStuffMenu(){
		Time.timeScale = 0;
	}

	public void closeStuffMenu(){
		Time.timeScale = 1;
	}

	public void toggleAmmoToRight(){
		
	}

	public void toggleAmmoToLeft(){
		
	}
}
