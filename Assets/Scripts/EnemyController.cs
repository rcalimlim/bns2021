using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float damage = 0f;
    public float buff = 0f;
    public float moveSpeed = 5f;

    public Sprite weakSprite;
    public Sprite strongSprite;
    public AudioClip enemyAudio;

    Text damageText;
    Text buffText;
    LayerMask stopsMovement;
    Transform movePoint;
    SpriteRenderer spriteRenderer;

    AudioSource universalAudioSource;



    // Bryce Tracking
    GameObject player;
    PlayerController playerController;
    Transform playerMovePoint;
    Vector3 playerOldPosition;

    // Movement Control
    float startTime;
    float journeyLength;
    bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        stopsMovement = LayerMask.GetMask("StopMovement");

        spriteRenderer = GetComponent<SpriteRenderer>();

        movePoint = transform.GetChild(0);
        movePoint.SetParent(null);

        player = GameObject.Find("Bryce");
        playerController = player.GetComponent<PlayerController>();
        playerMovePoint = GameObject.Find("Bryce Move Point").transform;
        playerOldPosition = playerMovePoint.position;

        Transform buffTransform = transform.Find("Stat Canvas/Buff");
        if (buffTransform != null)
        {
            buff = Mathf.Floor(Random.Range(1f, 10f));

            spriteRenderer.sprite = (buff > 5f) ? strongSprite : weakSprite;

            buffText = buffTransform.GetComponent<Text>();
            buffText.text = buff.ToString();
        }

        Transform damageTransform = transform.Find("Stat Canvas/Health");
        if (damageTransform != null)
        {
            damage = damage * Mathf.Floor(Random.Range(1f, 10f));

            spriteRenderer.sprite = (Mathf.Abs(damage) > 5f) ? strongSprite : weakSprite;
            damageText = damageTransform.GetComponent<Text>();
            damageText.text = Mathf.Abs(damage).ToString();
        }

        GameObject audioGameObject = GameObject.Find("Audio_GO");
        universalAudioSource = audioGameObject.GetComponent<AudioSource>();

    }

    private void Update()
    {
        // Did we fall too far out of bounds?
        if (transform.position.x > 3 || transform.position.x < -3 || transform.position.y > 3 || transform.position.y < -3)
        {
            Destroy(movePoint.gameObject);
            Destroy(gameObject);
        }

        if (!Vector3.Equals(playerOldPosition, playerMovePoint.position) && !isMoving)
        {

            if (movePoint.position.x == playerOldPosition.x && movePoint.position.x == playerMovePoint.position.x)
            {
                if (playerMovePoint.position.y > playerOldPosition.y && playerMovePoint.position.y > movePoint.position.y)
                {
                    movePoint.position += Vector3.up;
                    isMoving = true;
                    startTime = Time.time;
                    journeyLength =  Vector3.Distance(transform.position, movePoint.position);
                }

                else if (playerMovePoint.position.y < playerOldPosition.y && playerMovePoint.position.y < movePoint.position.y)
                { 
                    movePoint.position += Vector3.down;
                    isMoving = true;
                    startTime = Time.time;
                    journeyLength =  Vector3.Distance(transform.position, movePoint.position);
                }
            }
            else if (movePoint.position.y == playerOldPosition.y && movePoint.position.y == playerMovePoint.position.y)
            {
                if (playerMovePoint.position.x > playerOldPosition.x && playerMovePoint.position.x > movePoint.position.x)
                {
                    movePoint.position += Vector3.right;
                    isMoving = true;
                    startTime = Time.time;
                    journeyLength =  Vector3.Distance(transform.position, movePoint.position);
                }
                else if (playerMovePoint.position.x < playerOldPosition.x && playerMovePoint.position.x < movePoint.position.x)
                { 
                    movePoint.position += Vector3.left; 
                    isMoving = true;
                    startTime = Time.time;
                    journeyLength =  Vector3.Distance(transform.position, movePoint.position); 
                }
            }

            playerOldPosition = playerMovePoint.position;
        }

        if (isMoving)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(transform.position, movePoint.position, fractionOfJourney);

            if (Vector3.Distance(transform.position, movePoint.position) == 0f)
                isMoving = false;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            AudioClip overrideSound = playerController.CalculateEffects(damage, buff);

            if (overrideSound != null)
                universalAudioSource.PlayOneShot(overrideSound);
            else
                universalAudioSource.PlayOneShot(enemyAudio, 1f);

            Destroy(movePoint.gameObject);
            Destroy(gameObject);
        }
        
        if(other.tag == "Objective")
        {
            Destroy(movePoint.gameObject);
            Destroy(gameObject);
        }
    }

    public void UpdateDamage(float number)
    {
        damage = number;
        if (damageText != null)
            damageText.text = Mathf.Abs(number).ToString();
    }
}
