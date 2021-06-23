using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConstructionData
{
    public int ID = -1;
    public int StageIndex = 0;
    public int Stone = 0;
    public int Wood = 0;
    public int Grass = 0;

    public ConstructionData(int ID)
    {
        this.ID = ID;
    }
}
