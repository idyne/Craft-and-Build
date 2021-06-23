using FateGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleObject : MonoBehaviour
{
    private static CraftLevel levelManager = null;
    public int Amount = 1;
    private ObjectState state = ObjectState.IDLE;
    [SerializeField] private ObjectType type = ObjectType.STONE;
    [SerializeField] private GameObject mesh = null;

    public ObjectState State { get => state; }
    public ObjectType Type { get => type; }

    private void Awake()
    {
        if (!levelManager)
            levelManager = (CraftLevel)LevelManager.Instance;
    }
    private void Start()
    {
        GetDropped();
    }
    public void GetDropped()
    {
        if (state == ObjectState.IDLE)
        {
            state = ObjectState.DROPPING;
            Vector3 targetPosition = transform.position;
            targetPosition.x += Random.value < 0.5f ? Random.Range(-2f, -1f) : Random.Range(1, 2f);
            targetPosition.y = 0;
            targetPosition.z += Random.value < 0.5f ? Random.Range(-2f, -1f) : Random.Range(1, 2f);
            transform.SimulateProjectileMotion(targetPosition, 0.6f, () =>
            {
                state = ObjectState.DROPPED;
            });
        }
    }

    public void GetCollected(Player by)
    {
        if (state == ObjectState.DROPPED)
        {
            state = ObjectState.COLLECTING;
            float timePassed = 0;
            LeanTween.value(gameObject, (float value) =>
            {
                float deltaTime = timePassed;
                timePassed = value;
                deltaTime = timePassed - deltaTime;
                transform.position += (by.transform.position - transform.position) * deltaTime;
            }, 0, 1, 0.4f).setEaseInCubic().setOnComplete(() =>
            {
                state = ObjectState.COLLECTED;
                switch (Type)
                {
                    case ObjectType.STONE:
                        by.Data.Stone += Amount;
                        break;
                    case ObjectType.WOOD:
                        by.Data.Wood += Amount;
                        break;
                    case ObjectType.GRASS:
                        by.Data.Grass += Amount;
                        break;
                }
                mesh.SetActive(false);
            });
        }
    }

    public enum ObjectType { STONE, WOOD, GRASS }
    public enum ObjectState { COLLECTED, IDLE, COLLECTING, DROPPING, DROPPED }
}
