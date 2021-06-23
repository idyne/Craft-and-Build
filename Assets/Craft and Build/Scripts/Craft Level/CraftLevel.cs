using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;

public class CraftLevel : LevelManager
{

    private Transform levelContainer = null;
    private Transform collectibleObjectContainer = null;

    private Player player = null;
    private List<Construction> constructions = null;
    public Player Player { get => player; }
    public Transform CollectibleObjectContainer { get => collectibleObjectContainer; }

    private new void Awake()
    {
        base.Awake();
        levelContainer = GameObject.Find("Level").transform;
        collectibleObjectContainer = new GameObject("Collectible Objects").transform;
        collectibleObjectContainer.parent = levelContainer;
        player = FindObjectOfType<Player>();
        constructions = new List<Construction>(FindObjectsOfType<Construction>());
    }
    public override void FinishLevel(bool success)
    {
        GameManager.Instance.FinishLevel(success);
    }

    public override void StartLevel()
    {
        InvokeRepeating("Save", 1, 1);
    }

    private void Save()
    {
        player.SaveData();
        foreach (Construction construction in constructions)
        {
            construction.SaveData();
        }
    }


}
