using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Singleton<PlayerMovement>
{
    public CharacterController controller;
    public float speed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;
    public float health = 100;

    void Update()
    {
        //Checks if we are touching the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        //Get the input from the player
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //Move the player
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        //Does the jump stuff
        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }

    public void Hit(int damage)
    {
        health -= damage;
        print("Player health: " + health);
        if (health <= 0)
            _GM.gameState = GameState.GameOver;
    }
}
