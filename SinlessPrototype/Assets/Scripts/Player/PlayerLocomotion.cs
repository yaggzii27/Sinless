using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isSprintingHash;

    private void Start()
    {
        animator = GetComponent<Animator>();
        isSprintingHash = Animator.StringToHash("isSprinting");
        isWalkingHash = Animator.StringToHash("isWalking");
    }

    // Update is called once per frame
    void Update()
    {
        bool isSprinting = animator.GetBool(isSprintingHash);

        bool forwardPressed = Input.GetKey("w");

        if (forwardPressed && !isSprinting)
        {
            animator.SetBool("isSprinting", true);
        }

        if (!forwardPressed && isSprinting)
        {
            animator.SetBool("isSprinting", false);
        }
    }
}
