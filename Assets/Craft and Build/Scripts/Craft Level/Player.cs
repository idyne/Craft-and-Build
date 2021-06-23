using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    private Rigidbody rb = null;
    private Swerve2D swerve = null;
    private List<FarmableObject> farmableObjectsInRange = null;
    private Construction constructionInRange = null;
    private float farmCooldownTime = 0;
    private float constructCooldownTime = 0;
    public PlayerData Data = null;
    [SerializeField] private float speed = 1;
    [SerializeField] private float farmRange = 2f;
    [SerializeField] private float farmCooldown = 1;
    [SerializeField] private float collectRange = 3;
    [SerializeField] private float constructRange = 1;
    [SerializeField] private float constructCooldown = 0.05f;
    [SerializeField] private LayerMask farmableObjectLayerMask = 0;
    [SerializeField] private LayerMask collectibleObjectLayerMask = 0;
    [SerializeField] private LayerMask constructionLayerMask = 0;
    [SerializeField] private LayerMask obstacleLayerMask = 0;


    private void Awake()
    {
        this.LoadData();
        rb = GetComponent<Rigidbody>();
        swerve = InputManager.CreateSwerve2D();
        farmableObjectsInRange = new List<FarmableObject>();
        SetRigidbody();
    }


    private void Update()
    {
        CheckInput();
        if (farmableObjectsInRange.Count > 0 && farmCooldownTime <= 0)
            Farm();
        else
            farmCooldownTime = Mathf.Clamp(farmCooldownTime - Time.deltaTime, 0, farmCooldown);
        if (constructionInRange && constructCooldownTime <= 0)
            Construct();
        else
            constructCooldownTime = Mathf.Clamp(constructCooldownTime - Time.deltaTime, 0, constructCooldown);
        Collect();
    }

    private void CheckInput()
    {
        if (swerve.Active)
        {
            Vector3 direction = swerve.Difference.normalized;
            direction.z = direction.y;
            direction.y = 0;
            if (!Physics.Raycast(transform.position, direction, out RaycastHit hit, 0.5f, obstacleLayerMask))
            {
                Move(direction, swerve.Rate);
                CheckFarm();
                CheckConstruction();
            }
        }

    }
    private void Move(Vector3 direction)
    {
        Move(direction, 1);
    }
    private void Move(Vector3 direction, float speedMultiplier)
    {
        if (direction.magnitude > 0)
            transform.rotation = Quaternion.LookRotation(direction);
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, Time.deltaTime * speed * speedMultiplier);
    }

    private void SetRigidbody()
    {
        rb.isKinematic = false;
    }

    private void CheckFarm()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, farmRange, farmableObjectLayerMask);
        farmableObjectsInRange.Clear();
        for (int i = 0; i < colliders.Length; i++)
        {
            FarmableObject farmableObject = colliders[i].GetComponent<FarmableObject>();
            if (!farmableObject)
                print(colliders[i].name);
            if (!farmableObjectsInRange.Contains(farmableObject) && farmableObject.Amount > 0)
                farmableObjectsInRange.Add(farmableObject);
        }
    }

    private void Farm()
    {
        /*
         * TODO
         * 
         * Animate swing
         * 
         */
        farmCooldownTime = farmCooldown;
        int i = 0;
        while (i < farmableObjectsInRange.Count)
        {
            FarmableObject farmableObject = farmableObjectsInRange[i];
            farmableObject.GetFarmed();
            if (farmableObject.Amount <= 0)
                farmableObjectsInRange.RemoveAt(i);
            i++;
        }
    }

    private void CheckConstruction()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, constructRange, constructionLayerMask);
        if (colliders.Length > 0)
            constructionInRange = colliders[0].GetComponent<Construction>();
        else constructionInRange = null;
    }

    private void Construct()
    {
        constructCooldownTime = constructCooldown;
        if (!constructionInRange.gameObject.activeSelf)
        {
            constructionInRange = null;
            return;
        }
        if (Data.Stone > 0 && constructionInRange.NeedStone)
        {
            Data.Stone -= 1;
            constructionInRange.AddMaterial(1, 0, 0);
        }
        if (Data.Wood > 0 && constructionInRange.NeedWood)
        {
            Data.Wood -= 1;
            constructionInRange.AddMaterial(0, 1, 0);
        }
        if (Data.Grass > 0 && constructionInRange.NeedGrass)
        {
            Data.Grass -= 1;
            constructionInRange.AddMaterial(0, 0, 1);
        }
    }
    private void Collect()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, collectRange, collectibleObjectLayerMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            CollectibleObject collectibleObject = colliders[i].GetComponent<CollectibleObject>();
            collectibleObject.GetCollected(this);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, farmRange);
    }

}
