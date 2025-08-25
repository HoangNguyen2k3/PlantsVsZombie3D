using DG.Tweening;
using UnityEngine;

public class SunBomb : Plant {
    public int damage = 5;
    public float explosionRadius = 3f;
    public GameObject explodePrefab;
    public float scaleTarget = 0.8f;

    public Transform posAppear;
    public GameObject sunPrefab;
    protected override void Start() {
        base.Start(); // Gọi Start() của Plant trước
                      // Lưu scale ban đầu
        Vector3 startScale = transform.localScale;

        // Scale gấp đôi (0.8 lần hoặc 2 lần tuỳ bạn muốn)
        Vector3 targetScale = startScale * 2f; // gấp đôi
                                               // Vector3 targetScale = startScale * 0.8f; // nhỏ hơn

        transform.DOScale(targetScale, 2f).OnComplete(() => {
            Attack();
        });
    }

    public override void Attack() {
        // Tạo hiệu ứng vụ nổ
        if (explodePrefab != null) {
            Instantiate(explodePrefab, transform.position, Quaternion.identity);
        }

        // Tìm tất cả các collider trong bán kính vụ nổ
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders) {
            if (hit.CompareTag("Enemy")) {
                // Gây sát thương (giả sử Enemy có script "EnemyHealth" với hàm "TakeDamage")
                ZombieController health = hit.GetComponent<ZombieController>();
                if (health != null) {
                    health.TakeDamage(damage);
                }
            }
        }
        Instantiate(sunPrefab, posAppear.position, Quaternion.identity);
        Instantiate(sunPrefab, posAppear.position, Quaternion.identity);
        Instantiate(sunPrefab, posAppear.position, Quaternion.identity);
        // Hủy PotatoMine sau khi nổ
        Destroy(gameObject);
    }

    // Gợi ý thêm: vẽ phạm vi trong Scene view (debug)
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
