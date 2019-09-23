using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour, ITakeDamage {
	
	//public GameObject fastPaddle;
	public GameObject Owner { get; private set;}
	//public OpponentAI opponent;
	//public Background backGround { get; private set;}
	public GameManager gameManager { get; private set;}
	public TennisPlayer ballDispenser { get; private set;}
	public Vector2 Direction { get; set;}
	public Vector2 Velocity { get; private set;}
	public CharacterController2D characterController;
	public LayerMask CollisionMask;
	public AudioClip GoalSound;
	//public Animator Animator;
	public Rigidbody2D rigidBody;
	public ZombieDispenser zombieDispenser;
	public GUISkin Skin;
	public AudioClip HitBallSound;
	public AudioClip NiceHitBallSound;
	public FlashText flashText;
	
	private static System.Random random = new System.Random();
	
	public float Speed;
	public float timeToLive;
	public int damage;
	public int pointsToGiveToPlayer;
	public bool isActive {get; set;}
	public bool isDeflected{get; set;}
	public bool ballIsInAir = false;
	public bool ballHasBeenServed;
	Vector3 lastPosition = Vector3.zero;

	public float ZZoneMin = -0.1f;
	public float YZoneMin = -0.2f;
	public float XZoneMin = -0.4f;
	public float WZoneMin = -0.8f;
	public float VZoneMin = -1.6f;

	public float ZSpeed = 16;
	public float YSpeed = 8;
	public float XSpeed = 4;
	public float WSpeed = 2;
	public float VSpeed = 1;
	public float currentServeSpeed; 
	public int currentServeSpeedInt; 
	//public bool hasHitZombie;

	public void Awake(){
		//opponent = FindObjectOfType<OpponentAI> ();
	}
	
	public void Start () {
		//rigidBody = GetComponent<Rigidbody2D> ();
		//flashText = GetComponent<FlashText> ();
		characterController = GetComponent<CharacterController2D> ();
		ballDispenser = FindObjectOfType<TennisPlayer> ();
		zombieDispenser = FindObjectOfType<ZombieDispenser> ();
		isActive = true;
	}

	void Update () {
		if(transform.position.y > 5.2){
			GameManager.Instance.intialBallHasBeenDestroyed = true;
//			if(!GameManager.Instance.hasHitZombie){
//				GameManager.Instance.ResetPointsToZero();
//			}
			//GameManager.Instance.hasHitZombie = false; 

			//Debug.Log("SPEED:" + Speed);
			//Debug.Log("VForce:" + relativeSpeed);   
			//Debug.Log ("Ball Destroyed");
			ResetGame();
			return;
		}
		
		if(transform.position.y < -5.2){  
			ResetGame();
			return;
		}


		transform.Translate ((Direction + new Vector2 (0, Velocity.y)) * Speed * Time.deltaTime, Space.World);
		isActive = true;
		
		HandleKeyboard();
		HandleUserTouches();


	}

//	void FixedUpdate(){
//		relativeSpeed = (transform.position - lastPosition).magnitude;
//		lastPosition = transform.position;
//
////		relativeSpeed = (transform.position.y - lastPosition.y);
////		lastPosition = transform.position;	
////		Debug.Log("VForce:" + relativeSpeed);  
//	}

	public void HandleUserTouches(){
		if(ballDispenser.hasThrownBall){
			for (int i = 0; i < Input.touchCount; i++){
				Touch touch = Input.GetTouch(i);
				if (touch.phase == TouchPhase.Began && touch.tapCount == 1){
					Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);  
					if(touchPosition.y < 0 && !ballHasBeenServed){
						serveBall(touchPosition.x * 4);    
					}
				}
			}
		}
	}

	public void HandleKeyboard(){   
		if(ballDispenser.hasThrownBall && !ballHasBeenServed){
			if (Input.GetKeyDown(KeyCode.UpArrow)){
				serveBall(0);
			}
			if (Input.GetKeyDown(KeyCode.LeftArrow)){
				serveBall(-2);
			}
			if (Input.GetKeyDown(KeyCode.RightArrow)){
				serveBall(2);
			}
		}
	}
	
	public void TakeDamage(int damage, GameObject instigator){
		if (pointsToGiveToPlayer != 0) {
			//			var projectile = instigator.GetComponent<Ball>();
			//			if(projectile != null && projectile.Owner.GetComponent<Paddle>() != null){
			//				//GameManager.Instance.AddPoints(PointsToGiveToPlayer);
			//			}
		}
		DestroyBall ();
	}
	
	public void Initialize(GameObject owner, Vector2 direction, Vector2 velocity){

		direction = new Vector2(0, 1);

		transform.up = direction;
		Owner = owner;
		Direction = direction;
		Velocity = velocity;
		//Animator.SetTrigger("Dispense");
		OnInitialized();
	}
	
	protected virtual void OnInitialized(){
		return;
	}
	
	public virtual void OnTriggerEnter2D(Collider2D other){
		//Debug.Log("TAKE OnTriggerEnter2D"); 
		if((CollisionMask.value & (1 << other.gameObject.layer)) == 0){
			OnNotCollideWith(other);
			return;
		}
		OnCollideOther(other);
	}
	
	protected virtual void OnNotCollideWith(Collider2D other){
		//Debug.Log("TAKE OnNotCollideWith"); 
	}
	
	protected virtual void OnCollideOwner(Collider2D other, ITakeDamage takeDamage){
		//Debug.Log("TAKE OnCollideOwner"); 
	}
	
	protected virtual void OnCollideTakeDamage(Collider2D other, ITakeDamage takeDamage){
		//Debug.Log("TAKE OnCollideTakeDamage"); 
		takeDamage.TakeDamage(damage, gameObject);
		DestroyBall();
	}
	
	public void OnCollideOther(Collider2D other){  
		//Debug.Log("Ball hits " + other.gameObject.name); 
		//hasHitZombie = true;
	} 

	private void DestroyBall(){
		Destroy(gameObject);
	}
	
	private void ResetGame(){
		GameManager.Instance.displayMultiKillGraphic();
		GameManager.Instance.zombiesKilledOnServe = 0;
		ballDispenser.hasThrownBall = false;
		DestroyBall();
		isActive = false;
	}

