using UnityEngine;

public class AttackZombies : MonoBehaviour
{
    public int attackDamage = 20;
    public float attackRange = 1.5f; // Phạm vi tấn công
    public float attackCooldown = 1.0f; // Thời gian hồi chiêu giữa các đòn tấn công

    private float lastAttackTime;

    private void Update()
    {
        // Kiểm tra và tấn công zombie nếu có trong phạm vi
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Zombie"))
            {
                AttackZombie(hitCollider.GetComponent<ZombieHealth>());
            }
        }
    }

    private void AttackZombie(ZombieHealth zombieHealth)
    {
        if (zombieHealth == null) return;

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            zombieHealth.TakeDamage(attackDamage);
            lastAttackTime = Time.time;
            Debug.Log("Unit attacks zombie!");
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Vẽ phạm vi tấn công trong editor để dễ dàng kiểm tra
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
