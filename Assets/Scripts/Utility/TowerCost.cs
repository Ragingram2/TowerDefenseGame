using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerCost : MonoBehaviour
{
    private Tower tower;
    void Update()
    {
        if (GameManager.TowerType != TowerType.None)
        {
            if (GameManager.IsBuildMode(BuildMode.Place))
            {
                GetComponent<TMP_Text>().text = $"$-{ GameManager.GetTowerCost(GameManager.TowerType)}";
                return;
            }
        }

        tower = transform.parent.parent.parent.GetComponentInChildren<Tower>();
        if (tower && tower.preset.towerType != TowerType.None)
        {
            if (GameManager.IsBuildMode(BuildMode.Upgrade))
            {
                GetComponent<TMP_Text>().text = $"$-{ GameManager.GetTowerUpgradeCost(tower.preset.towerType)}";
            }
            else if (GameManager.IsBuildMode(BuildMode.Sell))
            {
                if (tower.isUpgraded)
                    GetComponent<TMP_Text>().text = $"+{ GameManager.GetTowerUpgradeSellPrice(tower.preset.towerType)}";
                else
                    GetComponent<TMP_Text>().text = $"+{ GameManager.GetTowerSellPrice(tower.preset.towerType)}";
            }
        }
    }
}
