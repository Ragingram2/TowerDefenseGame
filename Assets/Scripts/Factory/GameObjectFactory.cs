using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameObjectFactory<T> : UnityEngine.Object where T : ScriptableObject
{
    public abstract GameObject CreateInstance(T preset);
}
//Factory
public class EnemyFactory : GameObjectFactory<Enemy>
{
    public override GameObject CreateInstance(Enemy preset)
    {
        var go = Instantiate(preset.enemyPrefab);
        go.GetComponent<EnemyLogic>().enemy = preset;
        return go;
    }

    public static GameObject CreateEnemy(Enemy preset)
    {
        EnemyFactory factory = new EnemyFactory();
        return factory.CreateInstance(preset);
    }
}

public class TowerFactory : GameObjectFactory<TowerPreset>
{
    public override GameObject CreateInstance(TowerPreset preset)
    {
        var go = Instantiate(preset.towerPrefab);
        go.transform.localPosition = Vector3.zero;
        go.GetComponent<Tower>().preset = preset;
        return go;
    }

    public static GameObject CreateTower(Transform parent, TowerPreset preset)
    {
        TowerFactory factory = new TowerFactory();
        var go = factory.CreateInstance(preset);
        go.transform.parent = parent;
        go.transform.localPosition = Vector3.zero;
        return go;
    }
}