using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public bool playerInput = true;

    public bool canRoll = true;

    public Vector2 RawMovementInput { get; private set; }

    private int NormInputXbuffer;
    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }
    public float RawInputY { get; private set; }

    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }

    public bool AttackInput { get; private set; }
    public bool DashInput { get; private set; }
    public bool DashInputHold { get; private set; }
    public bool DashInputHoldBuffer { get; private set; }

    public bool SkillInput { get; private set; }

    public bool SkillSelectInput { get; private set; }

    public bool MapInput { get; private set; }

    public bool HealInput { get; private set; }

    private bool isDashInputActivated;

    [SerializeField]
    private float jumpInputBufferTime = 0.15f;
    private float jumpInputBufferTimeCounter;

    [SerializeField]
    private float attackInputBufferTime = 0.15f;
    private float attackInputBufferTimeCounter;

    [SerializeField]
    private float dashInputBufferTime = 0.15f;
    private float dashInputBufferTimeCounter;

    [SerializeField]
    private float skillInputBufferTime = 0.15f;
    private float skillInputBufferTimeCounter;

    [SerializeField]
    private float healInputBufferTime = 0.15f;
    private float healInputBufferTimeCounter;

    private void Update()
    {
        CheckJumpInputBufferTime();
        CheckAttackInputBufferTime();
        CheckDashInputBufferTime();
        CheckSkillInputBufferTime();
        CheckHealInputBufferTime();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();
        NormInputXbuffer = (int)(RawMovementInput * Vector2.right).normalized.x;
        if (playerInput)
        {
            //normalize the input
            NormInputX = NormInputXbuffer;

            if ((RawMovementInput * Vector2.up).y > 0.7) NormInputY = 1;
            else if ((RawMovementInput * Vector2.up).y < -0.7) NormInputY = -1;
            else NormInputY = 0;

            RawInputY = (RawMovementInput * Vector2.up).y;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (playerInput)
        {
            if (context.started)
            {
                JumpInput = true;
                JumpInputStop = false;
                jumpInputBufferTimeCounter = jumpInputBufferTime;
            }
            if (context.canceled)
            {
                JumpInputStop = true;
            }
        }
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (playerInput)
        {
            if (context.started)
            {
                AttackInput = true;
                attackInputBufferTimeCounter = attackInputBufferTime;
            }
        }
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DashInput = true;
            DashInputHoldBuffer = true;
            dashInputBufferTimeCounter = dashInputBufferTime;
        }
        if (context.canceled)
        {
            DashInputHoldBuffer = false;
        }

        if (playerInput)
        {
            DashInputHold = DashInputHoldBuffer;
        }
    }

    public void OnSkillInput(InputAction.CallbackContext context)
    {
        if (playerInput)
        {
            if (context.started)
            {
                SkillInput = true;
                skillInputBufferTimeCounter = skillInputBufferTime;
            }
        }
    }

    public void OnHealInput(InputAction.CallbackContext context)
    {
        if (playerInput)
        {
            if (context.started)
            {
                HealInput = true;
                healInputBufferTimeCounter = healInputBufferTime;
            }
        }
    }

    public void OnSkillSelectInput(InputAction.CallbackContext context)
    {
        if (playerInput)
        {
            if (context.started)
            {
                SkillSelectInput = true;
            }

            if (context.canceled)
            {
                SkillSelectInput = false;
            }
        }

    }

    public void OnMapInput(InputAction.CallbackContext context)
    {
        if (playerInput)
        {
            if (context.started)
            {
                MapInput = true;
            }

            if (context.canceled)
            {
                MapInput = false;
            }
        }
    }

    public void StartCustomInput(int xInput, bool jumpInput, bool dashInputHold)
    {
        //Stop Player Input
        playerInput = false;
        NormInputX = xInput;
        JumpInput = jumpInput;
        DashInputHold = dashInputHold;

        canRoll = false;
    }

    public void CanPlayerInput(float delayTime)
    {
        Invoke(nameof(SetCanPlayerInput), delayTime);
    }

    private void SetCanPlayerInput()
    {
        playerInput = true;
        NormInputX = NormInputXbuffer;
        JumpInput = false;
        DashInputHold = DashInputHoldBuffer;
        canRoll = true;
    }



    public void UseJumpInput() => JumpInput = false;

    public void UseAttackInput() => AttackInput = false;

    public void UseDashInput() => DashInput = false;

    public void UseSkillInput() => SkillInput = false;

    public void UseHealInput() => HealInput = false;

    private void CheckJumpInputBufferTime()
    {
        jumpInputBufferTimeCounter -= Time.deltaTime;
        if (jumpInputBufferTimeCounter < 0)
        {
            JumpInput = false;
        }
    }

    private void CheckAttackInputBufferTime()
    {
        attackInputBufferTimeCounter -= Time.deltaTime;
        if (attackInputBufferTimeCounter < 0)
        {
            AttackInput = false;
        }
    }

    private void CheckDashInputBufferTime()
    {
        dashInputBufferTimeCounter -= Time.deltaTime;
        if (dashInputBufferTimeCounter < 0)
        {
            DashInput = false;
        }
    }

    private void CheckSkillInputBufferTime()
    {
        skillInputBufferTimeCounter -= Time.deltaTime;
        if (skillInputBufferTimeCounter < 0)
        {
            SkillInput = false;
        }
    }

    private void CheckHealInputBufferTime()
    {
        healInputBufferTimeCounter -= Time.deltaTime;
        if (healInputBufferTimeCounter < 0)
        {
            HealInput = false;
        }
    }
}
