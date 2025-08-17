using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SunPrefab : MonoBehaviour {
    public float shootForce = 5f;
    public float spreadAngle = 20f; // góc lệch trái/phải ngẫu nhiên
    Rigidbody rb;
    void Start() {
        rb = GetComponent<Rigidbody>();

        // Hướng cơ bản: bay lên
        Vector3 baseDirection = Vector3.up;

        // Random hướng trong hình nón với góc spreadAngle
        Vector3 randomDir = Random.insideUnitSphere; // random 3D vector trong hình cầu
        randomDir.y = Mathf.Abs(randomDir.y); // đảm bảo nó bay lên trên (không bay xuống đất)

        Vector3 shootDirection = (baseDirection + randomDir * 0.5f).normalized;

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
            GamePlayManager.Ins.ChangeNumSun(50);
            Destroy(gameObject);
        }
    }
}
