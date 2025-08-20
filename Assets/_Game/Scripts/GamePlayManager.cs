using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GamePlayManager : MonoBehaviour {
    public static GamePlayManager Ins;

    public int currentSun = 0;
    public TextMeshProUGUI text_numSun;
    public GridManager gridManager;
    public PlantCard selectedPlantCard;
    public LayerMask gridCellLayer; //Layer của grid cell
    public List<PlantCard> list_PlantCard = new();
    [Header("-----------Spawn Zombie------------")]
    public float timeBeforeSpawnZombie = 10f;
    public List<Transform> list_posSpawn = new();
    public List<GameObject> zombies = new();
    public bool startSpawn = false;
    public float timeBetweenSpawn = 5f;
    public int numSpawnMax = 10;
    public int numEnemyCurrentInMap = 10;

    private float time = 0;
    public Transform posHoldPlant;

    public GameObject winningGameUI;
    public GameObject losingGameUI;
    public bool isEndGame = false;

    public TextMeshProUGUI textAnnouce;

    public List<PlantMapping> list_PlantMapping = new();
    private void Awake() {
        Ins = this;
    }
    private void Start() {
        numEnemyCurrentInMap = numSpawnMax;
        text_numSun.text = currentSun.ToString();
        selectedPlantCard = null;
    }
    void Update() {
        if (isEndGame) { return; }
        time += Time.deltaTime;
        if (startSpawn == false && time > timeBeforeSpawnZombie) {
            startSpawn = true;
            StartCoroutine(SpawnEnemy());
        }
        if (Input.GetMouseButtonDown(0)) {
            if (IsPointerOverUI()) {

            }
            else {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit)) {
                    GridCell gridcell = hit.collider.GetComponentInParent<GridCell>();
                    if (gridcell != null && selectedPlantCard != null && selectedPlantCard.isCoolDown == false) {
                        if (!gridcell.isOccupied && currentSun >= selectedPlantCard.price) {
                            Plant temp = Instantiate(selectedPlantCard.plantType, gridcell.transform.position, selectedPlantCard.plantType.transform.rotation);
                            ChangeNumSun(-selectedPlantCard.price);

                            gridcell.currentPlant = temp;
                            temp.gameObject.layer = gridCellLayer;
                            temp.transform.parent = gridcell.transform;

                            selectedPlantCard.Cooldown();
                            selectedPlantCard = null;
                            InactiveAllCurrentPlant();
                        }
                        else if (gridcell.isOccupied && currentSun >= selectedPlantCard.price) {
                            ProcessMergePlant(selectedPlantCard.plantType.typePlant, gridcell.currentPlant.typePlant, gridcell);
                        }
                    }
                }

            }
        }
        if (numEnemyCurrentInMap == 0) {
            WinningGame();
            isEndGame = true;
        }
    }
    bool IsPointerOverUI() {
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
            return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        else
            return false;
#else
    return EventSystem.current.IsPointerOverGameObject();
#endif
    }
    public void ChangeCurrentPlant(PlantCard plant) {
        selectedPlantCard = plant;
    }
    public void InactiveAllCurrentPlant() {
        foreach (var item in list_PlantCard) {
            item.chooseGameObject.SetActive(false);
        }
    }
    public void ChangeNumSun(int num) {
        currentSun += num;
        text_numSun.text = currentSun.ToString();
    }
    public IEnumerator SpawnEnemy() {
        while (numSpawnMax > 0) {
            numSpawnMax--;
            int ranpos = Random.Range(0, list_posSpawn.Count);
            Instantiate(zombies[Random.Range(0, zombies.Count)], list_posSpawn[ranpos].position, Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenSpawn);
        }
    }
    public void WinningGame() {
        isEndGame = true;
        winningGameUI.SetActive(true);
    }
    public void LosingGame() {
        isEndGame = true;
        losingGameUI.SetActive(true);
    }
    public void PlayAgain() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ProcessMergePlant(TypePlant handPlant, TypePlant groundPlant, GridCell gridCell) {
        if (handPlant == TypePlant.SunFlower && groundPlant == TypePlant.SunFlower) {
            SpawnMergePlant(gridCell, TypePlant.TwinSunFlower);
        }
        else if (handPlant == TypePlant.PeaShoot && groundPlant == TypePlant.PeaShoot) {
            SpawnMergePlant(gridCell, TypePlant.Repeter);
        }
        else if (handPlant == TypePlant.Repeter && groundPlant == TypePlant.Repeter) {
            SpawnMergePlant(gridCell, TypePlant.GatlingGun);
        }
        else if ((handPlant == TypePlant.PeaShoot && groundPlant == TypePlant.SunFlower)
            || (handPlant == TypePlant.SunFlower && groundPlant == TypePlant.PeaShoot)) {
            SpawnMergePlant(gridCell, TypePlant.SunGun);
        }
        else if ((handPlant == TypePlant.CherryBomb && groundPlant == TypePlant.SunFlower)
            || (handPlant == TypePlant.SunFlower && groundPlant == TypePlant.CherryBomb)) {
            SpawnMergePlant(gridCell, TypePlant.SunBomb);
        }
        else {
            Color c = textAnnouce.color;
            c.a = 0;
            textAnnouce.color = c;

            textAnnouce.DOFade(1, 0.05f).OnComplete(() => {
                textAnnouce.DOFade(0, 0.5f);
            });
        }
    }

    private void SpawnMergePlant(GridCell gridCell, TypePlant typePlant) {
        Plant plant = GetPlantMapping(typePlant);
        if (plant != null) {
            Destroy(gridCell.currentPlant.gameObject);
            gridCell.currentPlant = null;
            Plant temp = Instantiate(plant, gridCell.transform.position, plant.transform.rotation);
            temp.transform.parent = gridCell.transform;
            temp.gameObject.layer = gridCellLayer;
            gridCell.currentPlant = temp;
        }
        else {
            Debug.LogWarning("Lỗi rồi");
        }
    }

    public Plant GetPlantMapping(TypePlant typePlant) {
        foreach (var item in list_PlantMapping) {
            if (item.type == typePlant) {
                return item.prefab;
            }
        }
        return null;
    }

}

[System.Serializable]
public enum TypePlant {
    SunFlower,
    PeaShoot,
    PotatoMine,
    WallNut,
    CherryBomb,
    Squash,
    TwinSunFlower,
    Repeter,
    GatlingGun,
    SunGun,
    SunBomb
}
[System.Serializable]
public class PlantMapping {
    public TypePlant type;
    public Plant prefab;
}

