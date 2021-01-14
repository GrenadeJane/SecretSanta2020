using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;                //Floating point variable to store the player's movement speed.

    private Rigidbody2D rb2d;
    private PlayerInteraction playerInteractionComponent;
    SoundManager SoundManagerComponent;

    private Animator animatorComponent;
    private void Awake()
    {
        playerInteractionComponent = GetComponent<PlayerInteraction>();
        animatorComponent = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        SoundManagerComponent = GetComponent<SoundManager>();
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        if (Game.Singleton.gameState != Game.GameState.GS_GAME) return;

        if (rb2d.velocity.sqrMagnitude > 0.2f)
            SoundManagerComponent.PlayWalkSound();
        else
            SoundManagerComponent.StopSound();

        if (playerInteractionComponent.playerState != PlayerState.PS_NONE &&
            playerInteractionComponent.playerState != PlayerState.PS_CarryBone)
            return;
        //Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = Input.GetAxis("Horizontal");
        //Store the current vertical input in the float moveVertical.
        float moveVertical = Input.GetAxis("Vertical");

        //Use the two store floats to create a new Vector2 variable movement.
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        //Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
        rb2d.AddForce(movement * speed, ForceMode2D.Force );

        animatorComponent.SetFloat("Speed", rb2d.velocity.sqrMagnitude);

       

    }
}
