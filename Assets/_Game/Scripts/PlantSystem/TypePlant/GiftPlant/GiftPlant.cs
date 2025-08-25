using System.Collections;
using DG.Tweening;
using UnityEngine;

public class GiftPlant : Plant {
    public bool bool_currentContainPlant = false; // Kiểm tra cây đã chứa zombie chưa
    private ZombieController enemyAttack;
    public Transform boxPoint; // Điểm "hộp" để zombie chui vào
    private bool doneEat = false;
    public override void Attack() {
        if (enemyAttack == null) return;
        StartCoroutine(CaptureZombie(enemyAttack));
    }

    private void OnTriggerEnter(Collider other) {
        if (!bool_currentContainPlant && other.CompareTag("Enemy")) {
            ZombieController zombie = other.GetComponent<ZombieController>();
            if (zombie != null) {
                enemyAttack = zombie;
                bool_currentContainPlant = true;
                Attack();
            }
        }
    }

    private IEnumerator CaptureZombie(ZombieController zombie) {
        // Lưu vị trí ban đầu của zombie
        Vector3 originalPos = zombie.transform.position;

        // Dừng mọi hoạt động của zombie
        zombie.enabled = false;

        // Đường dẫn để hút vào hộp
        Vector3[] path = new Vector3[] {
            zombie.transform.position,
            transform.position + Vector3.up * 3f,
            boxPoint.position
        };

        // Hút zombie vào hộp
        float duration = 1f;
        zombie.transform.DOPath(path, duration, PathType.CatmullRom)
            .SetEase(Ease.InOutSine);
        Vector3 scale_zombie = zombie.transform.localScale;
        zombie.transform.DOScale(scale_zombie * 0.5f, 1f);
        yield return new WaitForSeconds(duration + 0.1f);

        // Ẩn zombie trong 10 giây
        zombie.gameObject.SetActive(false);
        yield return new WaitForSeconds(10f);
        doneEat = true;
        ReturnZombie(zombie, originalPos, scale_zombie);

        // Xóa cây
        Destroy(gameObject);
    }

    private static void ReturnZombie(ZombieController zombie, Vector3 originalPos, Vector3 scale_zombie) {
        zombie.transform.localScale = scale_zombie;
        // Nhả zombie ra lại
        zombie.gameObject.SetActive(true);
        zombie.transform.position = originalPos;

        // Cho zombie hoạt động trở lại
        zombie.enabled = true;
    }
}
