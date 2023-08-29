using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private BuildingTypeSO buildingType;    
    private HealthSystem healthSystem;
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
}
