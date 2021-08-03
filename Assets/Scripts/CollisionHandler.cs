using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float delay = 2f;
    [SerializeField] AudioClip explosionSound;
    [SerializeField] AudioClip successSound;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem explosionParticles;


    AudioSource audioSource;

    bool isTransitioning = false;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void OnCollisionEnter(Collision other)
    {
        if(isTransitioning)
        {
            return;
        }

        switch(other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Friendly");
                break;
            case "Finish":
                Debug.Log("This is the Finish");
                NextLevelSequence();
                break;
            case "Fuel":
                Debug.Log("Fuel picked");
                break;
            default:
                Debug.Log("You Explode!");
                StartCrash();
                break;
        }
    }

    void StartCrash()
    {
        isTransitioning = true;
        audioSource.Stop();
        explosionParticles.Play();
        GetComponent<Movement>().enabled = false;
        audioSource.PlayOneShot(explosionSound);
        Invoke("ReloadLevel", delay);
    }
    void NextLevelSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        successParticles.Play();
        GetComponent<Movement>().enabled = false;
        audioSource.PlayOneShot(successSound);
        Invoke("LoadNextLevel", delay);
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
