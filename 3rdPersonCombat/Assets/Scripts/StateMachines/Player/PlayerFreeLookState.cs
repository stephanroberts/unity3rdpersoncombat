using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{

    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");

    private const float AnimatorDampTime = 0.1f;
    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine){}

    public override void Enter()
    {

    }
    public override void Tick(float deltaTime)
    {
        RotateCamera(deltaTime);

        Vector3 movement = CalculateMovement();
        stateMachine.Controller.Move(movement * stateMachine.FreeLookMovementSpeed * deltaTime);

        if (stateMachine.InputReader.MovementValue == Vector2.zero) {
            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
            return;
        }

        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 1, AnimatorDampTime, deltaTime);
        FaceMovementDirection(movement, deltaTime);
    }

    public override void Exit()
    {

    }


    private Vector3 CalculateMovement() {
        Vector3 cameraForward = stateMachine.MainCameraTransform.forward;
        Vector3 cameraRight = stateMachine.MainCameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        return cameraForward * stateMachine.InputReader.MovementValue.y + cameraRight * stateMachine.InputReader.MovementValue.x;
    }
    private void FaceMovementDirection(Vector3 movement, float deltaTime) {
        stateMachine.CharacterModel.transform.rotation = Quaternion.Lerp(
                stateMachine.CharacterModel.transform.rotation,
                Quaternion.LookRotation(movement),
                deltaTime * stateMachine.RotationSmoothValue
            );
    }

    private void RotateCamera(float deltaTime) {
        float horizontalRotation = stateMachine.InputReader.LookValue.x * deltaTime * 100;
        float verticalRotation = stateMachine.InputReader.LookValue.y * deltaTime * 100 * -1;
        // stateMachine.CameraHorizontal.transform.rotation = Quaternion.Euler(0, 90, 0);
        stateMachine.CameraHorizontal.transform.Rotate(new Vector3(0 ,horizontalRotation, 0));
        stateMachine.CameraVertical.transform.Rotate(new Vector3(verticalRotation, 0, 0));
    }

}
