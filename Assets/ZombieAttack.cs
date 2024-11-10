using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    public int damage = 10;                // Sát thương mà zombie gây ra
    public float attackRate = 1f;          // Tần suất tấn công
    private float nextAttackTime = 0f;     // Thời gian tiếp theo zombie có thể tấn công
    Animator animator;
    private void OnTriggerStay(Collider other)
    {
        
        if (Time.time >= nextAttackTime && other.CompareTag("Unit"))  // Kiểm tra nếu zombie đang tiếp xúc với Unit
        {
            UnitHealth unitHealth = other.GetComponent<UnitHealth>();
            if (unitHealth != null)
            {
                unitHealth.TakeDamage(damage);  // Gây sát thương lên Unit
                nextAttackTime = Time.time + attackRate;  // Cập nhật thời gian cho lần tấn công tiếp theo
            }
        }
    }
}
