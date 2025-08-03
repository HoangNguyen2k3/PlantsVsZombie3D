using UnityEngine;

public abstract class Plant : MonoBehaviour {
    public int maxHP = 100;
    protected int currentHP;

    protected virtual void Start() {
        currentHP = maxHP;
    }

    public void TakeDamage(int amount) {
        currentHP -= amount;
        if (currentHP <= 0) {
            Die();
        }
    }

    protected virtual void Die() {
        // Mặc định: phá hủy object
        Destroy(gameObject);
    }

    // Bắt buộc cây con phải định nghĩa hành vi tấn công
    public abstract void Attack();
}
