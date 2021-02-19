using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementGrid : MonoBehaviour
{
    public GameObject enemy;
    private bool isMoving;
    private Vector3 origPos, targetPos;
    private float timeToMove = 0.2f;

    void Update()
    {
        Vector3 moveDirection = Vector3.zero;
        // input
        if (Input.GetKey(KeyCode.W) && !isMoving)
            moveDirection = Vector3.up;
            

        if (Input.GetKey(KeyCode.A) && !isMoving)
            moveDirection = Vector3.left;
           
        
        if (Input.GetKey(KeyCode.S) && !isMoving)
            moveDirection = Vector3.down;
            
        
        if (Input.GetKey(KeyCode.D) && !isMoving)
            moveDirection = Vector3.right;
        
        if (moveDirection != Vector3.zero) {
            StartCoroutine(MovePlayer(moveDirection));
            createEnemy(moveDirection);
        }
            
    }

    private void createEnemy(Vector3 moveDirection) {
        //Instantiate(enemy, transform.position, Quaternion.identity);
       
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
}
