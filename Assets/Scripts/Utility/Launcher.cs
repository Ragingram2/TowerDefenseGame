using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    public GameObject projectile;
    public void Fire()
    {
        var go = Instantiate(projectile, transform.position, Quaternion.LookRotation(transform.forward), null);
        go.AddComponent<Rigidbody>().AddForce(transform.forward * 500f);
        go.AddComponent<SphereCollider>().isTrigger = true;
        go.AddComponent<DestroyAfterSeconds>();
    }
}
