using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OmegaMovementScript : MonoBehaviour
{
    Rigidbody2D omegaRB;
    Vector3 lastPlayerPosition;

    // Start is called before the first frame update
    void Start()
    {
        omegaRB = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            lastPlayerPosition = other.transform.position;
            print("Omega Collider" + lastPlayerPosition);
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.tag == "Player") {
            Vector3 omegaPos = transform.position;
            Vector3 newPlayerPos = other.transform.position;

            if(Mathf.Abs(omegaPos.y - lastPlayerPosition.y) < Mathf.Abs(omegaPos.y - newPlayerPos.y))
            {
                Vector3 move = Input.GetKey(KeyCode.W)? Vector3.up : Vector3.down;
                print("Alpha moving vertically away");
            }
                
            if(Mathf.Abs(omegaPos.x - lastPlayerPosition.x) < Mathf.Abs(omegaPos.x - newPlayerPos.x))
            {
                Vector3 move = Input.GetKey(KeyCode.A)? Vector3.left : Vector3.right;
                print("Alpha moving horizontally away");
            }
            lastPlayerPosition = newPlayerPos;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Player") {
            print("Player exited Omega Collider");
        }
    }


}
