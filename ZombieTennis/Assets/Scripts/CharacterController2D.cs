﻿using UnityEngine;
using System.Collections;

public class CharacterController2D : MonoBehaviour {
	private const float SkinWidth = .02f;
	private const int TotalHorizontalRays = 8;
	private const int TotalVerticalRays = 4;
	
	//private static readonly float SlopeLimitTangant = Mathf.Tan (75f * Mathf.Deg2Rad);
	
	public LayerMask PlatformMask;
	public ControllerParameters2D DefaultParameters;
	public ControllerState2D State{get; private set;}
	public Vector2 Velocity{get{return _velocity;}}
	public bool HandleCollisions {get; set;}
	public ControllerParameters2D Parameters{get{return _overrideParameters ?? DefaultParameters;}} 
	public GameObject StandingOn {get; private set;}
	public bool CanJump{
		get{
			if(Parameters.JumpRestrictions == ControllerParameters2D.JumpBehavior.CanJumpAnywhere)
				return _jumpIn <= 0;
			
			if(Parameters.JumpRestrictions == ControllerParameters2D.JumpBehavior.CanJumpOnGround)
				return State.IsGrounded;
			
			return false;
		}
	}
	
	private Vector2 _velocity;
	private Transform _transform;
	private Vector3 _localScale;
	private BoxCollider2D _boxCollider;
	private ControllerParameters2D _overrideParameters; 
	private float _jumpIn;
	
	private Vector3
		_raycastTopLeft,
		_raycastBottomLeft,
		_raycastBottomRight;
	
	private float
		_verticalDistanceBetweenRays,
		_horizontalDistanceBetweenRays;
	
	public void Awake(){ 
		HandleCollisions = true;
		State = new ControllerState2D ();
		_transform = transform;
		_localScale = transform.localScale;
		_boxCollider = GetComponent<BoxCollider2D>();
		
		var colliderWidth = _boxCollider.size.x * Mathf.Abs (transform.localScale.x) - (2 * SkinWidth);
		_horizontalDistanceBetweenRays = colliderWidth / (TotalVerticalRays - 1);
		
		var colliderHeight = _boxCollider.size.y * Mathf.Abs (transform.localScale.y) - (2 * SkinWidth);
		_verticalDistanceBetweenRays = colliderHeight / (TotalHorizontalRays - 1);
	}
	
	//maybe switch the += with SetForce
	public void AddForce(Vector2 force){
		_velocity += force;
	}
	
	public void SetForce(Vector2 force){
		_velocity = force;
	}
	
	public void SetHorizontalForce(float x){
		_velocity.x = x;
	}
	
	public void SetVerticalForce(float y){
		_velocity.y = y;
	}
	
	public void Jump(){
		AddForce (new Vector2 (0, Parameters.JumpMagnitude));
		_jumpIn = Parameters.JumpFrequency;
	}
	
	public void LateUpdate(){
		_jumpIn -= Time.deltaTime;
		_velocity.y += Parameters.Gravity * Time.deltaTime;
		Move(Velocity * Time.deltaTime);
	}
	
	private void Move(Vector2 deltaMovement){
		var wasGrounded = State.IsCollidingBelow;
		State.Reset ();
		if (HandleCollisions) {
			HandlePlatforms();
			CalculateRayOrigins();
			
			if(deltaMovement.y < 0 && wasGrounded)
				HandleVerticalSlope(ref deltaMovement);
			
			if(Mathf.Abs(deltaMovement.x) > .001f)
				MoveHorizontally(ref deltaMovement);
			
			MoveVertically(ref deltaMovement);
		}
		
		_transform.Translate (deltaMovement, Space.World);
		
		if (Time.deltaTime > 0)
			_velocity = deltaMovement / Time.deltaTime;
		
		_velocity.x = Mathf.Min (_velocity.x, Parameters.MaxVelocity.x);
		_velocity.y = Mathf.Min (_velocity.y, Parameters.MaxVelocity.y);
		
		if (State.IsMovingUpSlope)
			_velocity.y = 0;
	}
	
	private void HandlePlatforms(){
		
	}
	
