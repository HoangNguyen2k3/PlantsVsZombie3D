using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SunPrefab : MonoBehaviour {
    public float shootForce = 5f;
    public float spreadAngle = 20f; // góc lệch trái/phải ngẫu nhiên
    Rigidbody rb;
    void Start() {
        rb = GetComponent<Rigidbody>();

        // Tạo hướng xiên lên
        Vector3 baseDirection = Vector3.up + Vector3.forward * 0.5f;
        float randomAngle = Random.Range(-spreadAngle, spreadAngle);
        Quaternion spreadRotation = Quaternion.Euler(0f, randomAngle, 0f);
        Vector3 shootDirection = spreadRotation * baseDirection.normalized;

        rb.AddForce(shootDirection * shootForce, ForceMode.Impulse);
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("GridCell") || other.CompareTag("Ground")) {
            if (!rb.isKinematic) {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
            }
        }
        if (other.CompareTag("Player")) {
            Destroy(gameObject);
        }
    }
}
