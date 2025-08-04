using System.Collections;
using UnityEngine;

public class ZombieController : MonoBehaviour {
    public float speed;
    public int maxHealth;
    private int currentHealth;

    private float attackRange = 1.0f;
    private int damagePerHit = 1;
    private float attackInterval = 1.0f; //chu ky tan cong

    private bool isDead = false;
    private bool isAttacking = false;

    private GameObject targetPlant;

    public void Start() {
        currentHealth = maxHealth;
    }

    private void Update() {
        if (isDead) return;
        if (!isAttacking) {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            DetectPlant();
        }

    }

    void DetectPlant() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange)) {
            if (hit.collider.CompareTag("Plant")) {
                targetPlant = hit.collider.gameObject;
                StartCoroutine(AttackPlant(targetPlant));
            }
        }
    }

    IEnumerator AttackPlant(GameObject plant) {
        /*        isAttacking = true;

                Plant plantHealth = plant.GetComponent<Plant>();
                if (plantHealth == null)
                {
                    isAttacking = false;
                    yield break;
                }

                while (!isDead && plantHealth != null && !plantHealth.IsDead())
                {
                    plantHealth.TakeDamage(damagePerHit);
                    yield return new WaitForSeconds(attackInterval);
                }

                isAttacking = false;*/
        yield return null;
    }

    public void TakeDamage(int amount) {
        if (isDead) return;

        currentHealth -= amount;

        Debug.Log("Zombie HP: " + currentHealth);

        if (currentHealth <= 0) {
            Die();
        }
    }

    void Die() {
        isDead = true;

        StopAllCoroutines();

        Destroy(gameObject, 2f);
    }
}
