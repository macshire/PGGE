using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //public CharacterController controller;
    //public float speed = 4f;
    //public float sprintSpeed = 10f;
    //public float mRotationSpeed = 150.0f;
    //Animator animator;

    //private void Start()
    //{
    //    animator = GetComponentInChildren<Animator>();
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    float horizontal = Input.GetAxisRaw("Horizontal");
    //    float vertical = Input.GetAxisRaw("Vertical");
    //    //ensure character speed does not increase when to keys are pressed
    //    Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

    //    animator.SetFloat("PosX", horizontal);
    //    animator.SetFloat("PosZ", vertical);

    //    transform.Rotate(0.0f, horizontal * mRotationSpeed * Time.deltaTime, 0.0f);

    //    if (direction.magnitude >= 0.1f)
    //    {

    //        if (Input.GetKey(KeyCode.LeftShift))
    //        {
    //            controller.Move(direction * sprintSpeed * Time.deltaTime);
    //        }
    //        else
    //        {
    //            controller.Move(direction * speed * Time.deltaTime);
    //        }
    //    }

    //}
    [HideInInspector]
    public CharacterController mCharacterController;
    public Animator mAnimator;

    public float mWalkSpeed = 1.0f;
    public float mRotationSpeed = 50.0f;

    void Start()
    {
        //getting controller from child component
        mCharacterController = GetComponentInChildren<CharacterController>();
    }

    void Update()
    {
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        float speed = mWalkSpeed;

        //player sprint when leftshift down
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = mWalkSpeed * 2.0f;
        }

        if (mAnimator == null) return;

        //rotating player based on horizontal input between each frame
        transform.Rotate(0.0f, hInput * mRotationSpeed * Time.deltaTime, 0.0f);

        //define forward direction of character independently
        Vector3 forward =
            transform.TransformDirection(Vector3.forward).normalized;
        forward.y = 0.0f;

        //player move based on foward input and speed 
        mCharacterController.Move(forward * vInput * speed * Time.deltaTime);

        //set float for parameters to transition between animations
        mAnimator.SetFloat("PosX", 0);
        mAnimator.SetFloat("PosZ", vInput * speed / 2.0f * mWalkSpeed);
    }

}
