using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerActions : MonoBehaviour
{
    PlayerInput playerInput;
    [SerializeField] private float _moveSpeed;
    CharacterController characterController;
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _rotationSpeed = .8f;
    public Vector3 moveVector;
    [SerializeField]
    public bool isSprintPressed, isJumpPressed, isGrounded, isSprinting, isRunning, isMovePressed,
        isAiming, isShooting, isGrenadeGettingThrown, isReloading, reloadStarted, isCrouching, bombWaiting;
    private const float _groundDistance = 1f;
    [SerializeField] private float _jumpPower = 5f;
    private Transform _cameraTransform;
    private Animator _animator;
    [SerializeField] private float _gravityConstant = 1f;
    [SerializeField] CinemachineVirtualCamera tpsCamera;
    [SerializeField] CinemachineVirtualCamera aimCamera;
    private Pistol _weapon;
    private Grenade grenade;
    [SerializeField] private GameObject _grenadePrefab;
    [SerializeField] private float _throwForce;
    private PlayerInventory _inventory;
    Rigidbody rb;



    private void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        _cameraTransform = Camera.main.transform;
        _animator = GetComponentInChildren<Animator>();
        _weapon = GameObject.FindWithTag("Weapon").GetComponent<Pistol>();
        _inventory = GameObject.FindWithTag("Player").GetComponent<PlayerInventory>();


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
        playerInput.Player.ThrowGrenade.started += HandleGrenade;
        playerInput.Player.ThrowGrenade.canceled += HandleGrenade;
        playerInput.Player.Crouch.started += HandleCrouch;
        playerInput.Player.Crouch.canceled += HandleCrouch;
    }


    private void Update()
    {
        RotatePlayer();
        Crouch();
        Move();
        GroundControl();
        Jump();
        ApplyGravity();
        HandleAnimations();
        ThrowGrenade();
    }

    private void HandleReload(InputAction.CallbackContext context)
    {
        //isReloading = context.ReadValueAsButton();
        if (context.phase == InputActionPhase.Started && !isReloading)
        {
            StartReload();
        }
    }
    private void HandleSprintInput(InputAction.CallbackContext context)
    {
        isSprintPressed = context.ReadValueAsButton();
    }
    private void HandleGrenade(InputAction.CallbackContext context)
    {
        if (context.started && _inventory.grenadeAmount > 0)
        {
            isGrenadeGettingThrown = true;
        }
        else if (context.canceled)
        {
            isGrenadeGettingThrown = false;
        }
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
        if (context.started && _weapon.currentAmmo > 0 && !isReloading)
        {
            isShooting = true;
            _weapon.StartRecoil();
            _weapon.Shoot();
        }
        else if (context.canceled)
        {
            isShooting = false;
            _weapon.StopRecoil();
        }
    }
    private void HandleCrouch(InputAction.CallbackContext context)
    {
        isCrouching = context.ReadValueAsButton();
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
        Vector3 moveDirection = desiredMoveDirection * (isSprintPressed ? _sprintSpeed : _moveSpeed);

        characterController.Move(moveDirection * Time.deltaTime);

        isSprinting = isSprintPressed;
        isRunning = isMovePressed;
    }
    private void GroundControl()
    {
        if (Physics.Raycast(transform.position, Vector3.down, _groundDistance))
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
        Quaternion rotation = Quaternion.Euler(_cameraTransform.eulerAngles.x, _cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, _rotationSpeed * Time.deltaTime);
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
                moveVector.y = _jumpPower;
                _animator.SetBool("isJumping", true);
            }
        }
    }
    private void ApplyGravity()
    {
        if (!isGrounded)
        {
            float gravityScale = moveVector.y < 0 ? 2.0f : 1.0f;
            moveVector.y -= _gravityConstant * gravityScale * Time.deltaTime;
        }
        else if (!isJumpPressed)
        {
            moveVector.y = -0.1f;
        }

        characterController.Move(new Vector3(0, moveVector.y, 0) * Time.deltaTime);
    }
    private void StartReload()
    {
        isReloading = true;
        Invoke("CompleteReload", 1.0f);
    }
    private void CompleteReload()
    {
        _weapon.Reload();
        isReloading = false;
    }//Done
    private void ThrowGrenade()
    {
        if (isGrenadeGettingThrown && _inventory.grenadeAmount > 0)
        {
            Transform handTransform = GameObject.FindWithTag("Hand").transform;
            _inventory.grenadeAmount -= 1;
            GameObject grenade = Instantiate(_grenadePrefab, handTransform.position, transform.rotation);
            rb = grenade.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * _throwForce/3 + transform.up * _throwForce/6, ForceMode.Impulse);
            isGrenadeGettingThrown = false;
            Grenade grenadeScript = grenade.GetComponent<Grenade>();
            grenadeScript.explosionReady = true;
            bombWaiting = true;

        }
    }//Done
    private void Crouch()
    {
        if (isCrouching)
        {
            _animator.SetBool("isCrouching", true);
        }
        else if (!isCrouching)
            _animator.SetBool("isCrouching", false);
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }//Activation of script
}
