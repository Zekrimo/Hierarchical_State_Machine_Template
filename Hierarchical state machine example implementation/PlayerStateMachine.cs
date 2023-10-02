﻿/**
/// PlayerStateMachine is the context class of the player controller, also known as the player controller.
/// This class inherits from NetworkBehaviour and passes on all NetworkBehaviour functionality to the concrete states.
@author: Sonny Selten
@date 19 april 2023
**/

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

using FishNet;
using FishNet.Object;

using Cinemachine;
using Core.GameManager;
using Core.GrapplingSystem;
using Multiplayer.UI;

//this class is the context class of the player controller. 
//also known als the player controller
//this class will inherit from network behavior and pass on all networkbehavior functionality to the concrete states


namespace Multiplayer.PlayerController
{
    [RequireComponent(typeof(Rigidbody))]
    //[RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Grapple))]
    public class PlayerStateMachine : NetworkBehaviour, IRespawnable
    {
        ///   State Variables
        private PlayerBaseState _currentState;
        private PlayerStateFactory _states;

        ///   Member Variables
        [Header("Movement Settings")]
        [SerializeField, Tooltip("Defines the force that will be added to the movement speed of the player")] private float _moveSpeed = 125f;
        [SerializeField, Tooltip("Defines the amount of drag applied to the player")] private float _dragResistance = 5f;
        [SerializeField, Tooltip("Defines the amount of drag applied to the player")] private float _dragResistanceInAir = 0f;
        [SerializeField, Tooltip("Defines which mask layer to check for ground collision")] private LayerMask _groundLayerMask;
        [SerializeField, Tooltip("Defines the position of the collider to check for ground collision")] private Vector3 _groundCheckPosition = new(0, -0.49f, 0);

        [Header("Strafe Settings")]
        [SerializeField, Tooltip("Defines the strafing speed of the player (movement with added force in air)")] private float _strafeForce = 20f;
        [SerializeField, Tooltip("Defines the maximum speed a player can reach (no force will be applied beyond this speed)")] private float _maxSpeedForStrafing = 15f;

        [Header("Jump Settings")]
        [SerializeField, Tooltip("Defines the jump height of the player")] private float _jumpHeight = 8f;
        [SerializeField, Tooltip("Defines the time it takes for the player to be able to jump again")] private float _jumpCooldown = 0.25f;

        [Header("Dash Settings")]
        [SerializeField, Tooltip("Defines the time it takes for the player to dash again")] private float _dashCooldown = 5;
        [SerializeField, Tooltip("Defines the amount of time that the gravity is disabled when dashing")] private float _gravityDashCooldown = 0.25f;
        [SerializeField, Tooltip("Defines the speed of the dash action")] private float _dashSpeed = 5;
        [SerializeField, Tooltip("Defines the action triggerd when the dash cooldown is active?")] private UnityEvent<float> _onDashCooldown;
        [SerializeField, Tooltip("Defines the action triggerd when the dash is active?")] private UnityEvent _onDash;

        [Header("Camera Settings")]
        [SerializeField, Tooltip("Defines the position of the camera")] private Transform _head;
        [SerializeField, Tooltip("Defines the sensitivity of the camera movement")] private float _mouseSensitivity = 1f;
        [SerializeField, Tooltip("Defines the angle of the camera head movement")] private Vector2 _headAngleMinMax = new(-89f, 90f);
        [SerializeField, Tooltip("Defines the radius of the head movement")] private float _radius = 0.49f;

        [field: SerializeField]
        public UnityEvent onStartRunning { get; set; }
        [field: SerializeField]
        public UnityEvent onStopRunning { get; set; }

        [Header("Debug")]
        [SerializeField, Tooltip("Toggle this on if you want to see debug logs for the current player superstate and substate")] private Boolean _showCurrentStateInLog = false;

        ///     private variables
        private Grapple _grapple;
        private bool _isGrounded;
        private bool _isReadyToJump;
        private bool _isDashing;
        private bool _isGrapplePressed;
        private bool _isGrappleOut;

        private Vector2 _movementInput;
        private bool _isMovementPressed;
        private bool _isJumpPressed;
        private Vector2 _mouseLook;
        private float _angle;

        private Rigidbody _rb;
        private Animator _animator;
        private CinemachineVirtualCamera _virtualCamera;
        private bool _isDashCompleted;
        private bool _isDashPressed;





        ///    getters and setters
        public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
        public float MoveSpeed { get { return _moveSpeed; } }
        public float MaxSpeedForStrafing { get { return _maxSpeedForStrafing; } }
        public float StrafeForce { get { return _strafeForce; } }
        public bool IsJumpPressed { get { return _isJumpPressed; } set { _isJumpPressed = value; } }
        public bool IsMovementPressed { get { return _isMovementPressed; } }
        public float DragResistance { get { return _dragResistance; } }
        public float DragResistanceInAir
        {
            get { return _dragResistanceInAir; }
        }
        public float JumpHeight { get { return _jumpHeight; } }
        public float MouseSensitivity { get { return _mouseSensitivity; } }
        public Vector2 HeadAngleMinMax { get { return _headAngleMinMax; } }
        public LayerMask GroundLayerMask { get { return _groundLayerMask; } }
        public float Radius { get { return _radius; } }
        public Vector3 GroundCheckPosition { get { return _groundCheckPosition; } }
        public float JumpCooldown { get { return _jumpCooldown; } }
        public Transform Head { get { return _head; } }
        public bool IsGrounded { get { return _isGrounded; } }
        public bool IsReadyToJump { get { return _isReadyToJump; } }
        public Vector2 MovementInput { get { return _movementInput; } set { _movementInput = value; } }
        public Vector2 mouseLook { get { return _mouseLook; } set { _mouseLook = value; } }
        public float Angle { get { return _angle; } set { _angle = value; } }
        public Rigidbody Rigidbody { get { return _rb; } set { _rb = value; } }
        public Animator Animator { get { return _animator; } }
        public Boolean ShowCurrentStateInLog { get { return _showCurrentStateInLog; } }
        public Boolean IsDashing { get { return _isDashing; } }
        public float DashCooldown { get { return _dashCooldown; } }
        public float GravityDashCooldown { get { return _gravityDashCooldown; } }
        public float DashSpeed { get { return _dashSpeed; } }
        public bool IsGrapplePressed { get { return _isGrapplePressed; } }
        public bool IsGrappleOut { get { return _isGrappleOut; } set { _isGrappleOut = value; } }
        public Grapple Grapple { get { return _grapple; } }
        public bool IsDashCompleted { get { return _isDashCompleted; } set { _isDashCompleted = value; } }
        public bool IsDashPressed { get { return _isDashPressed; } set { _isDashPressed = value; } }




