using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float runSpeed = 5f;
    public float jumpPower = 3f;
    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 movement = Vector3.zero;
    private float gravity = Physics.gravity.y;

    private float height;
    private bool jumpOver = false;
    private bool freezMovement = false;
    public float inputSmoothSpeed = 5f; // Increase for faster response
    public float inputRotationSpeed = 5f; // Increase for faster response
    public Animator animator;

    [SerializeField] private float currentInputMagnitude = 0f;
    public float deceration;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        height = controller.height;
    }

    void LateUpdate()
    {
        if (!freezMovement)
        {
            HandleMovement();
            ApplyGravity();
        }

        controller.Move(new Vector3(moveDirection.x, gravity, moveDirection.z) * Time.deltaTime);
    }

    void HandleMovement()
    {
        Vector3 rawInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        float rawMagnitude = Mathf.Clamp01(rawInput.magnitude);

        currentInputMagnitude = Mathf.Lerp(currentInputMagnitude, rawMagnitude, inputSmoothSpeed * Time.deltaTime);
        animator.SetFloat("MoveX", currentInputMagnitude);
        // Reapply direction
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = (camForward * rawInput.z + camRight * rawInput.x).normalized;
        moveDirection = moveDir * runSpeed * currentInputMagnitude;

        // Rotate character if moving
        if (moveDir != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * inputRotationSpeed);
        }

    }

    void ApplyGravity()
    {
        if (controller.isGrounded)
        {
            gravity = -1f; // Slight downward force to stay grounded
        }
        else
        {
            gravity += Physics.gravity.y * Time.deltaTime;
        }
    }
}
