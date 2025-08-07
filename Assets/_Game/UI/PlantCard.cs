using UnityEngine;

public class PlantCard : MonoBehaviour {
    public Plant plantType;
    public GameObject chooseGameObject;
    public int price = 0;
    public void ChoosePlant() {
        GamePlayManager.Ins.ChangeCurrentPlant(this);
        chooseGameObject.SetActive(true);
        foreach (var item in GamePlayManager.Ins.list_PlantCard) {
            if (item != this) {
                item.chooseGameObject.SetActive(false);
            }
        }
    }
}
