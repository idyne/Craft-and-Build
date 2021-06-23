using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConstructionStage
{
    
    [SerializeField] private int requiredStone = 10;
    [SerializeField] private int requiredWood = 10;
    [SerializeField] private int requiredGrass = 10;
    [SerializeField] private GameObject stagePrefab = null;


    public int RequiredStone { get => requiredStone; }
    public int RequiredWood { get => requiredWood; }
    public int RequiredGrass { get => requiredGrass; }

    public GameObject StagePrefab { get => stagePrefab; }
}
