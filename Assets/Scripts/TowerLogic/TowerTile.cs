using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTile : MonoBehaviour
{
    private Material m_Material;
    [SerializeField] private GameObject sellableHint;
    [SerializeField] private Material buildableMat;
    [SerializeField] private Material sellableMat;
    [SerializeField] private Material upgradeableMat;

    private GameObject selection;
    private GameObject highlighter;
    private TowerType type;

    private void Start()
    {
        highlighter = GameObject.CreatePrimitive(PrimitiveType.Cube);
        highlighter.transform.parent = gameObject.transform;
        highlighter.transform.localPosition = new Vector3(0.0f, .15f, 0.0f);
        highlighter.GetComponent<BoxCollider>().enabled = false;
        highlighter.transform.localScale = new Vector3(1.0f, .2f, 1.0f);
        m_Material = highlighter.GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        if (!highlighter)
            return;

        if (CanBuild())
        {
            highlighter.GetComponent<MeshRenderer>().material = buildableMat;
            highlighter.SetActive(true);
        }
        else if (CanSell())
        {
            highlighter.GetComponent<MeshRenderer>().material = sellableMat;
            highlighter.SetActive(true);
        }
        else if (CanUpgrade())
        {
            highlighter.GetComponent<MeshRenderer>().material = upgradeableMat;
            highlighter.SetActive(true);
        }
        else if (GameManager.IsBuildMode(BuildMode.None))
        {
            highlighter.GetComponent<MeshRenderer>().material = m_Material;
            highlighter.SetActive(false);
        }
    }

    private void OnMouseEnter()
    {
        if (!selection && !Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (CanBuild())
                ShowBuildHint();
            else if (CanUpgrade())
                ShowUpgradeHint();
            else if (CanSell())
                ShowSellHint();
        }
    }

    void OnMouseOver()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (CanBuild())
                Build();
            else if (CanUpgrade())
                Upgrade();
            else if (CanSell())
                Sell();

            GameManager.BuildMode = BuildMode.None;
            GameManager.TowerType = TowerType.None;
        }
    }

    void OnMouseExit()
    {
        if (selection)
            Destroy(selection);
    }

    void ShowBuildHint()
    {
        if (GameManager.IsSelectedTowerType(TowerType.None))
            return;

        var hintModel = GameManager.towerPresets[GameManager.TowerType].towerPrefab.GetComponent<Tower>().preset.towerBasicHint;
        selection = Instantiate(hintModel, transform, false);
    }

    void Build()
    {
        if (GameManager.IsSelectedTowerType(TowerType.None))
            return;

        if (GameManager.HasEnoughMoney(GameManager.TowerType))
        {
            type = GameManager.TowerType;
            GameManager.BuyTower(transform, type);
            gameObject.tag = "Upgradable";
        }
        else
        {
            Debug.LogError("Insufficient Funds");
        }

        GameManager.TowerType = TowerType.None;

        if (selection)
            Destroy(selection);
    }

    void ShowUpgradeHint()
    {
        if (IsThisTowerType(TowerType.None))
            return;

        var hintModel = GameManager.towerPresets[type].towerPrefab.GetComponent<Tower>().preset.towerUpgradedHint;
        selection = Instantiate(hintModel, transform, false);
    }

    void Upgrade()
    {
        if (GameManager.HasEnoughMoney(type))
        {
            GameManager.UpgradeTower(GetComponentInChildren<Tower>().towerId);
            gameObject.tag = "Upgraded";
        }
        else
        {
            Debug.LogError("Insufficient Funds");
        }

        GameManager.TowerType = TowerType.None;

        if (selection)
            Destroy(selection);
    }

    void ShowSellHint()
    {
        if (IsThisTowerType(TowerType.None))
            return;

        selection = Instantiate(sellableHint, transform, false);
    }

    void Sell()
    {
        GameManager.SellTower(GetComponentInChildren<Tower>().towerId);
        gameObject.tag = "Buildable";
        type = TowerType.None;

        if (selection)
            Destroy(selection);
    }

    bool IsThisTowerType(TowerType type) => type == this.type;

    bool CanBuild()
    {
        return GameManager.IsBuildMode(BuildMode.Place) && gameObject.tag.Equals("Buildable");
    }
    bool CanUpgrade()
    {
        return GameManager.IsBuildMode(BuildMode.Upgrade) && gameObject.tag.Equals("Upgradable");
    }
    bool CanSell()
    {
        return GameManager.IsBuildMode(BuildMode.Sell) && !gameObject.tag.Equals("Buildable");
    }
}
