using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffTower : TowerLogic
{
    private int radius;
    private int effectModifier;
    private float duration;

    private void Update()
    {
        GetComponent<SphereCollider>().radius = radius;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Enemy"))
        {
            targets.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Enemy") && targets.Contains(other.gameObject))
        {
            targets.Remove(other.gameObject);
        }
    }

    public override void Fire()
    {
        effectModifier = tower.isUpgraded ? preset.upgradeEffectModifier : preset.baseEffectModifier;
        duration = tower.isUpgraded ? preset.upgradeDuration : preset.baseDuration;

        foreach (GameObject go in targets)
        {
            if (!go)
                continue;

            if (!go.GetComponent<EnemyLogic>().isDebuffed)
                go.GetComponent<EnemyLogic>().ChangeSpeed(effectModifier, duration);
        }
    }
}
