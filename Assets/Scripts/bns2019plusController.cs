using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class bns2019plusController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player")
        {
            //SceneManager.LoadScene("Bryce Bedroom");
            SceneManager.LoadScene("MainMenu");
        }
    }
}
