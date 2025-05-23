using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dave.PhysicsExtension;

public class NextGenWallRunning : MonoBehaviour
{
    [Header("Wall Running")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce = 200f;
    public float wallRunJumpUpForce = 7f;
    public float wallRunJumpSideForce = 7f;
    public float maxWallRunTime = 1f;
    public float wallClimbSpeed;
    private float wallRunTimer;

    [Header("Input")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode upwardsRunKey = KeyCode.LeftShift;
    public KeyCode downwardsRunKey = KeyCode.LeftControl;
    private bool upwardsRunning;
    private bool downwardsRunning;
    private float horizontalInput;
    private float verticalInput;

    [Header("Limitations")]
    public bool doJumpOnEndOfTimer = false;
    public bool resetDoubleJumpsOnNewWall = true;
    public bool resetDoubleJumpsOnEveryWall = false;
    public int allowedWallJumps = 1;

    [Header("Detection")]
    public float wallCheckDistance = 0.7f;
    public float minJumpHeight = 2f;

    [Header("Exiting")]
    private bool exitingWall;
    public float exitWallTime = 0.2f;
    private float exitWallTimer;

    [Header("Gravity")]
    public bool useGravity = false;
    public float gravityCounterForce;
    public float yDrossleSpeed;

    [Header("References")]
    public Transform orientation;
    private PlayerMovementAdvancedFinished pm;
    private Rigidbody rb;
    /// private PlayerCam cam;

    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;

    private bool wallLeft;
    private bool wallRight;

    private bool wallRemembered;
    private Transform lastWall;

    private int wallJumpsDone;


    public TextMeshProUGUI text_wallState;

    private void Start()
    {
        if (whatIsWall.value == 0)
            whatIsWall = LayerMask.GetMask("Default");

        if (whatIsGround.value == 0)
            whatIsGround = LayerMask.GetMask("Default");

        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovementAdvancedFinished>();
        /// cam = GetComponent<PlayerCam>();
    }

    private void Update()
    {
        CheckForWall();
        StateMachine();
        JumpPrediction();

        // if grounded, next wall is a new one
        if (pm.grounded && lastWall != null)
            lastWall = null;
    }

    private void FixedUpdate()
    {
        if (pm.wallrunning && !exitingWall)
            WallRunningMovement();
    }

    private void StateMachine()
    {
        // Getting Inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        upwardsRunning = Input.GetKey(upwardsRunKey);
        downwardsRunning = Input.GetKey(downwardsRunKey);

        // State 1 - Wallrunning
        if ((wallLeft || wallRight) && verticalInput > 0 && AboveGround() && !exitingWall)
        {
            // start wallrun
            if (!pm.wallrunning) StartWallRun();

            // wallrun timer
            wallRunTimer -= Time.deltaTime;

            if (wallRunTimer < 0 && pm.wallrunning)
            {
                if (doJumpOnEndOfTimer)
                    WallJump();

                else
                {
                    exitingWall = true;
                    exitWallTimer = exitWallTime;
                }
            }

            // wall jump
            if (Input.GetKeyDown(jumpKey)) WallJump();
        }

        // State 2 - Exiting (Here)
        else if (exitingWall)
        {
            //pm.restricted = true;

            if (pm.wallrunning)
                StopWallRun();

            if (exitWallTimer > 0)
                exitWallTimer -= Time.deltaTime;

            if (exitWallTimer <= 0)
                exitingWall = false;
        }

        // State 3 - None
        else
        {
            if (pm.wallrunning)
                StopWallRun();
        }

        if (!exitingWall && pm.restricted)
            pm.restricted = false;
    }

    /// do all of the raycasts
    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, whatIsWall);

        // reset readyToClimb and wallJumps whenever player hits a new wall
        if ((wallLeft || wallRight) && NewWallHit())
        {
            wallJumpsDone = 0;
            wallRunTimer = maxWallRunTime;
        }
    }

    private void RememberLastWall()
    {
        if (wallLeft)
            lastWall = leftWallHit.transform;

        if (wallRight)
            lastWall = rightWallHit.transform;
    }

