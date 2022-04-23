using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HomeBase : MonoBehaviour
{
    public float health = 100.0f;

    void Update()
    {
        GetComponentInChildren<TMP_Text>().text = health + "";
        if (health <= 0.0f)
        {
            GameManager.EndGame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Enemy"))
        {
            var enemy = other.GetComponent<EnemyLogic>().enemy;
            health -= (enemy.health / enemy.maxHealth) * enemy.damage;
            Destroy(other.gameObject);
        }
    }
}
