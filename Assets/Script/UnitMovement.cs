using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Camera cam;
    private Animator animator;
    public LayerMask ground;

    public VirtualJoystick joystick;  // Tham chiếu tới joystick
    public bool isCommandedToMove;
    private bool isInitialized = false;

    public void Init()
    {
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (cam == null)
        {
            Debug.LogError("Main Camera not found. Make sure there's a Camera with the 'MainCamera' tag.");
        }

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on the Unit object.");
        }

        if (animator == null)
        {
            Debug.LogError("Animator component is missing on the Unit object.");
        }

        isInitialized = true;  // Đánh dấu rằng Init đã hoàn tất
    }

    public void Update()
    {
        if (!isInitialized) return;

        // Kiểm tra đầu vào từ joystick
        float horizontal = joystick.Horizontal();
        float vertical = joystick.Vertical();

        if (horizontal != 0 || vertical != 0)
        {
            Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;
            Vector3 targetPosition = transform.position + moveDirection;

            agent.SetDestination(targetPosition);
            isCommandedToMove = true;
            animator.SetBool("isMoving", true);
        }
        else if (agent.hasPath == false || agent.remainingDistance <= agent.stoppingDistance)
        {
            isCommandedToMove = false;
            animator.SetBool("isMoving", false);
        }
    }
}
