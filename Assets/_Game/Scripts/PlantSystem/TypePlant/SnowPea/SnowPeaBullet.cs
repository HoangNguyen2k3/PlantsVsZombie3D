using UnityEngine;

public class SnowPeaBullet : MonoBehaviour {
    public float speed = 5f;
    public GameObject explosion;

    void Update() {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy") || other.CompareTag("EndAttack")) {
            if (other.CompareTag("Enemy")) {
                var zombie = other.GetComponent<ZombieController>();
                zombie.TakeDamage(1);
                zombie.ApplySlow();
                Instantiate(explosion, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}
