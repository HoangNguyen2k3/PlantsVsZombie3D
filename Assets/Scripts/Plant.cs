using UnityEngine;

public class Plant : MonoBehaviour
{
    public int health = 5;
    private bool isDead = false;

    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float shootInterval = 1.5f;

    private void Start()
    {
        InvokeRepeating(nameof(Shoot), 1f, shootInterval);
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        health -= amount;

        if (health <= 0)
        {
            Die();
        }
    }

    public bool IsDead()
    {
        return isDead;
    }

    private void Die()
    {
        isDead = true;

        Destroy(gameObject);
    }
}
