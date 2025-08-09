using DG.Tweening;
using UnityEngine;

public class Squash : Plant {
    public int damage = 5;
    public float explosionRadius = 2f; // bán kính gây damage
    public Vector3 posAttack;
    public float jumpHeight = 5f;
    public float moveDuration = 0.5f; // thời gian di chuyển
    public GameObject explosion;
    public override void Attack() {
        // B1: Quay mặt về hướng tấn công
        Vector3 dir = (posAttack - transform.position).normalized;
        // Quaternion lookRot = Quaternion.LookRotation(dir);
        transform.DOLocalRotate(new Vector3(0, -90, 0), 0.2f).OnComplete(() => {
            // B2: Tạo path để nhảy (cung parabol đơn giản: giữa đường cao hơn)
            Vector3 midPoint = (transform.position + posAttack) / 2f;
            midPoint.y += jumpHeight;
            Vector3[] path = new Vector3[] {
            midPoint,
            posAttack
        };

            // B3: Nhảy đến vị trí tấn công
            transform.DOPath(path, moveDuration, PathType.CatmullRom)
                .SetEase(Ease.InQuad)
                .OnComplete(() => {
                    // Giậm xuống
                    transform.DOMoveY(transform.position.y - 0.5f, 0.1f)
                        .SetRelative(true)
                        .SetEase(Ease.InBounce)
                        .OnComplete(() => {
                            // B4: Gây damage
                            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
                            foreach (Collider hit in colliders) {
                                if (hit.CompareTag("Enemy")) {
                                    ZombieController health = hit.GetComponent<ZombieController>();
                                    if (health != null) {
                                        health.TakeDamage(damage);
                                    }
                                }
                            }
                            Instantiate(explosion, transform.position, Quaternion.identity);
                            // B5: Biến mất
                            Destroy(gameObject);
                        });
                });
        });


    }

    // Hiển thị vùng gây damage trong Editor
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
