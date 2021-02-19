using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    Transform movePoint;

    LayerMask stopsMovement;

    // Start is called before the first frame update
    void Start()
    {
        movePoint = transform.GetChild(0);
        stopsMovement = LayerMask.GetMask("StopMovement");
        //print(movePoint.gameObject.name);
        movePoint.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        Movement.move(transform, movePoint, moveSpeed, stopsMovement);

    }
}