//	public void determineServeSpeedAndServe(){
//		float height = transform.position.y;
//		if(height > ZZoneMin){
//			characterController.SetVerticalForce(ZSpeed); 
//			return;
//		}else if(height > YZoneMin){
//			characterController.SetVerticalForce(YSpeed); 
//			return;
//		}else if(height > XZoneMin){
//			characterController.SetVerticalForce(XSpeed); 
//			return;
//		}else if(height > WZoneMin){
//			characterController.SetVerticalForce(WSpeed); 
//			return;
//		}else if(height > VZoneMin){
//			characterController.SetVerticalForce(VSpeed); 
//			return;
//		}
//	}

	public void serveBall(float horizontalForce){

		AudioSource.PlayClipAtPoint(HitBallSound, transform.position); 
		characterController.DefaultParameters.Gravity = 0;
		//float serveSpeed = (.24f - relativeSpeed) * 36;
		//Debug.Log("YPos: " + transform.position.y);
		float serveSpeed = (3 + transform.position.y) * 12 ;
		currentServeSpeed = serveSpeed * 10;
		currentServeSpeedInt = (int)currentServeSpeed;
		characterController.SetVerticalForce(serveSpeed);
		characterController.SetHorizontalForce(horizontalForce);
		ballHasBeenServed = true;

		if(currentServeSpeedInt > 104){
			//Debug.Log("Hi Zpace");
			AudioSource.PlayClipAtPoint(NiceHitBallSound, transform.position); 
		}
	}

	public void OnGUI(){
		GUI.skin = Skin;
		
		GUILayout.BeginArea(new Rect(0 ,0, Screen.width, Screen.height)); //also added padding for GameSkin on Inspector
		{
			GUILayout.BeginVertical(Skin.GetStyle("EnemyKillText"));
			{
				if(Application.loadedLevel == 0){
					GUILayout.Label(string.Format("{0}", currentServeSpeedInt), Skin.GetStyle("EnemyKillText")); 
				}
				//		UNCOMMENT FOR TIME GUI
				//				var time = LevelManager.Instance.RunningTime;
				//				GUILayout.Label(string.Format(
				//					"{0:00}:{1:00} with {2} bonus", 
				//					time.Minutes + (time.Hours * 60), 
				//					time.Seconds,
				//				    LevelManager.Instance.CurrentTimeBonus), Skin.GetStyle("TimeText"));
			}
			GUILayout.EndVertical();
		}
		GUILayout.EndArea();
	}
}