using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerSelector : MonoBehaviour
{
    [SerializeField] private TowerType type;

    private Button btn;

    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(SetTowerType);
    }

    public void Update()
    {
        btn.interactable = !GameManager.IsGameMode(GameMode.WaveInProgress) ? GameManager.HasEnoughMoney(type) : false;
        if (transform.Find("Cost"))
        {
            if (GameManager.IsBuildMode(BuildMode.Upgrade))
                transform.Find("Cost").GetComponent<TMP_Text>().text = $"$-{ GameManager.GetTowerUpgradeCost(type)}";
            else
                transform.Find("Cost").GetComponent<TMP_Text>().text = $"$-{ GameManager.GetTowerCost(type)}";
        }
    }

    public void SetTowerType()
    {
        GameManager.TowerType = type;
    }
}
