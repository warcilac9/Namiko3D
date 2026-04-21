using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class MovementHandler : MonoBehaviour
{
    [Header("Player")]
    public float MoveSpeed = 2.0f;
    public float SpeedChangeRate = 10f;
    public float Gravity = -9.81f;
    public float TerminalVelocity = -53f;

    private float _speed;
    private float _targetRotation;
    private GameObject _mainCamera;
    private float _verticalVelocity;
    
    private CharacterController _controller;
    public InputHandler input;
    public groundCheck groundCheck;

    void Awake()
    {
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        if (groundCheck == null)
            groundCheck = GetComponentInChildren<groundCheck>();
    }

    void Update()
    {
        ApplyGravity();
        Move();
    }

    private void ApplyGravity()
    {
        if (groundCheck.isGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity = -2f;
        }
        else
        {
            _verticalVelocity += Gravity * Time.deltaTime;
            if (_verticalVelocity < TerminalVelocity)
                _verticalVelocity = TerminalVelocity;
        }
    }

    private void Move()
    {
        float targetSpeed = MoveSpeed;
        float speedOffset = 0.1f;
        float inputMagnitude = input.movementValue.magnitude;
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
        Vector3 inputDirection = new Vector3(input.movementValue.x, 0.0f, input.movementValue.y).normalized;

        if (input.movementValue == Vector2.zero) targetSpeed = 0f;

        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        if (input.movementValue != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
        }
        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
    }

    public void ApplyJumpVelocity(float jumpVelocity)
    {
        _verticalVelocity = jumpVelocity;
    }

    public Vector3 GetCurrentVelocity()
    {
        return _controller.velocity;
    }
}
