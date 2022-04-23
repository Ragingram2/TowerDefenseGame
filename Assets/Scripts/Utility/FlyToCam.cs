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
            GetComponent<Rigidbody>().AddForce((Camera.main.transform.position - transform.position).normalized * 10.0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("Player"))
        {
            GameManager.AddFunds(10);
            Destroy(gameObject);
        }
    }

    void StartFlyToCam() => flyToCam = true;
}
