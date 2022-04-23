using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildModeSelector : MonoBehaviour
{
    [SerializeField] private BuildMode mode;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(SetBuildMode);
    }

    public void SetBuildMode()
    {
        GameManager.SetBuildMode(mode);
    }
}
