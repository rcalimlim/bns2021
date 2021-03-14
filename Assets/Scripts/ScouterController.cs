using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ScouterController : MonoBehaviour
{
    public float moveSpeed = 5f;
    float startTime;
    float journeyLength;
    bool isMoving = false;

    float stopMovementTime;
    public float stopMovementDuration = 0.25f;
    bool preventInput = false;

    Transform movePoint;
    Transform priotMovePointLoc;
    LayerMask stopMovement;

    GameObject player;
    PlayerController playerController;

    public AudioClip scouterAudio;
    AudioSource universalAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        // How we are performing movement because we be cheating
        movePoint = transform.GetChild(0);
        movePoint.SetParent(null);
        stopMovement = LayerMask.GetMask("StopMovement");

        player = GameObject.Find("Bryce");
        playerController = player.GetComponent<PlayerController>();

        GameObject audioGameObject = GameObject.Find("Audio_GO");
        universalAudioSource = audioGameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isMoving & !preventInput) {
            Vector3 moveVector = Vector3.zero;
            do
            {
                switch(Random.Range(0,8))
                {
                    case 0:
                        moveVector = Vector3.up; break;
                    case 1:
                        moveVector = Vector3.down; break;
                    case 2:
                        moveVector = Vector3.left; break;
                    case 3:
                        moveVector = Vector3.right; break;
                    case 4:
                        moveVector = Vector3.up + Vector3.left; break;
                    case 5:
                        moveVector = Vector3.up + Vector3.right; break;
                    case 6:
                        moveVector = Vector3.down + Vector3.left; break;
                    case 7:
                        moveVector = Vector3.down + Vector3.right; break;
                }
            } while(Physics2D.OverlapCircle(movePoint.position + moveVector, .2f, stopMovement) 
            || Physics2D.OverlapCircle(movePoint.position + moveVector, .2f)?.gameObject.tag == "Player");

            preventInput = true;
            stopMovementTime = Time.time + stopMovementDuration;

            movePoint.position += moveVector;
            isMoving = true;
            startTime = Time.time;
            journeyLength =  Vector3.Distance(transform.position, movePoint.position);
                
        }

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

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player")
        {
            universalAudioSource.PlayOneShot(scouterAudio, 1f);
            

            Destroy(movePoint.gameObject);
            Destroy(gameObject);

            SceneManager.LoadScene("bns2019+");
            
        }

    }
}
