using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameMode
{
    Build,
    StartOfWave,
    WaveInProgress,
    EndOfWave
}

public enum BuildMode
{
    None,
    Sell,
    Upgrade,
    Place
}

public enum TowerType
{
    Normal,
    AOE,
    Debuff,
    None
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private TowerPrefabs towerPresetSO;

    public delegate void GameModeChangeHandler();
    public static event GameModeChangeHandler OnWaveStart;
    public static event GameModeChangeHandler OnWaveEnd;

    private static GameMode currentGameMode = GameMode.Build;
    public static GameMode GameMode { get { return currentGameMode; } set { currentGameMode = value; } }
    private static BuildMode currentBuildMode = BuildMode.None;
    public static BuildMode BuildMode { get { return currentBuildMode; } set { currentBuildMode = value; } }
    private static TowerType towerTypeSelected = TowerType.None;
    public static TowerType TowerType { get { return towerTypeSelected; } set { towerTypeSelected = value; } }
    private static int money = 200;
    public static int Money { get { return money; } set { money = value; } }
    private static int waveNumber = 0;
    public static int WaveNumber { get { return waveNumber; } }

    public static Dictionary<TowerType, TowerPreset> towerPresets = new Dictionary<TowerType, TowerPreset>();
    public static Dictionary<float, List<Action>> processes = new Dictionary<float, List<Action>>();
    private static Dictionary<int, Tower> towers = new Dictionary<int, Tower>();

    private static int spawnerCount;
    private static int finishedSpawners = 0;

    public static bool waveUpdating = false;
    private static bool waveStart = false;

    private float waveTime = 0.0f;

    private static int towerId = -1;

    #region Initialization
    void Start()
    {
        EnemySpawner.OnStoppedSpawning += SpawnerStopped;
        spawnerCount = FindObjectsOfType<EnemySpawner>().Length;
        foreach (prefabKVP kvp in towerPresetSO.presets)
        {
            towerPresets[kvp.type] = kvp.preset;
        }
    }
    #endregion

    #region Updates
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1))
            SetBuildMode(BuildMode.None);
        //FSM
        switch (currentGameMode)
        {
            case GameMode.Build:
                break;
            case GameMode.StartOfWave:
                OnWaveStart();
                waveNumber++;
                SetGameMode(GameMode.WaveInProgress);
                waveStart = true;
                break;
            case GameMode.WaveInProgress:
                waveUpdating = true;
                WaveUpdate();
                break;
            case GameMode.EndOfWave:
                waveTime = 0.0f;
                OnWaveEnd();
                SetGameMode(GameMode.Build);
                break;
        }
    }

    public void WaveUpdate()
    {
        //Observer
        if (waveStart)
        {
            waveStart = false;
            foreach (float timeBuffer in processes.Keys)
            {
                foreach (Action fire in processes[timeBuffer])
                {
                    StartCoroutine(Invoke(timeBuffer, fire));
                }
            }
        }

        if (finishedSpawners >= spawnerCount && FindObjectsOfType<EnemyLogic>().Length == 0)
        {
            SetGameMode(GameMode.EndOfWave);
            waveUpdating = false;
        }
    }

    public static IEnumerator Invoke(float interval, Action action)
    {
        while (waveUpdating)
        {
            yield return new WaitForSeconds(interval);
            action();
        }
    }
    #endregion

    #region Functions
    public static GameObject BuyTower(Transform parent, TowerType type)
    {
        money -= GetTowerCost(type);
        var go = TowerFactory.CreateTower(parent, towerPresets[type]);
        var tower = go.GetComponent<Tower>();
        RegisterTower(tower);
        return go;
    }
    public static Tower UpgradeTower(int id)
    {
        money -= GetTowerUpgradeCost(towers[id].preset.towerType);
        towers[id].Upgrade();
        return towers[id];
    }
    public static void SellTower(int id)
    {
        if (towers[id].isUpgraded)
            money += GetTowerUpgradeSellPrice(towers[id].preset.towerType);
        else
            money += GetTowerSellPrice(towers[id].preset.towerType);

        var go = towers[id].gameObject;
        UnregisterTower(id);
        Destroy(go);
    }
    public static Tower GetTower(int id) => towers.ContainsKey(id) ? towers[id] : null;
    //Observer
    public static void RegisterTower(Tower tower)
    {
        if (!processes.ContainsKey(towerPresets[tower.preset.towerType].fireInterval))
            processes.Add(towerPresets[tower.preset.towerType].fireInterval, new List<Action>());
        processes[towerPresets[tower.preset.towerType].fireInterval].Add(tower.Fire);
        towers.Add(++towerId, tower);
        tower.towerId = towerId;
    }
    //Observer
    public static void UnregisterTower(int id)
    {
        if (processes.ContainsKey(towerPresets[towers[id].preset.towerType].fireInterval))
        {
            processes[towerPresets[towers[id].preset.towerType].fireInterval].Remove(towers[id].Fire);
            towers.Remove(id);
        }
    }

    public static void SetGameMode(GameMode mode)
    {
        currentGameMode = mode;
        Debug.Log($"GameMode Set: {mode.ToString()}");
    }
    public static void SetBuildMode(BuildMode mode)
    {
        currentBuildMode = mode;
        Debug.Log($"BuildMode Set: {mode.ToString()}");
    }

    public static bool IsGameMode(GameMode mode) => currentGameMode == mode;
    public static bool IsBuildMode(BuildMode mode) => currentBuildMode == mode && IsGameMode(GameMode.Build);
    public static bool IsSelectedTowerType(TowerType type) => towerTypeSelected == type && IsGameMode(GameMode.Build);
    public static bool HasEnoughMoney(TowerType type) => towerPresets.ContainsKey(type) ? towerPresets[type].baseCost <= money : false;
    public static int GetTowerCost(TowerType type) => towerPresets.ContainsKey(type) ? towerPresets[type].baseCost : -1;
    public static int GetTowerUpgradeCost(TowerType type) => towerPresets.ContainsKey(type) ? towerPresets[type].upgradeCost : -1;
    public static int GetTowerSellPrice(TowerType type) => towerPresets.ContainsKey(type) ? towerPresets[type].baseSellPrice : -1;
    public static int GetTowerUpgradeSellPrice(TowerType type) => towerPresets.ContainsKey(type) ? towerPresets[type].upgradeSellPrice : -1;
    public static int GetCurrentTowerID() => towerId;
    public static void SpawnerStopped() => finishedSpawners++;
    public static void AddFunds(int amount) => money += amount;
    public static void EndGame() => SceneManager.LoadScene("Game Over");
    #endregion
}



