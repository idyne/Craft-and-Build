using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;
public abstract class FarmableObject : MonoBehaviour
{
    [SerializeField] protected int totalAmount = 8;
    [SerializeField] protected int unitAmount = 2;
    [SerializeField] protected int amount = 1;
    [SerializeField] protected float respawnTime = 20;
    [SerializeField] protected GameObject collectibleObjectPrefab = null;
    private static CraftLevel levelManager = null;

    public int Amount
    {
        get
        {
            return amount;
        }
    }

    private void Awake()
    {
        ResetAmount();
        if (!levelManager)
            levelManager = (CraftLevel)LevelManager.Instance;
    }

    protected void ResetAmount()
    {
        amount = totalAmount;
    }

    public int GetFarmed()
    {
        int result = 0;
        if (amount > 0)
        {
            result = amount >= unitAmount ? unitAmount : amount;
            amount -= result;
            Animate();
            CollectibleObject collectibleObject = Instantiate(collectibleObjectPrefab, transform.position, collectibleObjectPrefab.transform.rotation, levelManager.CollectibleObjectContainer).GetComponent<CollectibleObject>();
            collectibleObject.Amount = result;
            if (amount <= 0)
                LeanTween.delayedCall(respawnTime, Respawn);
        }
        return result;
    }

    protected abstract void Respawn();

    protected abstract void Animate();
}
