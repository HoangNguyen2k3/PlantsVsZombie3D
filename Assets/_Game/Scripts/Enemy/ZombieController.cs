using System.Collections;
using UnityEngine;

public class ZombieController : MonoBehaviour {

    public enum StateZombie
    {
        Walk,
        Attack,
        Dead
    }

    [Header("====== Base ======")]
    public float speed = 1f;
    public int maxHealth = 10;
    public int currentHealth;

    [Header("====== Attack ======")]
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private int damagePerHit = 20;
    [SerializeField] private float attackInterval = 1.0f;

    [Header("====== Effect ======")]       
    public GameObject slowEffect;
    
    private bool isDead = false;
    private bool isAttacking = false;

    private float currentMultiplier = 1f;

    private GameObject targetPlant;
    public Animator animator;

    private float halfSpeed = 0f;

    private StateZombie currentState;
    

    public void Start() {
        animator = GetComponentInChildren<Animator>();
        currentHealth = maxHealth;
        halfSpeed = speed / 2;

        //animator.SetBool("isWalking", true);

        ChangeState(StateZombie.Walk);
    }


    private void Update() {
        if (currentState == StateZombie.Dead) return;

        if (isAttacking && targetPlant == null) {
            EndAttack();
        }

        switch (currentState)
        {
            case StateZombie.Walk:
                WalkBehavior();
                break;
            case StateZombie.Attack:
                break;
            case StateZombie.Dead:
                break;
        }
    }

    private void WalkBehavior()
    {
        animator.SetBool("isWalking", true);
        transform.Translate(-Vector3.right * (speed * currentMultiplier) * Time.deltaTime);
        DetectPlant();
    }

    public void ChangeState(StateZombie state)
    {
        currentState = state;
    }

    private void DetectPlant() {
        if (isAttacking) return;

        if (Physics.Raycast(transform.position, -Vector3.right, out RaycastHit hit, attackRange)) {
            if (hit.collider.CompareTag("Plant") && hit.collider.isTrigger == false) {
                targetPlant = hit.collider.gameObject;
                ChangeState(StateZombie.Attack);
                StartCoroutine(AttackPlant(targetPlant));
            }
        }
    }

    IEnumerator AttackPlant(GameObject plant) {
        isAttacking = true;
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", true);

        Plant plantHealth = plant.GetComponent<Plant>();
        if (plantHealth == null) {
            EndAttack();
            yield break;
        }

        while (!isDead && plantHealth != null && !plantHealth.isDead) {
            plantHealth.TakeDamage(damagePerHit);
            yield return new WaitForSeconds(attackInterval);
        }

        EndAttack();
    }

    private void EndAttack() {

        if (isDead) return;
        isAttacking = false;
        animator.SetBool("isAttacking", false);
        animator.SetBool("isWalking", true);
        ChangeState(StateZombie.Walk);  
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
        animator.SetBool("isDead", true);
        ChangeState(StateZombie.Dead);
        
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);

        StopAllCoroutines();

        Destroy(gameObject, 1f);
    }
    private Coroutine slowRoutine;

    public void ApplySlow() {
        if (slowRoutine != null) StopCoroutine(slowRoutine);
        slowRoutine = StartCoroutine(EffectSlowEnemy());
    }
    public IEnumerator EffectSlowEnemy() {
        speed = halfSpeed;
        animator.speed = 0.5f;
        slowEffect.SetActive(true);
        
        yield return new WaitForSeconds(3f);
        speed = halfSpeed * 2;
        
        slowEffect.SetActive(false);
    }

    private void OnDestroy() {
        GamePlayManager.Ins.numEnemyCurrentInMap--;
    }

    //private void OnTriggerEnter(Collider other) {
    //    if (other.CompareTag("EnterGarden")) {
    //        transform.position -= new Vector3(0, 0.34f, 0);
    //    }
    //}
}
