using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float maxHealth = 10f;
    public float maxBuff = 10f;

    float health;
    float buff;

    Transform movePoint;
    LayerMask stopsMovement;

    // Start is called before the first frame update
    void Start()
    {
        movePoint = transform.GetChild(0);
        stopsMovement = LayerMask.GetMask("StopMovement");
        //print(movePoint.gameObject.name);
        movePoint.SetParent(null);
        health = maxHealth;
        buff = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        Movement.move(transform, movePoint, moveSpeed, stopsMovement);
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

        print("Health: " + health + " Weapon: " + buff);
        
        // check if we died
        if(health < 1)
        {
            Destroy(gameObject);
        }
    }
}
