using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private BuildingTypeSO buildingType;    
    private HealthSystem healthSystem;
    private Transform buildingDemolishButton;
    private Transform buildingRepairButton;


    private void Awake()
    {
        buildingDemolishButton = transform.Find("BuildingDemolishButton");
        buildingRepairButton = transform.Find("BuildingRepairButton");
        HideBuildingDemolishButton();      
        HideBuildingRepairButton();
    }
    private void Start()
    {
        buildingType =   GetComponent<BuildingTypeHolder>().buildingType;
        healthSystem = GetComponent<HealthSystem>();
        buildingRepairButton = transform.Find("BuildingRepairButton");

        healthSystem.SetHealthAmountMax(buildingType.healthAmountMax,true);

        healthSystem.onDamaged += HealthSystem_onDamaged;
        healthSystem.onHealed += HealthSystem_onHealed;

        healthSystem.onDied += HealthSystem_onDied;
        
    }

    private void HealthSystem_onHealed(object sender, System.EventArgs e)
    {
        if (healthSystem.IsFullHealth())
        {
            HideBuildingRepairButton();
        }
    }

    private void HealthSystem_onDamaged(object sender, System.EventArgs e)
    {
        ShowBuildingRepairButton();
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


    private void ShowBuildingRepairButton()
    {
        if (buildingRepairButton != null)
        {
            buildingRepairButton.gameObject.SetActive(true);
        }
    }

    private void HideBuildingRepairButton()
    {
        if (buildingRepairButton!= null)
        {
            buildingRepairButton.gameObject.SetActive(false);
        }
    }
}
