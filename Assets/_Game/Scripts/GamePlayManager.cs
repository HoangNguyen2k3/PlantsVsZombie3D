using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

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

    private float time = 0;
    public Transform posHoldPlant;
    private void Awake() {
        Ins = this;
    }
    private void Start() {
        text_numSun.text = currentSun.ToString();
    }
    void Update() {
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
                    if (gridcell != null && selectedPlantCard != null) {
                        if (!gridcell.isOccupied && currentSun >= selectedPlantCard.price) {
                            Instantiate(selectedPlantCard.plantType, gridcell.transform.position, selectedPlantCard.plantType.transform.rotation);
                            ChangeNumSun(-selectedPlantCard.price);
                        }
                    }
                }

            }
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
}
/*[System.Serializable]
public enum TypePlant {
    SunFlower,
    PeaShoot,
    PotatoMine,
    WallNut
}*/
