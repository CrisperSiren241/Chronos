using UnityEngine;

public class AttackMagic : MonoBehaviour
{
    private PlayerMovementAdvanced pma;
    public Animator animator;
    public Transform playerBody;
    public Transform cameraTransform;

    private bool aiming = false;

    void Start()
    {
        pma = GetComponent<PlayerMovementAdvanced>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            aiming = true;
            pma.restricted = true;
            AlignPlayerWithCamera();
            animator.SetBool("Prepare", true);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            aiming = false;
            pma.restricted = false;
            animator.SetBool("Prepare", false);
        }

        if (aiming)
        {
            RotatePlayerWithCamera();
        }

        if (aiming && Input.GetKeyDown(KeyCode.Mouse0)) Attack();
    }


    void Attack()
    {
        animator.SetTrigger("Attack");
    }

    void AlignPlayerWithCamera()
    {
        float cameraYaw = cameraTransform.eulerAngles.y;
        playerBody.rotation = Quaternion.Euler(0, cameraYaw, 0);
    }

    void RotatePlayerWithCamera()
    {
        float cameraYaw = cameraTransform.eulerAngles.y;
        playerBody.rotation = Quaternion.Lerp(playerBody.rotation, Quaternion.Euler(0, cameraYaw, 0), Time.deltaTime * 10f);
    }
}
