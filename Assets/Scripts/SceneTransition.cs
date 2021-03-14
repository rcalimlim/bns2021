using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField]
    private string sceneToLoad;
    [SerializeField]
    private AudioClip transitionSoundEffect;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            SoundManager.Instance.PlayMusic(transitionSoundEffect);
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
