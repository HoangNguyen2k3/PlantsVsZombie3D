using System.Collections;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    [Header("====== Base ======")]
    public float speed = 0.8f;
    public int maxHealth = 10;
    public int currentHealth;

    [Header("====== Attack ======")]
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private int damagePerHit = 20;
    [SerializeField] private float attackInterval = 1.0f;


    [Header("====== Burst walk duration ======")]
    public float fastDuration = 0.8f;
    public float slowDuration = 1.8f;
    public float fastMultiplier = 1.0f;
    public float slowMultiplier = 0.2f;


    private bool isDead = false;
    private bool isAttacking = false;
    private float currentMultiplier = 1f;

    private GameObject targetPlant;
    private Coroutine movePatternRoutine;
    private Animator animator;
    public void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        movePatternRoutine = StartCoroutine(BurstWalkPattern());

        animator.SetBool("isWalking", true);
    }


    private void Update()
    {
        if (isDead) return;

        if (isAttacking && targetPlant == null)
        {
            EndAttack();
        }

        if (!isAttacking)
        {
            animator.SetBool("isWalking", true);
            transform.Translate(-Vector3.right * (speed * currentMultiplier) * Time.deltaTime);
            DetectPlant();
        }


    }

    private IEnumerator BurstWalkPattern()
    {
        while (!isDead)
        {
            //fast
            currentMultiplier = fastMultiplier;
            float t = 0f;

            while (t < fastDuration && !isDead)
            {
                t += Time.deltaTime;
                yield return null;
            }

            //slow
            currentMultiplier = slowMultiplier;
            t = 0f;
            while (t < slowDuration && !isDead)
            {
                t += Time.deltaTime;
                yield return null;
            }
        }
    }

    private void DetectPlant()
    {
        if (isAttacking) return;

        if (Physics.Raycast(transform.position, -Vector3.right, out RaycastHit hit, attackRange))
        {
            if (hit.collider.CompareTag("Plant") && hit.collider.isTrigger == false)
            {
                targetPlant = hit.collider.gameObject;
                StartCoroutine(AttackPlant(targetPlant));
            }
        }
    }

    IEnumerator AttackPlant(GameObject plant)
    {
        isAttacking = true;
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", true);

        Plant plantHealth = plant.GetComponent<Plant>();
        if (plantHealth == null)
        {
            EndAttack();
            yield break;
        }

        while (!isDead && plantHealth != null && !plantHealth.isDead)
        {
            plantHealth.TakeDamage(damagePerHit);
            yield return new WaitForSeconds(attackInterval);
        }

        EndAttack();
    }

    private void EndAttack()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", false);
        animator.SetBool("isWalking", true);
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        Debug.Log("Zombie HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetBool("isDead", true);
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);

        if (movePatternRoutine != null) StopCoroutine(movePatternRoutine);
        StopAllCoroutines();

        Destroy(gameObject, 1f);
    }
}
