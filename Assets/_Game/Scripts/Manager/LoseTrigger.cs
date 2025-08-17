using UnityEngine;

public class LoseTrigger : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy") && !GamePlayManager.Ins.isEndGame) {
            GamePlayManager.Ins.LosingGame();
        }
    }
}
