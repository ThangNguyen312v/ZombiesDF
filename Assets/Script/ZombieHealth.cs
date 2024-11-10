using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public float health = 100f;

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        ZombieBehavior zombie = GetComponent<ZombieBehavior>();
        if (zombie != null)
        {
            zombie.Die(); // Gọi hành động chết
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
