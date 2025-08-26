using UnityEngine;

public class SnowPea : Plant {
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float attackRate = 1.5f;

    private float timer;
    void Update() {
        timer += Time.deltaTime;
        if (timer >= attackRate) {
            timer = 0f;
            Attack();
        }
    }

    public override void Attack() {
        if (bulletPrefab && shootPoint) {
            Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        }
    }
}
