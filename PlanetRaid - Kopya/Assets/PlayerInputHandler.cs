using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool sprint;
    public bool aim;
    public bool shoot;

    public bool buildMode;
    public bool analogMovement;

    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void OnLook(InputValue value)
    {
        if (cursorInputForLook)
        {
            LookInput(value.Get<Vector2>());
        }
    }

    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }

    public void OnSprint(InputValue value)
    {
        SprintInput(value.isPressed);
    }

    public void OnAim(InputValue value)
    {
        AimInput(value.isPressed);
    }

    public void OnChangeBuildMode(InputValue value)
    {
        BuildModeInput();
    }

    public void OnShoot(InputValue value)
    {
        ShootInput(value.isPressed);
    }



    public void BuildModeInput()
    {
        buildMode = !buildMode;

        if (buildMode)
        {
            cursorLocked = false;
            cursorInputForLook = false;
            Cursor.lockState = CursorLockMode.None;
            GameEvents.Instance.OnBuildMenuOpened?.Invoke(true);
        }
        else
        {

            cursorLocked = true;
            cursorInputForLook = true;
            Cursor.lockState = CursorLockMode.Locked;
            GameEvents.Instance.OnBuildMenuOpened?.Invoke(false);
        }

    }

    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }

    public void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }

    public void JumpInput(bool newJumpState)
    {
        jump = newJumpState;
    }

    public void SprintInput(bool newSprintState)
    {
        sprint = newSprintState;
    }

    public void AimInput(bool newAimState)
    {
        aim = newAimState;
    }

    public void ShootInput(bool newShootState)
    {
        shoot = newShootState;
    }


    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }


}
