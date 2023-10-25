using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TPCBase
{
    protected Transform mCameraTransform;
    protected Transform mPlayerTransform;
    
    public Transform CameraTransform
    {
        get
        {
            return mCameraTransform;
        }
    }
    public Transform PlayerTransform
    {
        get
        {
            return mPlayerTransform;
        }
    }

    public TPCBase(Transform cameraTransform, Transform playerTransform)
    {
        mCameraTransform = cameraTransform;
        mPlayerTransform = playerTransform;
    }

    public abstract void Update();

}

public class TPCTrack : TPCBase
{
    public TPCTrack(Transform cameraTransform, Transform playerTransform)
    : base(cameraTransform, playerTransform)
    {
    }
    public override void Update()
    {
        Vector3 targetPos = mPlayerTransform.position;
        //float playerHeight = 2.0f;
        //targetPos.y += playerHeight;
        // We add the camera offset on the Y-axis
        targetPos.y += GameConstants.CameraPositionOffset.y;
        mCameraTransform.LookAt(targetPos);
    }
}

public static class GameConstants
{
    public static Vector3 CameraAngleOffset { get; set; }
    public static Vector3 CameraPositionOffset { get; set; }
    public static float Damping { get; set; }
}

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform mPlayer;
    TPCBase mThirdPersonCamera;
    public Vector3 mPositionOffset = new Vector3(0.0f, 2.0f, -2.5f);
    public Vector3 mAngleOffset = new Vector3(0.0f, 0.0f, 0.0f);
    [Tooltip("The damping factor to smooth the changes in position and rotation of the camera.")]
    public float mDamping = 1.0f;
    void Start()
    {
        //Set GameConstants class so other objects can use
        GameConstants.CameraPositionOffset = mPositionOffset;
        //mThirdPersonCamera = new TPCTrack(transform, mPlayer);
        //mThirdPersonCamera = new TPCFollowTrackPosition(transform, mPlayer);
        mThirdPersonCamera = new TPCFollowTrackPositionAndRotation(transform,
        mPlayer);
        // Set the game constant parameters to the GameConstants class.
        GameConstants.Damping = mDamping;
        GameConstants.CameraPositionOffset = mPositionOffset;
        GameConstants.CameraAngleOffset = mAngleOffset;

    }
    void LateUpdate()
    {
        mThirdPersonCamera.Update();
    }
}

public abstract class TPCFollow : TPCBase
{
    public TPCFollow(Transform cameraTransform, Transform playerTransform)
    : base(cameraTransform, playerTransform)
    {
    }
    public override void Update()
    {
        // Calculate the camera transformed axes.
        // Calculate  new forward, up and right vectors for the camera
        Vector3 forward = mPlayerTransform.forward;
        Vector3 right = mPlayerTransform.right;
        Vector3 up = mPlayerTransform.up;
        // Calculate the offset in the camera's coordinate frame.
        // Calculate the targetPos
        Vector3 targetPos = mPlayerTransform.position;
        // Add the camera offset to the target position.
        Vector3 desiredPosition = targetPos + GameConstants.CameraPositionOffset;
        //Applying Lerp to chance position of camera.
        Vector3 position = Vector3.Lerp(mCameraTransform.position,
        desiredPosition, Time.deltaTime * GameConstants.Damping);
        mCameraTransform.position = position;
    }
}

public class TPCFollowTrackPosition : TPCFollow
{
    public TPCFollowTrackPosition(Transform cameraTransform, Transform
   playerTransform)
    : base(cameraTransform, playerTransform)
    {
    }
    public override void Update()
    {
        // Create the initial rotation quaternion based on the camera angle offset
        Quaternion initialRotation =
        Quaternion.Euler(GameConstants.CameraAngleOffset);
        // Rotate camera to the above initial rotation offset
        // Do it using damping/Lerp
        mCameraTransform.rotation =
        Quaternion.RotateTowards(mCameraTransform.rotation,
        initialRotation,
        Time.deltaTime * GameConstants.Damping);
        // Call the base class Update method to take care of the
        // position tracking
        base.Update();
    }
}

public class TPCFollowTrackPositionAndRotation : TPCFollow
{
    public TPCFollowTrackPositionAndRotation(Transform cameraTransform,
   Transform playerTransform)
    : base(cameraTransform, playerTransform)
    {
    }
    public override void Update()
    {
        // We apply the initial rotation to the camera.
        Quaternion initialRotation =
        Quaternion.Euler(GameConstants.CameraAngleOffset);
        // Allow rotation tracking of the player
        // so that our camera rotates when the Player rotates and at the same
        // time maintain the initial rotation offset.
        mCameraTransform.rotation = Quaternion.Lerp(
        mCameraTransform.rotation,
        mPlayerTransform.rotation * initialRotation,
        Time.deltaTime * GameConstants.Damping);
        base.Update();
    }
}


