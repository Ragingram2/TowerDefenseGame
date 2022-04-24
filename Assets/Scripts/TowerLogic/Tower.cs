using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [HideInInspector] public TowerPreset preset;
    public int towerId;
    private TowerLogic tower;
    private GameObject self;

    public bool isUpgraded => transform.parent.tag.Equals("Upgraded");

    public void Start()
    {
        Init();
    }

    public void Init()
    {
        if (!preset.towerBasicModel)
            return;


        self = Instantiate(preset.towerBasicModel, transform, false);

        switch (preset.towerType)
        {
            case TowerType.Normal:
                tower = gameObject.AddComponent<NormalTower>();
                break;
            case TowerType.AOE:
                tower = gameObject.AddComponent<AOETower>();
                break;
            case TowerType.Debuff:
                tower = gameObject.AddComponent<DebuffTower>();
                break;
        }
    }

    public void Fire()
    {
        tower.Fire();
    }

    public void Upgrade()
    {
        if (self)
            Destroy(self);

        self = Instantiate(preset.towerUpgradedModel, transform, false);
    }
}
