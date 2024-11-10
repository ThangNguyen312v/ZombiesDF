using UnityEngine;

public class UnitHealth : MonoBehaviour
{
    public Unit unit;
 

    private void Start()
    {
        unit = GetComponent<Unit>();
    }
    // Hàm nhận sát thương
    public void TakeDamage(int damage)
    {
        unit.unitMaxHealt -= damage;

        if (unit.unitMaxHealt <= 0)
        {
            Die();  // Unit chết khi sức khỏe bằng 0 hoặc thấp hơn
        }
    }

    // Hàm chết của Unit
    private void Die()
    {
        Debug.Log(gameObject.name + " has died.");
        // Thêm logic chết như tắt Unit, hoặc remove nó khỏi danh sách allUnitsList
        UnitSelectionManager.Instance.allUnitsList.Remove(gameObject);  // Xóa khỏi danh sách nếu cần
        Destroy(gameObject);  // Xóa object unit khỏi game
    }
}
