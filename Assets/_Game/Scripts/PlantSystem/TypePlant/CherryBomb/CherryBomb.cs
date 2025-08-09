using DG.Tweening;
using UnityEngine;

public class CherryBomb : Plant {
    public float damage = 5f;
    public float explosionRadius = 3f;
    public GameObject explodePrefab;
    public float scaleTarget = 0.8f;
    protected override void Start() {
        base.Start(); // Gọi Start() của Plant trước
        transform.DOScale(scaleTarget, 2f).OnComplete(() => {
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
                    health.TakeDamage(5);
                }
            }
        }

        // Hủy PotatoMine sau khi nổ
        Destroy(gameObject);
    }

    // Gợi ý thêm: vẽ phạm vi trong Scene view (debug)
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
