using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;

    void Awake() {
        int numOfGameSessions = FindObjectsOfType<GameSession>().Length;
        
        if(numOfGameSessions > 1){
            Destroy(gameObject);
            }
        else{
            DontDestroyOnLoad(gameObject);
            }
        }

    void Start(){
        // GetComponent<AudioSource>().Play();
        livesText.text = "Lives: " + playerLives.ToString(); 
        scoreText.text = "Score: " + score.ToString();
        }
    
    public void PlayerDeath(){
        if(playerLives > 1){
            TakeLife();
            }
        else{
            ResetGameSession();
            }
        
        }
    
    public void UpdateScore(int pointsToAdd){
        score += pointsToAdd;
        scoreText.text = "Score: " + score.ToString();
        }

    void ResetGameSession(){
        SceneManager.LoadScene(0);
        FindObjectOfType<ScenePersist>().ResetScreenPersist();
        Destroy(gameObject);
        }

    void TakeLife(){
        playerLives --;
        livesText.text = "Lives: " + playerLives.ToString();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

}
