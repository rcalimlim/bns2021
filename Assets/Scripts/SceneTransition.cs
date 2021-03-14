using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad;
    AudioSource audioData;

    void Start()
    {
        audioData = GetComponent<AudioSource>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
	Debug.Log("collision detected");
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            audioData.Play(0);
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
