using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTower : TowerLogic
{
    private GameObject target;
    private int damage;
    private int radius;

    void Update()
    {
        UpdateTarget();
    }

    public override void UpdateTarget()
    {
        GetComponent<SphereCollider>().radius = radius;

        if (targets == null || targets.Count < 1)
            return;

        while (targets[0] == null)
        {
            targets.RemoveAt(0);
            if (targets.Count < 1)
                return;
        }

        target = targets[0];

        transform.LookAt(target.transform.position - new Vector3(0.0f, 0.25f, 0.0f));
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
            if (other.gameObject == target)
                target = null;
            targets.Remove(other.gameObject);
        }
    }

    public override void Fire()
    {
        damage = tower.isUpgraded ? preset.upgradeDamage : preset.baseDamage;
        radius = tower.isUpgraded ? preset.upgradeRadius : preset.baseRadius;
        if (!target)
            return;

        target.GetComponent<EnemyLogic>().DealDamage(damage);
        gameObject.GetComponentInChildren<Launcher>()?.Fire();
    }
}
