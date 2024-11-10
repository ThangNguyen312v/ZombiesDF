using UnityEngine;
using UnityEngine.AI;

public class ZombieBehavior : MonoBehaviour
{
    public float attackDistance = 1.5f;       // Khoảng cách để zombie có thể tấn công
    public float detectRange = 10f;           // Phạm vi phát hiện Unit
    public float runDistance = 5f;            // Phạm vi bắt đầu chạy
    public float damage = 10f;                // Lượng sát thương khi tấn công
    public float attackCooldownZ = 0.5f;
    public float lastattack;
    private Transform target;                 // Mục tiêu hiện tại (Unit)
    private NavMeshAgent agent;
    private Animator animator;                // Dùng cho animation của zombie
    private bool isDead = false;              // Trạng thái chết

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Điều chỉnh tốc độ di chuyển ban đầu
        agent.speed = 1f;
        agent.stoppingDistance = attackDistance; // Dừng lại ở khoảng cách tấn công
    }

    void Update()
    {
        if (isDead) return;

        FindClosestUnit();

        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (distanceToTarget <= detectRange)
            {
                // Kiểm tra nếu zombie đang chạy hoặc đi bộ
                bool shouldRun = distanceToTarget <= runDistance;
                animator.SetBool("isRunning", shouldRun);
                agent.speed = shouldRun ? 4.5f : 2f;

                // Đuổi theo Unit
                if (distanceToTarget > attackDistance)
                {
                    agent.SetDestination(target.position);
                    animator.SetBool("isWalking", !shouldRun);
                    animator.SetBool("isAttacking", false);
                }
                else
                {
                    // Nếu trong khoảng cách tấn công, dừng lại và tấn công
                    agent.ResetPath();
                    Attack();
                }
            }
            else
            {
                // Trở về trạng thái chờ khi không có Unit trong tầm
                animator.SetBool("isIdle", true);
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
                animator.SetBool("isAttacking", false);
            }
        }
        else
        {
            // Nếu không có mục tiêu
            animator.SetBool("isIdle", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isAttacking", false);
        }
    }

    void FindClosestUnit()
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
        float closestDistance = Mathf.Infinity;
        Transform closestUnit = null;

        foreach (GameObject unit in units)
        {
            float distance = Vector3.Distance(transform.position, unit.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestUnit = unit.transform;
            }
        }

        target = closestUnit;
    }

    void Attack()
    {
        animator.SetBool("isAttacking", true);

        Unit unitScript = target.GetComponent<Unit>(); 
        if (unitScript == null) return;
        if(Time.time - lastattack >= attackCooldownZ)
        {
            unitScript.TakeDmage((int)damage);
            lastattack = Time.time;
            Debug.Log("Hit Unit");
        }
    }

    public void Die()
    {
        isDead = true;
        animator.SetBool("isDead", true);
        animator.SetBool("isIdle", false);
        agent.enabled = false;
        GameManager.instance.OnZombieKilled();
        Destroy(gameObject);
    }
}
