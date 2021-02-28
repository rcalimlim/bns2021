using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public AudioClip swordBreak;
    
    float startTime;
    float journeyLength;
    bool isMoving = false;

    float stopMovementTime;
    public float stopMovementDuration = 0.25f;
    bool preventInput = false;

    Transform movePoint;
    LayerMask stopMovement;

    PlayerController playerController;
    AudioSource universalAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        // How we are performing movement because we be cheating
        movePoint = transform.GetChild(0);
        movePoint.SetParent(null);

        playerController = GetComponent<PlayerController>();
        stopMovement = LayerMask.GetMask("StopMovement");

        GameObject audioGameObject = GameObject.Find("Audio_GO");
        universalAudioSource = audioGameObject.GetComponent<AudioSource>();

        
    }

    // Update is called once per frame
    void Update()
    {
        // If we're not moving, check the input and then attempt to move
        if(!isMoving & !preventInput) {
            Vector3 moveVector = new Vector3(0f,0f,100f);

            if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
                moveVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
            else if(Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
                moveVector = new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);

            if(moveVector.z == 0f)
            {
                preventInput = true;
                stopMovementTime = Time.time + stopMovementDuration;

                if(Physics2D.OverlapCircle(movePoint.position + moveVector, .2f, stopMovement)) 
                    return;

                if(Physics2D.OverlapCircle(movePoint.position + moveVector, .2f)) 
                {
                    Collider2D collider = Physics2D.OverlapCircle(movePoint.position + moveVector, .2f);

                    
                    
                    EnemyController tileController = collider.gameObject.GetComponent<EnemyController>();
                    bool breakSword = playerController.Attack(tileController);
                    if(breakSword) {
                        universalAudioSource.PlayOneShot(swordBreak);
                        return;
                    }
                }
                
                movePoint.position += moveVector;
                isMoving = true;
                startTime = Time.time;
                journeyLength =  Vector3.Distance(transform.position, movePoint.position);
                
            }
        }

        // If we are moving, update our position over time
        if(isMoving) {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(transform.position, movePoint.position, fractionOfJourney);

            if(Vector3.Distance(transform.position, movePoint.position) == 0f)
                isMoving = false;   
        }
        
        if(preventInput) {
            if(Time.time > stopMovementTime)
                preventInput = false;
        }
    }

    void movePlayer()
    {
        
    }
}
