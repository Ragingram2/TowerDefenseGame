using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOETower : TowerLogic
{
    private GameObject target;
    private int damage;
    private int damageRadius;
    private float damageMod;
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

        transform.LookAt(target.transform);
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
        damage = tower.isUpgraded ? preset.upgradeDamage : preset.baseDamage;
        radius = tower.isUpgraded ? preset.upgradeRadius : preset.baseRadius;
        damageRadius = tower.isUpgraded ? preset.upgradeDamageRadius : preset.baseDamageRadius;
        damageMod = tower.isUpgraded ? preset.upgradeDamageModifier : preset.baseDamageModifier;

        if (!target)
            return;

        var targetPos = target.transform.position;
        target.GetComponent<EnemyLogic>().DealDamage(damage);
        var colliders = Physics.OverlapSphere(targetPos, damageRadius);
        foreach (Collider col in colliders)
        {
            if (col.tag.Equals("Enemy"))
                col.GetComponent<EnemyLogic>().DealDamage((int)(damage * damageMod));
        }
    }
}
