using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    //public Animator playerAnim;
    public float speed = 200;
    public float rotationSpeed = 300;
    private float horizontal;
    private float vertical;
    private Vector3 inputDirection;
    private GameController gameController;

    public void Start()
    {
        gameController = GameController.Instance;
    }

    private void FixedUpdate()
    {
        if (gameController.currentState == GameState.Playing) {
            Movement();
        }
    }

    #region Movement
    private void Movement()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        inputDirection = new Vector3(horizontal, 0, vertical).normalized;
        rb.velocity = inputDirection * speed * Time.fixedDeltaTime;

        if (inputDirection.magnitude != 0) {
            Quaternion toRotation = Quaternion.LookRotation(inputDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.fixedDeltaTime);
        }


        if (this.rb.velocity.magnitude != 0) {
           // set movement animation
        } 

    }
    #endregion

    #region Collision

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "FinalFlag") {
            GameController.Instance.CompleteMaze();
        }
    }

    #endregion

}
