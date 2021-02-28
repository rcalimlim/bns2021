using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Player Max Stats
    public float maxHealth = 10f;
    public float maxBuff = 10f;

    // UI Components
    public Text playerHealthText;
    public Text playerBuffText;

    public Text[] playerScoreTexts;
    public Text[] playerTurnTexts;

    public GameObject gameOver;

    public AudioClip swordAudio;

    // Player Current Stats
    float score = 0f;
    int turns = 0;
    float health;
    float buff;

    Transform movePoint;
    Vector3 oldPosition;
    TileSpawner spawner;    


    // Start is called before the first frame update
    void Start()
    {
        // How we are performing movement because we be cheating
        movePoint = GameObject.Find("Bryce Move Point").transform;
        oldPosition = movePoint.position;

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
        // Spawn a new tile
        if (movePoint.position != oldPosition )
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

    public bool Attack(EnemyController tileController)
    {   
        float sqDamage = tileController.damage;
        
        if(sqDamage <= 0) {
            return false;
        }
        else if(buff >= sqDamage)
        {
            return false;
        }
        else if (buff > 0)
        {
            sqDamage -= buff;
            buff = 0f;
            playerBuffText.text = buff.ToString();
            tileController.UpdateDamage(sqDamage);
            return true;
        }
        return false;
    }
    
    public AudioClip CalculateEffects(float sqDamage, float sqBuff)
    {
        buff = Mathf.Min(maxBuff, buff + sqBuff);
        bool swordSound = false;

        // Calculate Damage
        if(sqDamage > 0)
        {
            swordSound = (buff > 0);

            // Can the weapon cover the damage?
            if (buff > sqDamage)
            {
                buff -= sqDamage;
            }
            // Weapon broke and we took damage
            //  weapon breaking handled in Attack
            else if(sqDamage >= buff) 
            {
                sqDamage -= buff;
                buff = 0;
                health -= sqDamage;
            }
        } 
        else
        {
            // it was a new weapon or health
            health += Mathf.Abs(sqDamage);
        }

        // make sure we don't have more than the max health
        health = Mathf.Min(maxHealth, health);

        playerHealthText.text = health.ToString();
        playerBuffText.text = buff.ToString();
        // check if we died
        if(health < 1)
        {
            gameOver.SetActive(true);
            Destroy(gameObject);
        }

        if(swordSound)
            return swordAudio;
            
        return null;
    }

    void calculateScore()
    {
        turns += 1;
        score += Random.Range(-36, 100);
        foreach(Text text in playerScoreTexts)
            text.text = score.ToString();
        
        foreach(Text text in playerTurnTexts)
            text.text = turns.ToString();
    }
    
}
