using System.Collections;
using UnityEngine;

public class PeashooterPlant : Plant {
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float attackRate = 1.5f;

    private float timer;

    public int time_Attacks = 1;
    void Update() {
        timer += Time.deltaTime;
        if (timer >= attackRate) {
            timer = 0f;
            Attack();
        }
    }

    public override void Attack() {
        StartCoroutine(AttackFourTime());
    }
    private IEnumerator AttackFourTime() {
        if (bulletPrefab && shootPoint) {
            for (int i = 0; i < time_Attacks; i++) {
                Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
                yield return new WaitForSeconds(attackRate / 12);
            }
        }
    }
}
