using DG.Tweening;
using UnityEngine;

public class PlantCard : MonoBehaviour {
    public Plant plantType;
    public GameObject chooseGameObject;
    public int price = 0;
    public float float_timeCoolDown = 1f;
    public RectTransform trans_coolDown;
    public bool isCoolDown = false;
    [Header("Mystery Plant")]
    public Plant[] plantMysTypes;
    public void ChoosePlant() {
        if (plantMysTypes.Length > 0) {
            int rand = Random.Range(0, plantMysTypes.Length);
            plantType = plantMysTypes[rand];
            GamePlayManager.Ins.ChangeCurrentPlant(this);
        }
        else
            GamePlayManager.Ins.ChangeCurrentPlant(this);
        chooseGameObject.SetActive(true);
        foreach (var item in GamePlayManager.Ins.list_PlantCard) {
            if (item != this) {
                item.chooseGameObject.SetActive(false);
            }
        }
    }
    public void Cooldown() {
        isCoolDown = true;
        Vector2 offsetMin = trans_coolDown.offsetMin;
        offsetMin.y = 0;
        trans_coolDown.offsetMin = offsetMin;
        // Lấy giá trị bottom hiện tại
        float startBottom = trans_coolDown.offsetMin.y;

        // Tween từ startBottom -> targetBottom
        DOTween.To(
            () => startBottom,
            value => {
                startBottom = value;
                Vector2 offsetMin = trans_coolDown.offsetMin;
                offsetMin.y = value;
                trans_coolDown.offsetMin = offsetMin;
            },
            225,
            float_timeCoolDown
        ).SetEase(Ease.Linear).OnComplete(() => {
            isCoolDown = false;
        });
    }
}
