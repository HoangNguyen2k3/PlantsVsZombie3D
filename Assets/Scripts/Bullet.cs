using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 1;

    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            ZombieController zombie = other.GetComponent<ZombieController>();
            if (zombie != null)
            {
                zombie.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
