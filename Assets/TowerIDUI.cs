using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerIDUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent.parent.GetComponentInChildren<Tower>())
            GetComponentInChildren<TMP_Text>().text = $"{ transform.parent.parent.GetComponentInChildren<Tower>().towerId}";
    }
}
