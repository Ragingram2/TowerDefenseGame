using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class EnemyLogic : MonoBehaviour
{
    [SerializeField] private Vector3 targetPos;

    [HideInInspector] public float health;
    [HideInInspector] public float moneyCarried;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Enemy enemy;
    [HideInInspector] public TMP_Text healthText;

    public bool isDebuffed => agent.speed < enemy.speed;

    public void Start()
    {
        Init();
    }

    public void Init()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent)
        {
            agent.enabled = true;//we do this cause theres some weird bug with the navmesh
            agent.SetDestination(targetPos);
            agent.speed = enemy.speed;
        }

        health = enemy.maxHealth;
        moneyCarried = Random.Range(enemy.moneyRange.x, enemy.moneyRange.y);
        healthText = GetComponentInChildren<TMP_Text>();
    }

    public void DealDamage(int num)
    {
        health -= num;
        if (healthText)
            healthText.text = health + "";

        if (health <= 0)
            OnDeath();
    }

    public void ChangeSpeed(int amount, float duration)
    {
        if (!agent)
            return;

        agent.speed -= amount;
        StartCoroutine(ResetDebuff());
        IEnumerator ResetDebuff()
        {
            yield return new WaitForSeconds(duration);
            agent.speed = enemy.speed;
        }
    }

    private void OnDeath()
    {
        for (int i = 0; i < moneyCarried; i++)
        {
            Instantiate(enemy.coinPrefab, transform.position + Random.insideUnitSphere * .1f, Quaternion.identity).GetComponent<Rigidbody>()?.AddExplosionForce(100.0f, transform.position, 0);
        }
        DestroyImmediate(gameObject);
    }
}
