using UnityEngine;

public class SunflowerPlant : Plant {
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
        GameObject sun = Instantiate(sunPrefab, posAppear.position, Quaternion.identity);
        Debug.Log("Sunflower bắn mặt trời theo hướng cong!");
    }
}
