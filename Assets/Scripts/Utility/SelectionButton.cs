using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionButton : MonoBehaviour
{
    [SerializeField] private TowerType type;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(SetTowerType);
    }

    public void SetTowerType()
    {
        GameManager.TowerType = type;
    }

}
