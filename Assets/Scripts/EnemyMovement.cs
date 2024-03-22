using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Rigidbody2D myRigidBody;
    [SerializeField] float moveSpeed = 5f;
    public int enemyKillScore = 50;
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        myRigidBody.velocity = new Vector2 (moveSpeed , 0f);
    }

   void OnTriggerExit2D(Collider2D other){
    FlipSprite();
    moveSpeed = -moveSpeed;
    }

    void FlipSprite(){
        transform.localScale = new Vector3 (-Math.Sign(myRigidBody.velocity.x), 1f, 1f);
        }
}
