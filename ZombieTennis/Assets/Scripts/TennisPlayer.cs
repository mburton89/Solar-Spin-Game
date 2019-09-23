using UnityEngine;
using System.Collections;

public class TennisPlayer : MonoBehaviour {
	
	private CharacterController2D controller;
	public GameManager gameManager;
	public virtual Vector2 direction { get; protected set; }
	public virtual Vector2 down { get; protected set; }
	public virtual Vector2 _startPosition { get; protected set; }
	public Ball ball;
	public AudioClip DispenseSound;
	public Animator Animator;
	public bool hasThrownBall;
	
	public float FireRate;
	private float _canFireIn;
	
	public virtual void Start(){
		gameManager = FindObjectOfType<GameManager> ();  
	}  
	
	public void Update(){
		HandleKeyboard();
		HandleUserTouches();
	} 
	
	public void FireBall(){  
		if(!hasThrownBall){
			//Debug.Log("NATHALIE IS THE BOMB DOT COM");
			hasThrownBall = true;
			AudioSource.PlayClipAtPoint(DispenseSound, transform.position); 
			Vector3 projectileSpawnLocation = new Vector3(transform.position.x + direction.x, transform.position.y, transform.position.z); 
			var gameBall = (Ball)Instantiate (ball, projectileSpawnLocation, transform.rotation); 
			gameBall.Initialize (gameObject,
			                     direction,
			                     new Vector2(0, 1));  
			gameObject.GetComponent<SpriteRenderer>().enabled = false; 
		}
	}
	
	private void HandleKeyboard(){
		if (Input.GetKeyDown(KeyCode.DownArrow)){
			FireBall();
		}
	}
	
	private void HandleUserTouches(){
		for (int i = 0; i < Input.touchCount; i++){
			Touch touch = Input.GetTouch(i);
			if (touch.phase == TouchPhase.Began){
				Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
				if(touchPosition.y < 0){  
						FireBall();
				}
			}
		}
	}
}
