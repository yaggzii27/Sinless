using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public string username;
    public CharacterController controller;
    public float gravity = -9.81f;
    public float moveSpeed = 2.5f;
    public float jumpSpeed = 5f;
    public float rotationSpeed = 75.0f;
    private PlayerLocomotion pl;

    private bool[] inputs;
    private float yVelocity = 0;

    private void Start()
    {
        pl = GetComponentInChildren<PlayerLocomotion>();
        gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
        moveSpeed *= Time.fixedDeltaTime;
        jumpSpeed *= Time.fixedDeltaTime;
    }

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;

        inputs = new bool[6];
    }

    /// <summary>Processes player input and moves the player.</summary>
    public void FixedUpdate()
    {
        pl.clearAnimations();

        Vector2 _inputDirection = Vector2.zero;
        if (inputs[0])
        {
            pl.forwardPressed = true;
            _inputDirection.y += 1;
        }
        if (inputs[1])
        {
            pl.backPressed = true;
            _inputDirection.y -= 1;
        }
        if (inputs[2])
        {
            pl.leftPressed = true;
            _inputDirection.x -= 1;
        }
        if (inputs[3])
        {
            pl.rightPressed = true;
            _inputDirection.x += 1;
        }
        if (inputs[5] || inputs[6])
        {
            moveSpeed = .5f;
            pl.runPressed = true;
        }
        else
        {
            moveSpeed = .1f;
        }

        Move(_inputDirection);
    }

    /// <summary>Calculates the player's desired movement direction and moves him.</summary>
    /// <param name="_inputDirection"></param>
    private void Move(Vector2 _inputDirection)
    {
        Vector3 _moveDirection = transform.right * _inputDirection.x + transform.forward * _inputDirection.y;
        _moveDirection *= moveSpeed;

      //  transform.rotation = Quaternion.LookRotation(_moveDirection);

        if (controller.isGrounded)
        {
            yVelocity = 0f;
            if (inputs[4])
            {
                yVelocity = jumpSpeed;
            }
        }

        yVelocity += gravity;

        _moveDirection.y = yVelocity;
        controller.Move(_moveDirection);

        ServerSend.PlayerPosition(this);
        ServerSend.PlayerRotation(this);
    }

    /// <summary>Updates the player input with newly received input.</summary>
    /// <param name="_inputs">The new key inputs.</param>
    /// <param name="_rotation">The new rotation.</param>
    public void SetInput(bool[] _inputs, Quaternion _rotation)
    {
        inputs = _inputs;
        transform.rotation = _rotation;
    }

}