//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.InputSystem;

//public class InputHandler : MonoBehaviour
//{
//    PlayerInput playerInput;
//    PlayerActions playerActions;
//    public bool isSprintPressed, isJumpPressed, isGrounded, isMovePressed, isAiming, isShooting;
//    public Vector3 moveVector;

//    private void Awake()
//    {
//        playerInput = new PlayerInput();
//        playerActions = GetComponent<PlayerActions>();
        

//        playerInput.Player.Movement.started += HandleMovementInput;
//        playerInput.Player.Movement.performed += HandleMovementInput;
//        playerInput.Player.Movement.canceled += HandleMovementInput;
//        playerInput.Player.Sprint.started += HandleSprintInput;
//        playerInput.Player.Sprint.canceled += HandleSprintInput;
//        playerInput.Player.Jump.started += HandleJumpInput;
//        playerInput.Player.Jump.canceled += HandleJumpInput;
//        playerInput.Player.Aim.started += HandleAim;
//        playerInput.Player.Aim.canceled += HandleAim;
//        playerInput.Player.Shoot.started += HandleShoot;
//        playerInput.Player.Shoot.canceled += HandleShoot;
//    }

//    private void Update()
//    {
//        isGrounded = playerActions.isGrounded;
//    }

//    private void HandleSprintInput(InputAction.CallbackContext context)
//    {
//        isSprintPressed = context.ReadValueAsButton();
//    }
//    private void HandleMovementInput(InputAction.CallbackContext context)
//    {

//        moveVector.x = context.ReadValue<Vector2>().x;
//        moveVector.z = context.ReadValue<Vector2>().y;
//        isMovePressed = moveVector.x != 0 || moveVector.z != 0;

//    }
//    private void HandleJumpInput(InputAction.CallbackContext context)
//    {
//        isJumpPressed = context.ReadValueAsButton();
//    }
//    private void HandleAim(InputAction.CallbackContext context)
//    {
//        isAiming = context.ReadValueAsButton();
//    }
//    private void HandleShoot(InputAction.CallbackContext context)
//    {
//        isShooting = context.ReadValueAsButton();
//    }

//    private void OnEnable()
//    {
//        playerInput.Player.Enable();
//    }

//    private void OnDisable()
//    {
//        playerInput.Player.Disable();
//    }
//}
