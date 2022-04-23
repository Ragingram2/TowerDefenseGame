using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "TowerPreset", menuName = "Tower/TowerPreset", order = 1)]
public class TowerPreset : ScriptableObject
{
    public GameObject towerPrefab;
    public GameObject towerBasicHint;
    public GameObject towerBasicModel;
    public GameObject towerUpgradedHint;
    public GameObject towerUpgradedModel;
    public TowerType towerType;
    public int baseCost;
    public int upgradeCost;
    public int baseSellPrice;
    public int upgradeSellPrice;
    public int baseRadius;
    public int upgradeRadius;
    public float fireInterval;
    public int baseDamage;
    public int upgradeDamage;
    public int baseDamageRadius;
    public int upgradeDamageRadius;
    public float baseDamageModifier;
    public float upgradeDamageModifier;
    public int baseDuration;
    public int upgradeDuration;
    public int baseEffectModifier;
    public int upgradeEffectModifier;
}






