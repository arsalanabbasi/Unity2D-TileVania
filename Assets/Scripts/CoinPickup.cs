using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] int coinPickupPoints = 100;

    

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player"){
            FindObjectOfType<GameSession>().UpdateScore(coinPickupPoints);
            AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position, 0.25f);
            Destroy(gameObject);
            }
        }

   
}
