using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject player;
    public float timeToMove = 0.2f;

    bool isMoving = false;
    private Vector3 origPos, targetPos, alphaPos;
    


    // Start is called before the first frame update
    void Start()
    {
        alphaPos = player.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if(!isMoving) {
            Follow();
        }
        
        alphaPos = player.transform.position;
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;

        float elapsedTime = 0;

        origPos = transform.position;
        targetPos = origPos + direction;

        while(elapsedTime < timeToMove) {
            transform.position = Vector3.Lerp(origPos, targetPos, elapsedTime / timeToMove);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        isMoving = false;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") 
        {
           Destroy(gameObject);
        }
    }

    void Follow() 
    {
        Vector3 omegaPos = transform.position;
        Vector3 newAlphaPos = player.transform.position;

        if(omegaPos.x == alphaPos.x && Mathf.Abs(omegaPos.y - alphaPos.y) < Mathf.Abs(omegaPos.y - newAlphaPos.y))
        {
            Vector3 move = Input.GetKey(KeyCode.W)? Vector3.up : Vector3.down;
            StartCoroutine(MovePlayer(move));
            print("Alpha moving vertically away");
        }
            
        if(omegaPos.y == alphaPos.y && Mathf.Abs(omegaPos.x - alphaPos.x) < Mathf.Abs(omegaPos.x - newAlphaPos.x))
        {
            Vector3 move = Input.GetKey(KeyCode.A)? Vector3.left : Vector3.right;
            StartCoroutine(MovePlayer(move));
            print("Alpha moving horizontally away");
        }
    }
}
