using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float maxHealth = 10f;
    public float maxBuff = 10f;

    float score = 0f;

    float health;
    float buff;

    Transform movePoint;
    Vector3 oldPosition;

    LayerMask stopsMovement;
    TileSpawner spawner;
    
    public Text playerHealthText;
    public Text playerBuffText;
    public Text playerScoreText;

    // Start is called before the first frame update
    void Start()
    {
        // How we are performing movement because we be cheating
        movePoint = transform.GetChild(0);
        movePoint.SetParent(null);
        oldPosition = movePoint.position;

        // needed for Wall collisions for Movepoint
        stopsMovement = LayerMask.GetMask("StopMovement");
        spawner = GetComponent<TileSpawner>();
        
        // Set up Player Stats
        health = maxHealth;
        buff = 0f;

        playerHealthText.text = health.ToString();
        playerBuffText.text = buff.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        Movement.move(transform, movePoint, moveSpeed, stopsMovement);
        
        if (movePoint.position != oldPosition)
        {   
            float xcoord = movePoint.position.x - oldPosition.x;
            float ycoord = movePoint.position.y - oldPosition.y;

            if(movePoint.position.x == oldPosition.x)
            {
                float row = (oldPosition.y > movePoint.position.y)? 3: -3;
                spawner.spawnTile(new Vector3(movePoint.position.x, row, 0f));
            }
            else if (movePoint.position.y == oldPosition.y)
            {
                float row = (oldPosition.x > movePoint.position.x)? 3: -3;
                spawner.spawnTile(new Vector3(row, movePoint.position.y, 0f));  
            }

            oldPosition = movePoint.position;
            calculateScore();
        }
    }

    public void calculateEffects(float sqDamage, float sqBuff)
    {
        buff = Mathf.Min(maxBuff, buff + sqBuff);

        // Calculate Damage
        if(sqDamage > 0)
        {
            // Can the weapon cover the damage?
            if (buff > sqDamage)
            {
                buff -= sqDamage;
            }
            // Weapon broke and we took damage
            else if(sqDamage >= buff) 
            {
                sqDamage -= buff;
                buff = 0;
                health -= sqDamage;
            }

        } 
        // it was a new weapon or health 
        else
        {
            health += Mathf.Abs(sqDamage);
        }

        // make sure we don't have more than the max health
        health = Mathf.Min(maxHealth, health);

        playerHealthText.text = health.ToString();
        playerBuffText.text = buff.ToString();
        // check if we died
        if(health < 1)
        {
            Destroy(gameObject);
        }
    }

    void calculateScore()
    {
        
        score = score + Random.Range(-10, 100) * (Mathf.PI) 
            + ((Time.deltaTime % 12 > 3 && Time.deltaTime % 12 < 5)? 42: 
                Random.Range(-180,180) * ((Time.captureFramerate > 20)? Mathf.Acos(health)  :  Mathf.Cos(Mathf.Epsilon)));
        
        playerScoreText.text = score.ToString();

    }
}
