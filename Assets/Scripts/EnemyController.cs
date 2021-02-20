using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float damage = 0f;
    public float weapon = 0f;
    public float moveSpeed = 5f;
    LayerMask stopsMovement;
    
    Transform movePoint;

    // Bryce Tracking
    GameObject bryce;
    Vector3 brycesOldPosition;

    // Start is called before the first frame update
    void Start()
    {
        movePoint = transform.GetChild(0);
        stopsMovement = LayerMask.GetMask("StopMovement");
        //print(movePoint.gameObject.name);
        movePoint.SetParent(null);
        bryce = GameObject.Find("Bryce");
        brycesOldPosition = bryce.transform.position;
    }

    private void Update() {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        if (InSightAndRunning())
        {
            Movement.move(transform, movePoint, moveSpeed, stopsMovement);
        }
        brycesOldPosition = bryce.transform.position;
    }

    private bool InSightAndRunning()
    {
        Vector3 enemyPosition = transform.position;
        Vector3 brycesPosition = bryce.transform.position;
        return 
            (enemyPosition.x == brycesOldPosition.x && Mathf.Abs(enemyPosition.y - brycesOldPosition.y) < Mathf.Abs(enemyPosition.y - brycesPosition.y))
        || (enemyPosition.y == brycesOldPosition.y && Mathf.Abs(enemyPosition.x - brycesOldPosition.x) < Mathf.Abs(enemyPosition.x - brycesPosition.x));
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player")
        {
            PlayerController pc = bryce.GetComponent<PlayerController>();
            pc.calculateEffects(damage, weapon);

            Destroy(movePoint.gameObject);
            Destroy(gameObject);
        }
    }
}
