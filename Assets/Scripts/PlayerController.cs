using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D playerRigidBody;
    SpriteRenderer playerSpriteRenderer;
    Animator playerAnimator;


    // Public Settings to fiddle with in Unity
    public float maxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        // Get the things we applied to the player
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        // Crappy Code incomming!!

        // Gets the input from da user. Can do specific keys but the axis gets
        // keys and alt keys
        float vert = Input.GetAxis("Vertical");
        float horz = Input.GetAxis("Horizontal");

        if (vert != 0) 
        {
            // Need triggers to tell the animator to change the animation
            playerAnimator.SetInteger("MoveY",vert < 0 ? -1 : vert > 0? 1 : 0);
            playerAnimator.SetInteger("MoveX", 0);

            // Set the player Speed
            playerRigidBody.velocity = new Vector2(0f, vert*maxSpeed);
        }
        else if(horz != 0)
        {
            playerAnimator.SetInteger("MoveX",horz < 0 ? -1 : horz > 0? 1 : 0);
            playerAnimator.SetInteger("MoveY", 0);
            playerRigidBody.velocity = new Vector2(horz * maxSpeed, 0f);
        }
        else {
            playerAnimator.SetInteger("MoveX", 0);
            playerAnimator.SetInteger("MoveY", 0);
        }
       


    }
}
