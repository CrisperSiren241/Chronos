using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrabbing : MonoBehaviour
{
    [Header("References")]
    public PlayerMovementAdvanced pm;
    public Transform orientation;
    public Transform cam;
    public Rigidbody rb;
    public Animator animator;

    [Header("Ledge Grabbing")]
    public float moveToLedgeSpeed;
    public float maxLedgeGrabDistance;

    public float minTimeOnLedge;
    private float timeOnLedge;

    public bool holding;

    [Header("Ledge Jumping")]
    public KeyCode jumpKey = KeyCode.Space;
    public float ledgeJumpForwardForce;
    public float ledgeJumpUpwardForce;

    [Header("Ledge Detection")]
    public float ledgeDetectionLength;
    public float ledgeSphereCastRadius;
    public LayerMask whatIsLedge;

    private Transform lastLedge;
    private Transform currLedge;

    private RaycastHit ledgeHit;

    [Header("Exiting")]
    public bool exitingLedge;
    public float exitLedgeTime;
    private float exitLedgeTimer;

    private void Update()
    {
        LedgeDetection();
        SubStateMachine();
    }

    private void SubStateMachine()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        bool anyInputKeyPressed = horizontalInput != 0 || verticalInput != 0;

        // SubState 1 - Holding onto ledge
        if (holding)
        {
            FreezeRigidbodyOnLedge();

            timeOnLedge += Time.deltaTime;

            if (timeOnLedge > minTimeOnLedge && anyInputKeyPressed) ExitLedgeHold();

            if (Input.GetKeyDown(jumpKey)) LedgeJump();
        }

        // Substate 2 - Exiting Ledge
        else if (exitingLedge)
        {
            if (exitLedgeTimer > 0) exitLedgeTimer -= Time.deltaTime;
            else exitingLedge = false;
        }
    }

    private void LedgeDetection()
    {
        bool ledgeDetected = Physics.SphereCast(transform.position, ledgeSphereCastRadius, cam.forward, out ledgeHit, ledgeDetectionLength, whatIsLedge);

        if (!ledgeDetected) return;

        float distanceToLedge = Vector3.Distance(transform.position, ledgeHit.transform.position);
        if (ledgeHit.transform == lastLedge) return;

        // Определяем, смотрит ли игрок в сторону уступа
        Vector3 directionToLedge = (ledgeHit.point - transform.position).normalized;
        float dot = Vector3.Dot(cam.forward.normalized, directionToLedge);

        if (dot < 0.5f) return; // Если игрок не смотрит в сторону уступа, не срабатывает

        if (distanceToLedge < maxLedgeGrabDistance && !holding) EnterLedgeHold();
    }


    private void LedgeJump()
    {
        ExitLedgeHold();
        animator.SetBool("isGrab", false);
        animator.SetBool("isJump", true);
        Invoke(nameof(DelayedJumpForce), 0.05f);
    }
    private void DelayedJumpForce()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // A (-1) / D (1)
        float verticalInput = Input.GetAxisRaw("Vertical"); // W (1) / S (-1)

        Vector3 jumpDirection = Vector3.zero;

        if (verticalInput > 0)
            jumpDirection = orientation.up;
        else
        {
            if (verticalInput < 0) jumpDirection -= orientation.forward;
            if (horizontalInput > 0) jumpDirection += orientation.right;
            if (horizontalInput < 0) jumpDirection -= orientation.right;
        }


        if (jumpDirection == Vector3.zero)
            jumpDirection = orientation.up;

        jumpDirection.Normalize();

        Vector3 forceToAdd = jumpDirection * ledgeJumpForwardForce + orientation.up * ledgeJumpUpwardForce;

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(forceToAdd, ForceMode.Impulse);

        animator.SetBool("isFalling", true);
        animator.SetBool("isJump", false);

    }

    private void EnterLedgeHold()
    {
        holding = true;

        pm.unlimited = true;
        pm.restricted = true;

        currLedge = ledgeHit.transform;
        lastLedge = ledgeHit.transform;

        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;

        animator.SetBool("isGrab", true);
        animator.SetBool("isJump", false);
        animator.SetBool("isFalling", false);


    }

    private void FreezeRigidbodyOnLedge()
    {
        rb.useGravity = false;

        Vector3 directionToLedge = currLedge.position - transform.position;
        float distanceToLedge = Vector3.Distance(transform.position, currLedge.position);

        // Move player towards ledge
        if (distanceToLedge > 1f)
        {
            if (rb.linearVelocity.magnitude < moveToLedgeSpeed)
                rb.AddForce(directionToLedge.normalized * moveToLedgeSpeed * 1000f * Time.deltaTime);
        }

        // Hold onto ledge
        else
        {
            if (!pm.freeze) pm.freeze = true;
            animator.SetBool("isGrab", true);
            animator.SetBool("isJump", false);
            animator.SetBool("isFalling", false);
            if (pm.unlimited) pm.unlimited = false;
        }

        // Exiting if something goes wrong
        if (distanceToLedge > maxLedgeGrabDistance) ExitLedgeHold();


    }

    private void ExitLedgeHold()
    {
        exitingLedge = true;
        exitLedgeTimer = exitLedgeTime;

        holding = false;
        timeOnLedge = 0f;

        pm.restricted = false;
        pm.freeze = false;

        rb.useGravity = true;

        StopAllCoroutines();
        Invoke(nameof(ResetLastLedge), 1f);
        animator.SetBool("isGrab", false);
        animator.SetBool("isJump", false);
        animator.SetBool("isFalling", true);
    }

    private void ResetLastLedge()
    {
        lastLedge = null;
    }
}
