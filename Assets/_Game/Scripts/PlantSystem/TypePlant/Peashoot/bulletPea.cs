using UnityEngine;

public class bulletPea : MonoBehaviour {
    public float speed = 5f;

    void Update() {
        // Di chuyển theo trục X (qua phải)
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy") || other.CompareTag("EndAttack")) {
            if (other.CompareTag("Enemy")) {
                other.GetComponent<ZombieController>().TakeDamage(1);
            }
            Destroy(gameObject); // Hủy viên đạn
        }
    }
}
