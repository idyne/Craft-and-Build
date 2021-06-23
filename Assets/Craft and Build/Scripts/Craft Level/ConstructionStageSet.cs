using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConstructionStageSet", menuName = "Craft&Build/ConstructionStageSet", order = 1)]
public class ConstructionStageSet : ScriptableObject
{
    [SerializeField] private List<ConstructionStage> set = null;

    public ConstructionStage GetStage(int index)
    {
        if (index < set.Count && index >= 0)
            return set[index];
        else
            return null;
    }

    public int Count { get => set.Count; }

    public void TotalMaterials(out int stone, out int wood, out int grass)
    {
        stone = 0;
        wood = 0;
        grass = 0;
        foreach (ConstructionStage stage in set)
        {
            stone += stage.RequiredStone;
            wood += stage.RequiredWood;
            grass += stage.RequiredGrass;
        }
    }

    public void TotalMaterialsBetween(int startInclusive, int endExclusive, out int stone, out int wood, out int grass)
    {
        stone = 0;
        wood = 0;
        grass = 0;
        for (int i = startInclusive; i < Mathf.Clamp(endExclusive, 1, set.Count); i++)
        {
            ConstructionStage stage = set[i];
            stone += stage.RequiredStone;
            wood += stage.RequiredWood;
            grass += stage.RequiredGrass;
        }
    }
}
