using DG.Tweening;
using UnityEngine;

public class CherryNut : Plant {
    public int damage = 5;
    public float explosionRadius = 3f;
    public GameObject explodePrefab;
    public float scaleTarget = 1.8f;
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

        // Hủy PotatoMine sau khi nổ
        Destroy(gameObject);
    }
    public override void Die() {
        transform.DOScale(transform.localScale * 1.5f, 2f).OnComplete(() => {
            Attack();
            //base.Die();
        });
    }
    // Gợi ý thêm: vẽ phạm vi trong Scene view (debug)
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
