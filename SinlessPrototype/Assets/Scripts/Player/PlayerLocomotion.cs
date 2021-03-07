using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    Animator animator;
    float velocityX = 0.0f;
    float velocityZ = 0.0f;
    public float acceleration = 0.1f;
    public float deceleration = 0.1f;
    public float maxWalkVelocity = 0.5f;
    public float maxRunVelocity = 2f;
    int VelocityXHash;
    int VelocityZHash;



    private void Start()
    {
        animator = GetComponent<Animator>();

        VelocityXHash = Animator.StringToHash("Velocity X");
        VelocityZHash = Animator.StringToHash("Velocity Z");
    }
    void changevelocity(bool forwardPressed, bool leftPressed, bool rightPressed, bool runPressed, float currentMaxVelocity)
    {
        if (forwardPressed && velocityZ < currentMaxVelocity && !runPressed)
        {
            velocityZ += Time.deltaTime * acceleration;
        }

        if (leftPressed && velocityX > -currentMaxVelocity && !runPressed)
        {
            velocityX -= Time.deltaTime * deceleration;
        }

        //left strafe
        if (leftPressed && runPressed && velocityX > -currentMaxVelocity)
        {
            velocityX = velocityX -= Time.deltaTime * deceleration;
        }
        else if (leftPressed && velocityX < -currentMaxVelocity)
        {
            velocityX += Time.deltaTime * deceleration;
            if (velocityX < -currentMaxVelocity && velocityX > (-currentMaxVelocity - 0.05f))
            {
                velocityX = -currentMaxVelocity;
            }
        }
        else if (leftPressed && velocityX > -currentMaxVelocity && velocityX < (-currentMaxVelocity + 0.05f))
        {
            velocityX = -currentMaxVelocity;
        }

        if (rightPressed && velocityX < currentMaxVelocity && !runPressed)
        {
            velocityX += Time.deltaTime * acceleration;
        }

        //right strafe
        if (rightPressed && runPressed && velocityX < currentMaxVelocity)
        {
            velocityX = velocityX += Time.deltaTime * acceleration;
        }
        else if (rightPressed && velocityX > currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * deceleration;
            if (velocityX < currentMaxVelocity && velocityX > (currentMaxVelocity + 0.05f))
            {
                velocityX = currentMaxVelocity;
            }
        }
        else if (rightPressed && velocityX < currentMaxVelocity && velocityX > (currentMaxVelocity - 0.05f))
        {
            velocityX = currentMaxVelocity;
        }

        if (!forwardPressed && velocityZ > 0.0f)
        {
            velocityZ -= Time.deltaTime * deceleration;
        }

        if (!leftPressed && velocityX < 0.0f)
        {
            velocityX += Time.deltaTime * acceleration;
        }

        if (!rightPressed && velocityX > 0.0f)
        {
            velocityX -= Time.deltaTime * deceleration;
        }

        if (!rightPressed && !leftPressed && velocityX != 0.0f && (velocityX > -0.05f && velocityX < 0.05f))
        {
            velocityX = 0.0f;
        }

        if (!forwardPressed && velocityZ < 0.0f)
        {
            velocityZ = 0.0f;
        }

        if (forwardPressed && runPressed && velocityZ < currentMaxVelocity)
        {
            velocityZ = velocityZ += Time.deltaTime * acceleration;
        }
        else if (forwardPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ -= Time.deltaTime * deceleration;
            if (forwardPressed && velocityZ < currentMaxVelocity && velocityZ > (currentMaxVelocity + 0.05f))
            {
                velocityZ = currentMaxVelocity;
            }
        }
        else if (forwardPressed && velocityZ < currentMaxVelocity && velocityZ > (currentMaxVelocity - 0.05f))
        {
            velocityZ = currentMaxVelocity;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //gets input being pressed by player
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool leftPressed = Input.GetKey(KeyCode.A);
        bool rightPressed = Input.GetKey(KeyCode.D);
        bool backPressed = Input.GetKey(KeyCode.S);
        bool runPressed = Input.GetKey(KeyCode.LeftControl);
        bool jumpPressed = Input.GetKey(KeyCode.Space);

        if (jumpPressed)
        {
            animator.Play("Jump");
        }
        
        //set current max velocity
        float currentMaxVelocity = runPressed ? maxRunVelocity : maxWalkVelocity;

        if (!jumpPressed)
        {

            //handle the velocity chnges based on buttons pressed
            changevelocity(forwardPressed, leftPressed, rightPressed, runPressed, currentMaxVelocity);


            //set parameters to local variable values
            animator.SetFloat(VelocityZHash, velocityZ);
            animator.SetFloat(VelocityXHash, velocityX);
        }
    }
}