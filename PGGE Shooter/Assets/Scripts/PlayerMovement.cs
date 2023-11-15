using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector]
    public CharacterController mCharacterController;
    public Animator mAnimator;

    public float mWalkSpeed = 1.5f;
    public float mRotationSpeed = 50.0f;
    public bool mFollowCameraForward = false;
    public float mTurnRate = 10.0f;

    private float hInput;
    private float vInput;
    private float speed;

#if UNITY_ANDROID
    public FixedJoystick mJoystick;
#endif


    void Start()
    {
        mCharacterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleInputs();
        Move();
    }
    private void HandleInputs()
    {
        // We shall handle our inputs here.
#if UNITY_STANDALONE
        hInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");
#endif

#if UNITY_ANDROID
        hInput = 2.0f * mJoystick.Horizontal;
        vInput = 2.0f * mJoystick.Vertical;
#endif


        speed = mWalkSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = mWalkSpeed * 2.0f;
        }
    }

    private void Move()
    {
        // We shall apply movement to the game object here.  
        if (mAnimator == null) return;
        if (mFollowCameraForward)
        {
            // rotate Player towards the camera forward.
            Vector3 eu = Camera.main.transform.rotation.eulerAngles;
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.Euler(0.0f, eu.y, 0.0f),
                mTurnRate * Time.deltaTime);
        }
        else
        {
            transform.Rotate(0.0f, hInput * mRotationSpeed * Time.deltaTime, 0.0f);
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward).normalized;
        forward.y = 0.0f;

        mCharacterController.Move(forward * vInput * speed * Time.deltaTime);
        mAnimator.SetFloat("PosX", 0);
        mAnimator.SetFloat("PosZ", vInput * speed / (2.0f * mWalkSpeed));
    }

}
