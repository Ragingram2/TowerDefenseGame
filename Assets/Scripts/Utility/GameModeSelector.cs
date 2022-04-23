using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeSelector : MonoBehaviour
{
    [SerializeField] private GameMode mode;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(SetGameMode);
    }

    private void Update()
    {
        GetComponent<Button>().interactable = !GameManager.IsGameMode(GameMode.WaveInProgress);
    }

    public void SetGameMode()
    {
        GameManager.SetGameMode(mode);
    }
}
