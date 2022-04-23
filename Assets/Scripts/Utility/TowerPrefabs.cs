using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct prefabKVP
{
    public TowerType type;
    public TowerPreset preset;
}

[CreateAssetMenu(fileName = "TowerPrefabs", menuName = "Tower/PrefabList", order = 2)]
public class TowerPrefabs : ScriptableObject
{
    public prefabKVP[] presets;
}
