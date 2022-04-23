using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    public GameObject indicatorPrefab;
    private GameObject indicator;
    public void Show(Vector3 position)
    {
        if (!indicator)
            SetIndicator(indicatorPrefab);
        SetPosition(position);
    }

    public void Hide()
    {
        if (indicator)
            Destroy(indicator);
    }

    public void SetIndicator(GameObject indicator)
    {
        this.indicator = Instantiate(indicator, transform, false);
    }

    public void SetPosition(Vector3 position)
    {
        indicator.transform.localPosition = position;
    }
}
