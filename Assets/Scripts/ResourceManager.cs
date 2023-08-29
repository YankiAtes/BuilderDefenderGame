using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }
    private Dictionary<ResourceTypeSO, int> resourceAmmountDictionary;

    public event EventHandler OnResourceAmountChanged;

    [SerializeField] private List<ResourceAmount> startingResourceAmountList;
    private void Awake()
    {
        Instance = this;
        resourceAmmountDictionary = new Dictionary<ResourceTypeSO, int>();

        ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

        foreach (ResourceTypeSO resourceType in resourceTypeList.list )
        {
            resourceAmmountDictionary[resourceType] = 0;
        }

        foreach (ResourceAmount resourceAmount in startingResourceAmountList)
        {
            AddResource(resourceAmount.resourceType, resourceAmount.amount);
        }
        
    }
    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.T))
        {
            ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
            AddResource(resourceTypeList.list[0], 2);
            TestLogResourceAmountDictionary();
        }*/
    }
    private void TestLogResourceAmountDictionary()
    {
        foreach(ResourceTypeSO resourceType in resourceAmmountDictionary.Keys )
        {
            Debug.Log(resourceType.nameString + ": " + resourceAmmountDictionary[resourceType]);
        }
    }
    public void AddResource(ResourceTypeSO resourceType, int amount)
    {
        resourceAmmountDictionary[resourceType] += amount;

        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);

        //TestLogResourceAmountDictionary();  
    }
    public int GetResourceAmount(ResourceTypeSO resourceType)
    {
        return resourceAmmountDictionary[resourceType];
    }

    public bool CanAfford(ResourceAmount[] resourceAmountArray)
    {
        foreach (ResourceAmount resourceAmount in resourceAmountArray)
        {
            if (GetResourceAmount(resourceAmount.resourceType) >= resourceAmount.amount)
            {
                //Can afford single type

            }
            else
            {
                //Can't afford
                return false;
            }
        }
        //Can afford all 
        return true; 
    }

    public void SpendResources(ResourceAmount[] resourceAmountArray)
    {
        foreach (ResourceAmount resourceAmount in resourceAmountArray)
        {
            resourceAmmountDictionary[resourceAmount.resourceType] -= resourceAmount.amount;
        }
    }
}
