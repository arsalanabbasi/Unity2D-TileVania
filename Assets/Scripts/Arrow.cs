using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody2D myRigidBody;
    PlayerMovement player;
    float xSpeed;
    [SerializeField] float arrowSpeed = 10f;
    [SerializeField] AudioClip enemyHitSFX;
    EnemyMovement enemy;
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        enemy = FindObjectOfType<EnemyMovement>();

        xSpeed = player.transform.localScale.x * arrowSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        myRigidBody.velocity = new Vector2 (xSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enemies"){
            AudioSource.PlayClipAtPoint(enemyHitSFX, Camera.main.transform.position, 0.35f);
            FindObjectOfType<GameSession>().UpdateScore(enemy.enemyKillScore);
            Destroy(other.gameObject);
            Destroy(gameObject);
            }
        }

    void OnCollisionEnter2D(Collision2D other) {
        Destroy(gameObject);
        }
}
