using UnityEngine;
using System.Collections;

public class ZombieAI : MonoBehaviour, ITakeDamage {

	public GameObject Owner { get; private set;}
	public GameManager gameManager { get; private set;}
	public TennisPlayer ballDispenser { get; private set;}
	
	public Vector2 Direction { get; set;}
	public Vector2 Velocity { get; private set;}
	public CharacterController2D characterController;
	public ZombieDispenser zombieDispenser;
	public LayerMask CollisionMask;
	public Rigidbody2D rigidBody;
	public int pointsToGiveToPlayer;
	public int damage;
	public AudioClip ZombieKillSound;
	private static System.Random random = new System.Random();
	public Vector3 screenDimensions;  
	public Animator Animator; 

	void Start () {
		screenDimensions = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width, (Screen.height), 1));
	}

	void Update () {
		//Debug.Log("GameManager.Instance.multiplier" + GameManager.Instance.multiplier); 
		 

		if(transform.position.x > screenDimensions.x + .5f || transform.position.x < -screenDimensions.x - .5f){
			GameManager.Instance.ResetPointsToZero();
			GameManager.Instance.ResetMultiplierToOne();
			GameManager.Instance.spawnGroupsHitInARow = 0;
			zombieDispenser.activeZombies --;
			//Debug.Log("zd.activeZombies" + zd.activeZombies);
			//DestroyZombie();
			Destroy(gameObject);
			if(zombieDispenser.activeZombies == 0){
				zombieDispenser.initiateRandomZombieSpawnScenario(); 
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
		DestroyZombie ();
	}
	
	public void Initialize(GameObject owner, Vector2 direction, Vector2 velocity, float horizontalForce){
		zombieDispenser = FindObjectOfType <ZombieDispenser> ();
		characterController = GetComponent<CharacterController2D> ();
		characterController.SetHorizontalForce(horizontalForce);
		direction = new Vector2(0, 1);
		transform.up = direction;
		Owner = owner; 
		Direction = direction;
		Velocity = velocity;
		OnInitialized();
	}

	public void InitializeFromRight(GameObject owner, Vector2 direction, Vector2 velocity, float horizontalForce){
		zombieDispenser = FindObjectOfType <ZombieDispenser> ();
		characterController = GetComponent<CharacterController2D> ();
		characterController.SetHorizontalForce(horizontalForce);
		direction = new Vector2(0, 1);
		transform.up = direction;
		Owner = owner; 
		Direction = direction;
		Velocity = velocity;
		transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
		OnInitialized();
	}
	
	protected virtual void OnInitialized(){
		return;
	}
	
	public virtual void OnTriggerEnter2D(Collider2D other){
		
		if((CollisionMask.value & (1 << other.gameObject.layer)) == 0){
			OnNotCollideWith(other);
			return;
		}
		//		var isOwner = other.gameObject == Owner;
		//		var takeDamageOwner = (ITakeDamage)other.GetComponent (typeof(ITakeDamage));
		//		if (isOwner) {
		//			OnCollideOwner(other, takeDamageOwner);
		//			return;
		//		}
		//		var takeDamage = (ITakeDamage)other.GetComponent (typeof(ITakeDamage));
		//		if (takeDamage != null) {
		//			OnCollideTakeDamage(other, takeDamage);
		//			return;
		//		}
		OnCollideOther(other);
	}
	
	protected virtual void OnNotCollideWith(Collider2D other){
		//Debug.Log(other.gameObject.name + "asdasd ");
	}
	
	protected virtual void OnCollideOwner(Collider2D other, ITakeDamage takeDamage){
		//Debug.Log(other.gameObject.name + "Owner");
	}
	
	protected virtual void OnCollideTakeDamage(Collider2D other, ITakeDamage takeDamage){
		
		//Debug.Log("TAKE DAMAGE"); 
		
		takeDamage.TakeDamage(damage, gameObject);
		DestroyZombie();

		//Debug.Log(other.gameObject.name + "Poop ");
	}
	
	public void OnCollideOther(Collider2D other){    

		Debug.Log("Zombie hits " + other.gameObject.name);

		if(other.gameObject.name == "Ball(Clone)"){ 
			//GameManager.Instance.hasHitZombie = true;
			//Debug.Log("Zombie hits " + other.gameObject.name);
			
			//GameManager.Instance.multiplier ++;
			//GameManager.Instance.multiplier = GameManager.Instance.multiplier + .01f;
			//GameManager.Instance.multiplierString = GameManager.Instance.multiplier.ToString("f2");
			//GameManager.Instance.zombieCash = GameManager.Instance.zombieCash + (100 * GameManager.Instance.multiplier);
			
			zombieDispenser.activeZombies --;
			GameManager.Instance.zombiesKilledOnServe ++;
			Debug.Log("ZKOS " + GameManager.Instance.zombiesKilledOnServe);
			GameManager.Instance.AddPoints(1);
			GameManager.Instance.AddToMultiplier(.01f);
			//Debug.Log("GameManager.Instance.multiplier" + GameManager.Instance.multiplier);
			GameManager.Instance.AddZombieCash(100 * GameManager.Instance.multiplier);
			if(GameManager.Instance.Points > PlayerPrefs.GetInt("HighScore")){
				PlayerPrefs.SetInt("HighScore", GameManager.Instance.Points);
			}
			
			if(zombieDispenser.activeZombies == 0 && !GameManager.Instance.intialBallHasBeenDestroyed){
				GameManager.Instance.spawnGroupsHitInARow ++;
			}
			
			if(zombieDispenser.activeZombies == 0){
				zombieDispenser.initiateRandomZombieSpawnScenario();
			}
			
			DestroyZombie(); 
		}
	}

//	public void DestroyZombie(){
//		Animator.SetTrigger("ZombieSplode");
//		AudioSource.PlayClipAtPoint(ZombieKillSound, transform.position); 
//		Destroy(gameObject);  
//	}

	public void DestroyZombie(){
		StartCoroutine(DestroyZombieCo()); 
	}
		
	private IEnumerator DestroyZombieCo(){
		characterController.SetVerticalForce(10);
		Animator.SetTrigger("ZombieSplode");
		AudioSource.PlayClipAtPoint(ZombieKillSound, transform.position); 
		yield return new WaitForSeconds(1);
		Destroy(gameObject);  
	}
			





	public void determineStartingXLocation(){
		float scenarioNumber = random.Next (1, 5);
		transform.position = new Vector3(transform.position.x, scenarioNumber, transform.position.z);
	}


}
