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

    Text damageText;
    Text buffText;
    LayerMask stopsMovement;
    Transform movePoint;
    SpriteRenderer spriteRenderer;


    // Bryce Tracking
    GameObject bryce;
    Vector3 brycesOldPosition;

    // Start is called before the first frame update
    void Start()
    {
        stopsMovement = LayerMask.GetMask("StopMovement");

        spriteRenderer = GetComponent<SpriteRenderer>();

        movePoint = transform.GetChild(0);
        movePoint.SetParent(null);

        bryce = GameObject.Find("Bryce");
        brycesOldPosition = bryce.transform.position;

        Transform buffTransform = transform.Find("Stat Canvas/Buff");
        if(buffTransform != null) {
            buff = Mathf.Floor(Random.Range(1f, 10f));

            spriteRenderer.sprite = (buff > 5f)? strongSprite : weakSprite;

            buffText = buffTransform.GetComponent<Text>();
            buffText.text = buff.ToString();
        }

        Transform damageTransform = transform.Find("Stat Canvas/Health");
        if(damageTransform != null)
        {
            damage = damage * Mathf.Floor(Random.Range(1f, 10f));

            spriteRenderer.sprite = (Mathf.Abs(damage) > 5f)? strongSprite : weakSprite;
            damageText = damageTransform.GetComponent<Text>();
            damageText.text = Mathf.Abs(damage).ToString();
        }
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
            pc.calculateEffects(damage, buff);

            Destroy(movePoint.gameObject);
            Destroy(gameObject);
        }
    }
}
