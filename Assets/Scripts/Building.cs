using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private BuildingTypeSO buildingType;    
    private HealthSystem healthSystem;
    private Transform buildingDemolishButton;


    private void Awake()
    {
        buildingDemolishButton = transform.Find("BuildingDemolishButton");
        HideBuildingDemolishButton();        
    }
    private void Start()
    {
        buildingType =   GetComponent<BuildingTypeHolder>().buildingType;
        healthSystem = GetComponent<HealthSystem>();

        healthSystem.SetHealthAmountMax(buildingType.healthAmountMax,true);
     

        healthSystem.onDied += HealthSystem_onDied;
        
    }
    
    private void HealthSystem_onDied(object sender, System.EventArgs e)
    {
        Destroy(gameObject);
    }

    private void OnMouseEnter()
    {
        ShowBuildingDemolishButton();
    }

    private void OnMouseExit()
    {
        HideBuildingDemolishButton();
    }

    private void ShowBuildingDemolishButton()
    {
        if (buildingDemolishButton != null)
        {
            buildingDemolishButton.gameObject.SetActive(true);
        }
    }

    private void HideBuildingDemolishButton()
    {
        if (buildingDemolishButton != null)
        {
            buildingDemolishButton.gameObject.SetActive(false);
        }
    }
}
