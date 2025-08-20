using UnityEngine;

public class TwinSunFlower : Plant {
    public GameObject sunPrefab;
    public float spawnRate = 5f;
    public float shootForce = 5f;
    public float spreadAngle = 20f; // góc lệch ngẫu nhiên sang trái/phải

    private float timer;
    public Transform posAppear;

    void Update() {
        timer += Time.deltaTime;
        if (timer >= spawnRate) {
            timer = 0f;
            Attack();
        }
    }

    public override void Attack() {
        Instantiate(sunPrefab, posAppear.position, Quaternion.identity);
        Instantiate(sunPrefab, posAppear.position, Quaternion.identity);
    }
}

