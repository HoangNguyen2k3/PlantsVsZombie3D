using UnityEngine;

public class SunGun : Plant {
    public GameObject sunPrefab;
    public float spawnRate = 5f;
    public float shootForce = 5f;
    public float spreadAngle = 20f; // góc lệch ngẫu nhiên sang trái/phải

    private float timer;
    public Transform posAppear;

    public GameObject bulletPrefab;
    public Transform shootPoint;
    void Update() {
        timer += Time.deltaTime;
        if (timer >= spawnRate) {
            timer = 0f;
            Attack();
        }
    }

    public override void Attack() {
        Instantiate(sunPrefab, posAppear.position, Quaternion.identity);
        Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
    }
}
