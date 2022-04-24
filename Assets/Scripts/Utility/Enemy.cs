using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyPreset", menuName = "Enemy/EnemyPreset", order = 1)]
public class Enemy : ScriptableObject
{
    public GameObject enemyPrefab;
    public GameObject coinPrefab;
    public float maxHealth;
    public float health;
    public float damage;
    public float speed;
    public Vector2 moneyRange;
}
