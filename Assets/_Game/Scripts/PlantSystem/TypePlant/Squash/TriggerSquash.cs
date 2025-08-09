using UnityEngine;

public class TriggerSquash : MonoBehaviour {
    public Squash squash;
    private bool firstTime = false;
    private void OnTriggerEnter(Collider other) {
        if (firstTime) { return; }
        if (other.CompareTag("Enemy")) {
            ZombieController zombie = other.GetComponent<ZombieController>();
            if (zombie != null && zombie.currentHealth > 0) {
                firstTime = true;
                squash.posAttack = new Vector3(zombie.transform.position.x, squash.transform.position.y, zombie.transform.position.z);
                squash.Attack();

            }
        }
    }
}
