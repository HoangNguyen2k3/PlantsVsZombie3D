using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ZombieSpawnData", menuName = "Game/Zombie Spawn Data")]
public class ZombieSpawnData : ScriptableObject {
    public List<ZombiePhaseData> phases = new();
}

[System.Serializable]
public class ZombiePhaseData {
    public string phaseName = "Phase 1";

    [Tooltip("Thời gian chờ trước khi phase này bắt đầu (tính từ khi phase trước kết thúc)")]
    public float delayBeforeStart = 3f;

    [Tooltip("Danh sách zombie sẽ spawn trong phase này")]
    public List<ZombieSpawnInfo> zombieSpawns = new();

    [Tooltip("Nếu có boss trong phase này")]
    public bool hasBoss = false;

    [Tooltip("Prefab boss (chỉ nếu hasBoss = true)")]
    public GameObject bossPrefab;

    [Tooltip("Vị trí spawn boss (null thì random)")]
    public Transform bossSpawnPoint;
}

[System.Serializable]
public class ZombieSpawnInfo {
    public GameObject zombiePrefab;

    [Tooltip("Số lượng zombie spawn ra trong phase này")]
    public int count = 5;

    [Tooltip("Khoảng cách giữa 2 lần spawn zombie (giây)")]
    public float interval = 1.5f;

    [Tooltip("Vị trí spawn (null = random trong list_posSpawn của GamePlayManager)")]
    public Transform spawnPoint;
}
