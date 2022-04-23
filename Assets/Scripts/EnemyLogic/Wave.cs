using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Wave/WavePreset", order = 1)]
public class Wave : ScriptableObject
{
    [SerializeField] public List<Enemy> enemies = new List<Enemy>();
}
