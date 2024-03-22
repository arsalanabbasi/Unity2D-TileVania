using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPortal : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] AudioClip checkpointSFX;

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player")
            AudioSource.PlayClipAtPoint(checkpointSFX, Camera.main.transform.position, 0.35f);
            StartCoroutine(LoadNextLevel());
        }


    IEnumerator LoadNextLevel(){
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        FindObjectOfType<ScenePersist>().ResetScreenPersist();
        SceneManager.LoadScene(currentSceneIndex + 1);
        }
}