        ////  callback functions
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _animator = GetComponentInChildren<Animator>();

            _rb.freezeRotation = true;
            _isReadyToJump = true;

            _grapple = GetComponent<Grapple>();
            GameManagerStateMachine.instance.mouseSensitivity = 0.25f;

            //state setup
            _states = new PlayerStateFactory(this);
            _currentState = _states.Grounded(); //set initials state to grounden
            _currentState.EnterStates(); // call the entry function of the curren state
        }

        //   Update is called once per frame
        private void Update()
        {
            //check if the player is the owner of the object, if not ignore the update
            if (!IsOwner) return;

            //Logic
            HandleRotation();
            GroundCheck();
            _currentState.UpdateStates();
            SetMouseSensitivity();
        }

        //  FixedUpdate is called to handle physic updates
        private void FixedUpdate()
        {
            if (!IsOwner) return;
            //only move when on the ground 
            _currentState.FixedUpdateStates();
        }

        //-------------------------------------------------------------------------------
        ////  Input functions
        public void OnMovementInput(InputAction.CallbackContext context)
        {
            _movementInput = context.ReadValue<Vector2>();
            _isMovementPressed = true;
        }

        public void OnJumpInput(InputAction.CallbackContext context)
        {
            _isJumpPressed = context.ReadValueAsButton();
        }

        public void OnDashInput(InputAction.CallbackContext context)
        {
            if (!_isDashing)
                _isDashPressed = true;
            Dash();
        }

        public void OnLookInput(InputAction.CallbackContext context)
        {
            _mouseLook = context.ReadValue<Vector2>();
        }

        public void OnGrappleInput(InputAction.CallbackContext context)
        {
            if (!IsOwner) return;
            if (context.performed)
            {
                Animator.SetBool("IsShooting", true);
                _isGrapplePressed = true;
            }
            else if (context.canceled)
            {
                Grapple.Release();
                _isGrapplePressed = false;
                Animator.SetBool("IsShooting", false);
            }
        }

        public void OnVaultInput(InputAction.CallbackContext context)
        {
            Debug.Log("VAULTING NOT YET IMPLEMENTED!");
            //TODO
        }

        /// <summary>
        ///     Sets the mouse sensitivity to the given value stored in the game manager state machine
        /// </summary>
        public void SetMouseSensitivity()
        {
            if (IsOwner) _mouseSensitivity = GameManagerStateMachine.instance.mouseSensitivity;
        }


        /// Member Functions
        private void HandleRotation()
        {
            transform.Rotate(Vector3.up, _mouseLook.x * _mouseSensitivity);
            _angle = Math.Clamp(_angle - _mouseLook.y * _mouseSensitivity, _headAngleMinMax.x, _headAngleMinMax.y);
            if (_head.transform.rotation.z != 0)
            {
                _head.localRotation = Quaternion.Euler(new Vector3(_angle, 0, _head.transform.rotation.z * -1));
            }
            else
            {
                _head.localRotation = Quaternion.Euler(new Vector3(_angle, 0, 0));
            }
        }




        private void Dash()
        {
            if (_isDashing) return;
            //Dont dash if we are dashing
            _isDashing = true;
            _rb.velocity = _head.forward * Mathf.Max(_dashSpeed, _rb.velocity.magnitude);
            _rb.useGravity = false;
            _onDash.Invoke();

            StartCoroutine(nameof(ResetDash));
            Invoke(nameof(ResetGravity), _gravityDashCooldown);
        }

        private IEnumerator ResetDash()
        {
            _isDashCompleted = true;
            for (var percentage = 0; percentage < 100; percentage++)
            {
                yield return new WaitForSeconds(_dashCooldown / 100f);
                _onDashCooldown?.Invoke(percentage);
            }

            _isDashing = false;
        }

        private void ResetGravity()
        {
            _rb.useGravity = true;
        }

        private void GroundCheck()
        {
            _isGrounded = Physics.CheckSphere(transform.position + _groundCheckPosition, _radius, _groundLayerMask);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position + _groundCheckPosition, _radius);
        }

        public void OnObjectRespawn(Quaternion rotation)
        {
            Grapple.Release();
            Transform trans;
            (trans = transform).rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
            _angle = trans.eulerAngles.x;
        }

        public void PlayerGetsTaggedAnimation()
        {
            Animator.SetTrigger("IsTagged");

        }
    }
}