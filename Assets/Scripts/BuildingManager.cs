using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }

    [SerializeField] private Building hqBuilding;

    private BuildingTypeSO activeBuildingType;
    private BuildingTypeListSO buildingTypeList;

    private Camera mainCamera;

    public event EventHandler<OnActiveBuildingTypeChangedEventArgs> OnActiveBuildingTypeChanged;

    public class OnActiveBuildingTypeChangedEventArgs : EventArgs
    {
        public BuildingTypeSO activeBuildingType;

    }

    private void Awake()
    {
        Instance = this;
        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name); 
    }
    private void Start()
    {
        mainCamera = Camera.main;
        hqBuilding.GetComponent<HealthSystem>().onDied += HQ_onDied;
    }

    private void HQ_onDied(object sender, EventArgs e)
    {
        GameOverUI.Instance.Show();
        SoundManager.Instance.PlaySound(SoundManager.Sound.GameOver);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if(activeBuildingType != null)
            {
                if (CanSpawnBuilding(activeBuildingType,UtilsClass.GetMouseWorldPosition(),out string errorMesage)) 
                {
                    
                        if (ResourceManager.Instance.CanAfford(activeBuildingType.constructionResourceCostArray))
                        {
                            ResourceManager.Instance.SpendResources(activeBuildingType.constructionResourceCostArray);
                        //Instantiate(activeBuildingType.prefab, UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
                        BuildingConstruction.Create(UtilsClass.GetMouseWorldPosition(),activeBuildingType);
                        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingPlaced);
                        }
                    else
                    {
                        TooltipUI.Instance.Show("Can't afford " + activeBuildingType.GetConstructionResourceCostString(),
                            new TooltipUI.TooltipTimer { timer = 2f }
                            );
                    }
                }
                else
                 {
                    TooltipUI.Instance.Show(errorMesage,new TooltipUI.TooltipTimer {timer = 2f});
                 }
            }  
        }  
    }

    

    public void SetActiveBuildingType(BuildingTypeSO buildingType)
    {
        activeBuildingType = buildingType;
        OnActiveBuildingTypeChanged?.Invoke(this,
            new OnActiveBuildingTypeChangedEventArgs { activeBuildingType = activeBuildingType}
        );
    }

    public BuildingTypeSO GetActiveBuildingType()
    {
        return activeBuildingType;
    }

    private bool CanSpawnBuilding(BuildingTypeSO buildingType, Vector3 position, out string errorMessage)
    {
        BoxCollider2D boxCollider2D = buildingType.prefab.GetComponent<BoxCollider2D>();

        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0);

        bool isAreaClear = collider2DArray.Length == 0;
        if (!isAreaClear)
        {
            errorMessage = "Area is not clear!";
            return false;
        }
        collider2DArray = Physics2D.OverlapCircleAll(position,buildingType.minConstructionRadius);
        foreach (Collider2D collider2d in collider2DArray)
        {
            //Buildings inside the constructino radius
            BuildingTypeHolder buildingTypeHolder = collider2d.GetComponent<BuildingTypeHolder>();
            if(buildingTypeHolder != null)
            {
                if(buildingTypeHolder.buildingType == buildingType)
                {
                    //Has same building in building radius
                    errorMessage = "Cannot build the same building too close!";
                    return false;
                }
            }
        }

        float maxConstructionRadius = 25;
        collider2DArray = Physics2D.OverlapCircleAll(position, maxConstructionRadius);
        foreach (Collider2D collider2d in collider2DArray)
        {
            //Buildings inside the constructino radius
            BuildingTypeHolder buildingTypeHolder = collider2d.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHolder != null)
            {
                //There is a building
                errorMessage = "";
                return true;
            }
        }
        errorMessage = "Cannot build too far from a building!";
        return false;  

    }

    public Building GetHQBuilding()
    {
        return hqBuilding;
    }
}
