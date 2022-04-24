using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerLogic : MonoBehaviour
{
    protected List<GameObject> targets = new List<GameObject>();

    protected Tower tower;
    protected TowerPreset preset => tower.preset;

    void Start() => tower = GetComponent<Tower>();

    public abstract void UpdateTarget();
    public abstract void Fire();
}
