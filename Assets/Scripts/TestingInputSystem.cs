using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestingInputSystem : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;

    private PlayerInput playerInput;
    PlayerInputActions playerInputActions;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Jump.performed += Jump;
        //playerInputActions.Player.Movement.performed += Movement_performed;
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
        //Debug.Log(inputVector);
        rb.velocity = inputVector;
        if(Gamepad.current.bButton.wasPressedThisFrame)
        {
            playerInput.SwitchCurrentActionMap("UI");
            playerInputActions.Player.Disable();
            playerInputActions.UI.Enable();
            Debug.Log("Switched Action Map to: " + playerInput.currentActionMap);
        }
        if(Gamepad.current.xButton.wasPressedThisFrame)
        {
            playerInput.SwitchCurrentActionMap("Player");
            playerInputActions.Player.Enable();
            playerInputActions.UI.Disable();
            Debug.Log("Switched Action Map to: " + playerInput.currentActionMap);
        }

        if(Gamepad.current.yButton.wasPressedThisFrame)
        {
            playerInputActions.Player.Disable();
            playerInputActions.Player.Jump.PerformInteractiveRebinding()
                .OnComplete(callback =>
                {
                    Debug.Log(callback.action.bindings[0].overridePath);
                    callback.Dispose();
                    playerInputActions.Player.Enable();
                })
                .Start();
        }
    }

    private void Movement_performed(InputAction.CallbackContext context)
    {
        Debug.Log(context);
        Vector2 inputVector = context.ReadValue<Vector2>();
        rb.velocity = inputVector;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        Debug.Log(context.phase);
        if (context.performed)
        {
            Debug.Log("Jump " + context.phase);
            //rb.velocity = new Vector2(0.0f, jumpForce);
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    public void Submit(InputAction.CallbackContext context)
    {
        Debug.Log("Submit" + context.phase);
    }
}
