using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    Vector2 inputValue;
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    SpriteRenderer mySpriteRenderer;
    float startGravityScale;
    bool isAlive = true;
    bool isVulnerable = false;
    Vector2 deathKick = new Vector2(2f, 10f);

    [Header ("Movements")]
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float jumpSpeedY = 10f;
    [SerializeField] float climbSpeedY = 6f;

    [Header ("Color")]
    [SerializeField] Color32 deathColor = new Color32 (1, 1, 1, 1);
  
    [Header ("Arrow")]
    [SerializeField] GameObject arrow;
    [SerializeField] Transform arrowSpawnPoint;

    [Header ("Audio SFXs")]
    [SerializeField] AudioClip jumpSFX;
    [SerializeField] AudioClip gameStartSFX;
    [SerializeField] AudioClip arrowSFX;
    [SerializeField] AudioClip playerHitSFX;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        AudioSource.PlayClipAtPoint(gameStartSFX, Camera.main.transform.position, 0.35f);
        
        startGravityScale = myRigidBody.gravityScale;
    }

    
    void Update()
    {
        if (!isAlive){ return; } 
        Run();
        ClimbingLadder();
        FlipSprite();
        Die();
    }

    void OnMove(InputValue value){
        if (!isAlive){ return; } 
        inputValue = value.Get<Vector2>();
        }

    void OnJump(InputValue value){
        if (!isAlive){ return; } 
        if(myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) || 
           myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Bouncy")) ){
            if(value.isPressed){
                myRigidBody.velocity += new Vector2 (0f, jumpSpeedY);
                AudioSource.PlayClipAtPoint(jumpSFX, Camera.main.transform.position, 0.35f);
                }
            }

        }

    void OnFire(InputValue value){
        if (!isAlive || myAnimator.GetBool("isIdle")){ return; }
        Instantiate(arrow, arrowSpawnPoint.position, transform.rotation);
        AudioSource.PlayClipAtPoint(arrowSFX, Camera.main.transform.position, 0.35f);
        myAnimator.SetTrigger("Shoot");
        }
    
    void OnRollOver(InputValue value){
        if (!isAlive || myAnimator.GetBool("isIdle")){ return; }
            isVulnerable = true;
            myAnimator.SetTrigger("Roll Over");
            Invoke("SetVulneralibityOff", 0.9f);
            }

    void Run(){  

        Vector2 playerVelocity = new Vector2 (inputValue.x * movementSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        myAnimator.SetBool("isRunning", Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon);
            
        }

    void FlipSprite(){
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        
        if (playerHasHorizontalSpeed){
            transform.localScale = new Vector3 (Math.Sign(myRigidBody.velocity.x), 1f, 1f);
            }
        
        }

    void ClimbingLadder(){
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))){
            Vector2 climbVelocity = new Vector2 (myRigidBody.velocity.x, inputValue.y * climbSpeedY);
            myRigidBody.velocity = climbVelocity;

            bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
            myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
            myAnimator.SetBool("isIdle", true);
            myRigidBody.gravityScale = 0;
            }
        
        else{           
            myRigidBody.gravityScale = startGravityScale;
            }
            
        }

    void Die(){
        if(isVulnerable){   return; }
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies")) ||
           myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Hazards"))){
            
            AudioSource.PlayClipAtPoint(playerHitSFX, Camera.main.transform.position, 1f);
            myRigidBody.gravityScale = startGravityScale;
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            mySpriteRenderer.color = deathColor;
            myRigidBody.velocity += deathKick;
            myBodyCollider.enabled = false;
            myFeetCollider.enabled = false;

            FindObjectOfType<GameSession>().PlayerDeath();
           }
        
        }

    void SetVulneralibityOff(){
        isVulnerable = false;
        }
    
    void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Ladder"){
            myAnimator.SetBool("isIdle", false);
            myAnimator.SetBool("isClimbing", false); 
            }
        }
        
}
