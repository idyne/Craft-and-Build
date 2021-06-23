using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Construction : MonoBehaviour
{
    [SerializeField] private int constructionID = -1;
    [SerializeField] private bool showTotalMaterials = false;
    [SerializeField] private GameObject buildingPrefab = null;
    [SerializeField] private ConstructionStageSet stages = null;
    [SerializeField] private Text stoneText = null;
    [SerializeField] private Text woodText = null;
    [SerializeField] private Text grassText = null;
    [SerializeField] private GameObject stonePanel = null;
    [SerializeField] private GameObject woodPanel = null;
    [SerializeField] private GameObject grassPanel = null;
    public ConstructionData Data = null;
    public GameObject BuildingPrefab { get => buildingPrefab; }
    public int ConstructionID { get => constructionID; }

    private ConstructionStage currentStage = null;
    private GameObject currentStageObject = null;

    public bool IsStageComplete { get => !NeedStone && !NeedWood && !NeedGrass; }

    private void Awake()
    {
        SynchronizeData();
    }

    public void AddMaterial(int stone, int wood, int grass)
    {
        if (stone > 0)
            AddStone(stone);
        if (wood > 0)
            AddWood(wood);
        if (grass > 0)
            AddGrass(grass);
        if (IsStageComplete)
            NextStage();
    }
    private void AddStone(int gain)
    {
        if (Data.Stone + gain <= currentStage.RequiredStone)
        {
            Data.Stone += gain;
            UpdateStoneText();
        }
    }

    private void AddWood(int gain)
    {
        if (Data.Wood + gain <= currentStage.RequiredWood)
        {
            Data.Wood += gain;
            UpdateWoodText();
        }
    }

    private void AddGrass(int gain)
    {
        if (Data.Grass + gain <= currentStage.RequiredGrass)
        {
            Data.Grass += gain;
            UpdateGrassText();
        }
    }

    public bool NeedStone
    {
        get
        {
            if (Data.Stone >= currentStage.RequiredStone)
                return false;
            return true;
        }
    }

    public bool NeedWood
    {
        get
        {
            if (Data.Wood >= currentStage.RequiredWood)
                return false;
            return true;
        }
    }

    public bool NeedGrass
    {
        get
        {
            if (Data.Grass >= currentStage.RequiredGrass)
                return false;
            return true;
        }
    }

    #region UI

    protected void UpdateStoneText()
    {
        stoneText.text = (currentStage.RequiredStone - Data.Stone).ToString();
        stonePanel.SetActive(NeedStone);
    }

    protected void UpdateWoodText()
    {
        woodText.text = (currentStage.RequiredWood - Data.Wood).ToString();
        woodPanel.SetActive(NeedWood);
    }

    protected void UpdateGrassText()
    {
        grassText.text = (currentStage.RequiredGrass - Data.Grass).ToString();
        grassPanel.SetActive(NeedGrass);
    }

    protected void UpdateTexts()
    {
        UpdateStoneText();
        UpdateWoodText();
        UpdateGrassText();
    }

    #endregion

    private void SynchronizeData()
    {
        this.LoadData();
        currentStage = stages.GetStage(Data.StageIndex);
        if (currentStage == null)
        {
            Complete();
            return;
        }
        currentStageObject = Instantiate(currentStage.StagePrefab, transform.position, currentStage.StagePrefab.transform.rotation, transform);
        UpdateTexts();
    }

    private void NextStage()
    {
        Data.StageIndex++;
        currentStage = stages.GetStage(Data.StageIndex);
        if (currentStage == null)
        {
            Complete();
            return;
        }
        if (currentStageObject)
            Destroy(currentStageObject);
        currentStageObject = Instantiate(currentStage.StagePrefab, transform.position, currentStage.StagePrefab.transform.rotation, transform);
        ResetMaterials();
        UpdateTexts();
    }

    private void Complete()
    {
        Instantiate(buildingPrefab, transform.position, buildingPrefab.transform.rotation);
        gameObject.SetActive(false);
    }

    private void ResetMaterials()
    {
        Data.Stone = 0;
        Data.Wood = 0;
        Data.Grass = 0;
    }


}