	private void CalculateRayOrigins(){
		var size = new Vector2(_boxCollider.size.x * Mathf.Abs (_localScale.x), _boxCollider.size.y * Mathf.Abs (_localScale.y)) / 2;
		var center = new Vector2 (_boxCollider.offset.x * _localScale.x, _boxCollider.offset.y * _localScale.y);
		
		_raycastTopLeft = _transform.position + new Vector3 (center.x - size.x + SkinWidth, center.y + size.y - SkinWidth);
		_raycastBottomRight = _transform.position + new Vector3 (center.x + size.x - SkinWidth, center.y - size.y + SkinWidth);
		_raycastBottomLeft = _transform.position + new Vector3 (center.x - size.x + SkinWidth, center.y - size.y + SkinWidth);
	}
	
	private void MoveHorizontally(ref Vector2 deltaMovement){
		var isGoingRight = deltaMovement.x > 0;
		var rayDistance = Mathf.Abs (deltaMovement.x) + SkinWidth;
		var rayDirection = isGoingRight ? Vector2.right : -Vector2.right;
		var rayOrigin = isGoingRight ? _raycastBottomRight : _raycastBottomLeft;
		
		for (var i = 0; i < TotalHorizontalRays; i++) {
			var rayVector = new Vector2(rayOrigin.x, rayOrigin.y + (i * _verticalDistanceBetweenRays));
			Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.red);
			
			var rayCastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, PlatformMask);
			if(!rayCastHit)
				continue;
			
			if(i == 0 && HandleHorizontalSlope(ref deltaMovement, Vector2.Angle(rayCastHit.normal, Vector2.up), isGoingRight))
				break;
			
			deltaMovement.x = rayCastHit.point.x - rayVector.x;
			rayDistance = Mathf.Abs (deltaMovement.x);
			
			if(isGoingRight){
				deltaMovement.x -= SkinWidth;
				State.IsCollidingRight = true;
			}
			else{
				deltaMovement.x += SkinWidth;
				State.IsCollidingLeft = true;
			}
			
			if(rayDistance < SkinWidth + .0001f)
				break;
		}
	}
	
	private void MoveVertically(ref Vector2 deltaMovement){
		var isGoingUp = deltaMovement.y > 0;
		var rayDistance = Mathf.Abs(deltaMovement.y) + SkinWidth;
		var rayDirection = isGoingUp ? Vector2.up : -Vector2.up;
		var rayOrigin = isGoingUp ? _raycastTopLeft : _raycastBottomLeft;
		
		rayOrigin.x += deltaMovement.x;
		
		var standingOnDistance = float.MaxValue;
		for (var i = 0; i < TotalVerticalRays; i++) {
			var rayVector = new Vector2(rayOrigin.x + (i * _horizontalDistanceBetweenRays), rayOrigin.y);
			Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.red);
			
			var raycastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, PlatformMask);
			if(!raycastHit)
				continue;
			
			if(!isGoingUp){
				var verticalDistanceToHit = _transform.position.y - raycastHit.point.y;
				if(verticalDistanceToHit < standingOnDistance){
					standingOnDistance = verticalDistanceToHit;
					StandingOn = raycastHit.collider.gameObject;
				}
			}
			
			deltaMovement.y = raycastHit.point.y - rayVector.y;
			rayDistance = Mathf.Abs (deltaMovement.y);
			
			if(isGoingUp){
				deltaMovement.y -= SkinWidth;
				State.IsCollidingAbove = true;
			}else{
				deltaMovement.y += SkinWidth;
				State.IsCollidingBelow = true;
				State.IsGrounded = true; // <-------------- THIS IS AN EXPERIMENT. TAKE THIS OUT MAYBE
			}
			
			if(!isGoingUp && deltaMovement.y > .0001f)
				State.IsMovingUpSlope = true;
			
			if(rayDistance < SkinWidth + .0001f)
				break;
		}
	}
	
	private bool HandleHorizontalSlope(ref Vector2 deltaMovement, float angle, bool isGoingRight){
		return false;
	}
	
	private void HandleVerticalSlope(ref Vector2 deltaMovement){
		
	}
	
	public void OnTriggerEnter2D(Collider2D other){
		
	}
	
	public void OnTriggerExit2D(Collider2D other){
		
	}
}
