using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerActions : MonoBehaviour
{
    PlayerInput playerInput;
    [SerializeField] private float moveSpeed;
    CharacterController characterController;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float rotationSpeed = .8f;
    public Vector3 moveVector;
    [SerializeField]
    public bool isSprintPressed, isJumpPressed, isGrounded, isSprinting, isRunning, isMovePressed,
        isAiming, isShooting, isGrenadeGettingThrown, isReloading, reloadStarted;
    private const float groundDistance = 1f;
    [SerializeField] float jumpPower = 5f;
    private Transform _cameraTransform;
    private Animator _animator;
    [SerializeField] private float gravityConstant = 1f;
    [SerializeField] CinemachineVirtualCamera tpsCamera;
    [SerializeField] CinemachineVirtualCamera aimCamera;
    private Weapon weapon;
   
    


    private void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        _cameraTransform = Camera.main.transform;
        _animator = GetComponentInChildren<Animator>();
        weapon = GameObject.Find("Weapon").GetComponent<Weapon>();




        playerInput.Player.Movement.started += HandleMovementInput;
        playerInput.Player.Movement.performed += HandleMovementInput;
        playerInput.Player.Movement.canceled += HandleMovementInput;
        playerInput.Player.Sprint.started += HandleSprintInput;
        playerInput.Player.Sprint.canceled += HandleSprintInput;
        playerInput.Player.Jump.started += HandleJumpInput;
        playerInput.Player.Jump.canceled += HandleJumpInput;
        playerInput.Player.Aim.started += HandleAim;
        playerInput.Player.Aim.canceled += HandleAim;
        playerInput.Player.Shoot.started += HandleShoot;
        playerInput.Player.Shoot.canceled += HandleShoot;
        playerInput.Player.Reload.started += HandleReload;
        playerInput.Player.Reload.canceled += HandleReload;
        playerInput.Player.ThrowGrenade.started += HandleGrenade;
        playerInput.Player.ThrowGrenade.canceled += HandleGrenade;
    }

    private void Update()
    {
        RotatePlayer();
        Move();
        GroundControl();
        Jump();
        ApplyGravity();
        HandleAnimations();
        ReloadWeapon();
        Debug.Log(weapon.ammoInPockets + " " + weapon.damage + " " + weapon.currentAmmo + " " + isReloading);
    }

    private void HandleReload(InputAction.CallbackContext context)
    {
        isReloading = context.ReadValueAsButton();
    }

    private void HandleSprintInput(InputAction.CallbackContext context)
    {
        isSprintPressed = context.ReadValueAsButton();
    }

    private void HandleGrenade(InputAction.CallbackContext context)
    {
        isGrenadeGettingThrown = context.ReadValueAsButton();
    }

    private void HandleMovementInput(InputAction.CallbackContext context)
    {

        moveVector.x = context.ReadValue<Vector2>().x;
        moveVector.z = context.ReadValue<Vector2>().y;
        isMovePressed = moveVector.x != 0 || moveVector.z != 0;

    }

    private void HandleJumpInput(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }
    private void HandleAim(InputAction.CallbackContext context)
    {
        isAiming = context.ReadValueAsButton();
    }

    private void HandleShoot(InputAction.CallbackContext context)
    {
        if (context.started && weapon.currentAmmo > 0 && !isReloading)
        {
            isShooting = true;
            weapon.Shoot();
            weapon.currentAmmo -= 1;
        }
        else if (context.canceled)
        {
            isShooting = false;
        }
    }

    private void Move()
    {
        Vector3 forward = _cameraTransform.forward;
        Vector3 right = _cameraTransform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = (forward * moveVector.z + right * moveVector.x).normalized;
        Vector3 moveDirection = desiredMoveDirection * (isSprintPressed ? sprintSpeed : moveSpeed);

        characterController.Move(moveDirection * Time.deltaTime);

        isSprinting = isSprintPressed;
        isRunning = isMovePressed;
    }
    private void GroundControl()
    {
        if (Physics.Raycast(transform.position, Vector3.down, groundDistance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
    private void RotatePlayer()
    {
        Quaternion rotation = Quaternion.Euler(0, _cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }


    private void HandleAnimations()
    {
        if (isSprinting)
            _animator.SetFloat("AControl", 1.0f);
        else if (isRunning)
            _animator.SetFloat("AControl", 0.5f);
        else
            _animator.SetFloat("AControl", 0.0f);

        if (isAiming)
            _animator.SetBool("isAiming", true);
        else
            _animator.SetBool("isAiming", false);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            _animator.SetBool("isJumping", false);
            if (isJumpPressed)
            {
                moveVector.y = jumpPower;
                _animator.SetBool("isJumping", true);
            }
        }
    }

    private void ApplyGravity()
    {
        if (!isGrounded)
        {
            float gravityScale = moveVector.y < 0 ? 2.0f : 1.0f;
            moveVector.y -= gravityConstant * gravityScale * Time.deltaTime;
        }
        else if (!isJumpPressed)
        {
            moveVector.y = -0.1f;
        }

        characterController.Move(new Vector3(0, moveVector.y, 0) * Time.deltaTime);
    }

    private void ReloadWeapon()
    {
        if (isReloading)
        {
            reloadStarted = true;
        }

        if (reloadStarted)
        {
            weapon.StartCoroutine("Reload");
        }
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }
}
