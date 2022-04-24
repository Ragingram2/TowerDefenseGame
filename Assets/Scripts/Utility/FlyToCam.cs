using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyToCam : MonoBehaviour
{
    bool flyToCam = false;

    private void Start()
    {
        GameManager.OnWaveEnd += StartFlyToCam;
    }

    void Update()
    {
        if (flyToCam)
        {
            transform.position = Vector3.MoveTowards(transform.position, Camera.main.transform.position, Time.deltaTime*10f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.IsGameMode(GameMode.Build) && other.gameObject.name.Equals("Player"))
        {
            GameManager.AddFunds(10);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        GameManager.OnWaveEnd -= StartFlyToCam;
    }

    void StartFlyToCam()
    {
        flyToCam = true;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