    private bool NewWallHit()
    {
        if (lastWall == null)
            return true;

        if (wallLeft && leftWallHit.transform != lastWall)
            return true;

        else if (wallRight && rightWallHit.transform != lastWall)
            return true;

        return false;
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StartWallRun()
    {
        pm.wallrunning = true;

        // move to CheckForWall() later
        wallRunTimer = maxWallRunTime;

        rb.useGravity = useGravity;

        wallRemembered = false;

        // fov and cam tilt in full file
        /// cam.DoFov(100f);
        /// if(wallRight) cam.DoTilt(5f);
        /// if(wallLeft) cam.DoTilt(-5f);
    }

    private void WallRunningMovement()
    {
        print(2);

        // set gravity
        rb.useGravity = useGravity;

        // calculate directions

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        // lerp upwards velocity of rb to 0 if gravity is turned off

        float velY = rb.linearVelocity.y;

        /// is this smoothing needed?
        if (!useGravity)
        {
            if (velY > 0)
                velY -= yDrossleSpeed;

            rb.linearVelocity = new Vector3(rb.linearVelocity.x, velY, rb.linearVelocity.z);
        }

        // add forces

        // forward force
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        /// not the best way to handle this
        if (upwardsRunning)
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, wallClimbSpeed, rb.linearVelocity.z);
        if (downwardsRunning)
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, -wallClimbSpeed, rb.linearVelocity.z);

        if (!exitingWall && !(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
            rb.AddForce(-wallNormal * 100, ForceMode.Force);

        // counter gravity to some extent
        if (useGravity)
            rb.AddForce(transform.up * gravityCounterForce, ForceMode.Force);

        // remember the last wall

        if (!wallRemembered)
        {
            RememberLastWall();
            wallRemembered = true;
        }
    }

    private void StopWallRun()
    {
        rb.useGravity = true;

        pm.wallrunning = false;

        /// cam.ResetFov();
        /// cam.ResetTilt();
    }

    public void WallJump()
    {
        // idea: allow one full jump, the second one is without upward force

        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 forceToApply = transform.up * wallRunJumpUpForce + wallNormal * wallRunJumpSideForce;

        bool firstJump = wallJumpsDone < allowedWallJumps;
        wallJumpsDone++;

        // if not first jump, remove y component of force
        if (!firstJump)
            forceToApply = new Vector3(forceToApply.x, 0f, forceToApply.z);

        // add force
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        // rb.AddForce(forceToApply, ForceMode.Impulse);
        // rb.AddForce(orientation.forward * 1f, ForceMode.Impulse);

        // precision please
        rb.linearVelocity = PhysicsExtension.CalculateJumpVelocity(transform.position, markerSphere.position, maxJumpHeight);

        RememberLastWall();

        // stop wallRun
        // StopWallRun();

        print(1);
    }

    public Camera cam;
    public Transform camHolder;

    public Transform markerSphere;
    public Transform someSecondSphere;

    public float maxJumpRange;
    public float maxJumpHeight;

    public TextMeshProUGUI text_predictionState;

    private void JumpPrediction()
    {
        RaycastHit viewRayHit;

        if (Physics.Raycast(cam.transform.position, camHolder.forward, out viewRayHit, maxJumpRange, whatIsWall))
        {
            // Case 1 - raycast hits (in maxDistance)
            markerSphere.position = viewRayHit.point;

            text_predictionState.SetText("in distance");
        }

        else if (Physics.SphereCast(cam.transform.position, 1f, camHolder.forward, out viewRayHit, 10f, whatIsWall))
        {
            // Case 2 - raycast hits (out of maxDistance)

            // calculate nearest possible point
            Vector3 maxRangePoint = cam.transform.position + camHolder.forward * maxJumpRange;

            RaycastHit wallHit;
            if(Physics.Raycast(maxRangePoint, -viewRayHit.normal, out wallHit, 4f, whatIsWall))
            {
                markerSphere.position = wallHit.point;
                text_predictionState.SetText("out of distance, to wall");
            }
            else
            {
                someSecondSphere.position = viewRayHit.point;

                if(Vector3.Distance(cam.transform.position, viewRayHit.point) <= maxJumpRange)
                {
                    text_predictionState.SetText("out of distance, hitPoint");
                    markerSphere.position = viewRayHit.point;
                }
                else
                {
                    text_predictionState.SetText("out of distance, can't predict point..."); // -> same as case 3
                    markerSphere.position = cam.transform.position + camHolder.forward * maxJumpRange;
                }
            }
        }

        else
        {
            // Case 3 - raycast completely misses
            // -> Normal Jump
            // Gizmos.DrawWireSphere(cam.transform.position + camHolder.forward * maxJumpRange, .5f);
            markerSphere.position = cam.transform.position + camHolder.forward * maxJumpRange;
            text_predictionState.SetText("complete miss");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, orientation.right * wallCheckDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, -orientation.right * wallCheckDistance);


        // jump predicting
        Gizmos.color = Color.yellow;
    }
}