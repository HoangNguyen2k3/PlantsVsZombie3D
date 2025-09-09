using DG.Tweening;
using Layer_lab._3D_Casual_Character.Demo2;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlayManager : MonoBehaviour {
    public static GamePlayManager Ins;

    public int currentSun = 0;
    public TextMeshProUGUI text_numSun;
    public GridManager gridManager;
    public PlantCard selectedPlantCard;
    public LayerMask gridCellLayer; //Layer của grid cell
    public LayerMask enablePlantLayer;
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

    [Header("------------Using Rake-----------")]
    public GameObject holdPlant;
    public bool using_rake = false;
    public Button btn_rake;
    public GameObject obj_plantCardChoice;

    float deltaTime = 0;
    public TextMeshProUGUI fpsText;

    public GameObject fakePlant;
    private GridCell gridcell1;

    [Header("-----------SpawnCharacter------------")]
    [SerializeField] protected Demo2Character characterPrefab;
    [SerializeField] protected Transform spawnPoint;
     protected Demo2Character playerCharacter;
    private void Awake() {
        Ins = this;
    }
    private void Start() {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        numEnemyCurrentInMap = numSpawnMax;
        text_numSun.text = currentSun.ToString();
        selectedPlantCard = null;
    }
    void Update() {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString() + " FPS";
        if (isEndGame) { return; }
        time += Time.deltaTime;
        if (startSpawn == false && time > timeBeforeSpawnZombie) {
            startSpawn = true;
            StartCoroutine(SpawnEnemy());
        }
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            if (IsPointerOverUI()) return;
            HandleTouch();
        }
#else
    if (Input.GetMouseButtonDown(0))
    {
        if (IsPointerOverUI()) return;
        HandleTouch();
    }
#endif
        /*        if (Input.GetMouseButtonDown(0)) {
                    if (IsPointerOverUI()) {

                    }
                    else {
                        HandleTouch();
                    }
                }*/
        if (numEnemyCurrentInMap == 0) {
            WinningGame();
            isEndGame = true;
        }
    }

    private void HandleTouch() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit)) {
            GridCell gridcell = hit.collider.GetComponentInParent<GridCell>();
            if (gridcell != null && selectedPlantCard != null && selectedPlantCard.isCoolDown == false) {
                if (!gridcell.isOccupied && currentSun >= selectedPlantCard.price) {
                    fakePlant.SetActive(true);
                    fakePlant.transform.position = gridcell.transform.position + new Vector3(0, 0.5f, 0);
                    gridcell1 = gridcell;
                    /*                            Plant temp = Instantiate(selectedPlantCard.plantType, gridcell.transform.position, selectedPlantCard.plantType.transform.rotation);
                                                ChangeNumSun(-selectedPlantCard.price);

                                                gridcell.currentPlant = temp;
                                                temp.gameObject.layer = gridCellLayer;
                                                temp.transform.parent = gridcell.transform;

                                                selectedPlantCard.Cooldown();
                                                selectedPlantCard = null;
                                                InactiveAllCurrentPlant();
                                                Destroy(holdPlant);*/
                }
                else if (using_rake == false && gridcell.isOccupied && currentSun >= selectedPlantCard.price) {
                    ProcessMergePlant(selectedPlantCard.plantType.typePlant, gridcell.currentPlant.typePlant, gridcell);
                }
                else if (using_rake == true && gridcell.isOccupied) {
                    TakeHoldPlant(gridcell.currentPlant.typePlant);
                    selectedPlantCard = GetPlantCardMapping(gridcell.currentPlant.typePlant);
                    Destroy(gridcell.currentPlant);
                    gridcell.currentPlant = null;
                }
            }
            else if (gridcell != null && using_rake == true && gridcell.isOccupied) {
                TakeHoldPlant(gridcell.currentPlant.typePlant);
                ClickUsingRake();
                selectedPlantCard = GetPlantCardMapping(gridcell.currentPlant.typePlant);
                Destroy(gridcell.currentPlant.gameObject);
                gridcell.currentPlant = null;
            }
        }
    }

    public void PlantReal() {
        if (selectedPlantCard == null || gridcell1 == null) { return; }
        fakePlant.SetActive(false);
        Plant temp = Instantiate(selectedPlantCard.plantType, gridcell1.transform.position, selectedPlantCard.plantType.transform.rotation);
        ChangeNumSun(-selectedPlantCard.price);

        gridcell1.currentPlant = temp;
        temp.gameObject.layer = gridCellLayer;
        temp.transform.parent = gridcell1.transform;

        selectedPlantCard.Cooldown();
        selectedPlantCard = null;
        InactiveAllCurrentPlant();
        Destroy(holdPlant);
    }
    /*    bool IsPointerOverUI() {
    #if UNITY_ANDROID || UNITY_IOS
            if (Input.touchCount > 0)
                return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
            else
                return false;
    #else
        return EventSystem.current.IsPointerOverGameObject();
    #endif
        }*/
    bool IsPointerOverUI() {
        PointerEventData eventData = new PointerEventData(EventSystem.current);

#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
            eventData.position = Input.GetTouch(0).position;
        else
            return false;
#else
    eventData.position = Input.mousePosition;
#endif

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        for (int i = results.Count - 1; i >= 0; i--) {
            if (results[i].gameObject.layer == 6) {
                results.RemoveAt(i);
            }
        }
        if (results.Count > 0) {
            Debug.Log("UI Raycast hits:");
            foreach (var result in results) {
                Debug.Log(" - " + result.gameObject.name);
            }
            return true;
        }

        return false;
    }
    public void ChangeCurrentPlant(PlantCard plant) {
        selectedPlantCard = plant;
        if (holdPlant != null) {
            Destroy(holdPlant.gameObject);
        }
        TakeHoldPlant(plant.plantType.typePlant);
    }
    public void TakeHoldPlant(TypePlant type) {
        GameObject temp = GetPlantHoldMapping(type);
        holdPlant = Instantiate(temp, posHoldPlant);
        holdPlant.transform.localPosition = Vector3.zero;
        holdPlant.transform.localScale /= 3;
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
        bool merge_success = true;
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
        else if ((handPlant == TypePlant.CherryBomb && groundPlant == TypePlant.WallNut)
    || (handPlant == TypePlant.WallNut && groundPlant == TypePlant.CherryBomb)) {
            SpawnMergePlant(gridCell, TypePlant.CherryNut);
        }
        else {
            merge_success = false;
            Color c = textAnnouce.color;
            c.a = 0;
            textAnnouce.color = c;

            textAnnouce.DOFade(1, 0.05f).OnComplete(() => {
                textAnnouce.DOFade(0, 0.5f);
            });
        }
        if (merge_success) {
            selectedPlantCard.Cooldown();
            selectedPlantCard = null;
            InactiveAllCurrentPlant();
            Destroy(holdPlant);
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
    public GameObject GetPlantHoldMapping(TypePlant typePlant) {
        foreach (var item in list_PlantMapping) {
            if (item.type == typePlant) {
                return item.plantHold;
            }
        }
        return null;
    }
    public PlantCard GetPlantCardMapping(TypePlant typePlant) {
        foreach (var item in list_PlantMapping) {
            if (item.type == typePlant) {
                return item.plantCard;
            }
        }
        return null;
    }
    public void ClickUsingRake() {
        if (selectedPlantCard != null) {
            return;
        }
        using_rake = !using_rake;
        if (using_rake) {
            selectedPlantCard = null;
            InactiveAllCurrentPlant();
            obj_plantCardChoice.gameObject.SetActive(false);
            btn_rake.GetComponent<Image>().color = Color.green;
            Destroy(holdPlant);
        }
        else {
            obj_plantCardChoice.gameObject.SetActive(true);
            btn_rake.GetComponent<Image>().color = Color.white;
        }
    }
    //public virtual void SpawnCharacter()
    //{
    //    // Nếu chưa có dữ liệu chọn nhân vật thì spawn mặc định
    //    if (CharacterDataHolder.Instance == null || CharacterDataHolder.Instance.SelectedCharacterData == null)
    //    {
    //        Debug.LogWarning("Không có dữ liệu nhân vật, spawn mặc định.");
    //        playerCharacter = Instantiate(characterPrefab, spawnPoint.position, Quaternion.identity);
    //        playerCharacter.Init();
    //        return;
    //    }

    //    // Spawn nhân vật đã chọn
    //    playerCharacter = Instantiate(characterPrefab, spawnPoint.position, Quaternion.identity);
    //    playerCharacter.Init();

    //    // Apply dữ liệu part từ CharacterDataHolder
    //    foreach (var kvp in CharacterDataHolder.Instance.SelectedCharacterData)
    //    {
    //        var part = playerCharacter.CurrentCharacterPartByType(kvp.Key);
    //        if (part != null)
    //        {
    //            part.SetPartByIndex(kvp.Value);
    //        }
    //    }
    //}
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
    SunBomb,
    GiftPlant,
    SnowPea,
    CherryNut
}
[System.Serializable]
public class PlantMapping {
    public TypePlant type;
    public Plant prefab;
    public GameObject plantHold;
    public PlantCard plantCard;
}

