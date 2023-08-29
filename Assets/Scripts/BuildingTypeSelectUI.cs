using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTypeSelectUI : MonoBehaviour
{

    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private List<BuildingTypeSO> ignoreBuildingTypeList;

    private Dictionary<BuildingTypeSO, Transform> buttonTransformDictionary;
    private Transform arrowButton;

    private void Awake()
    {
        Transform buttonTemplate = transform.Find("ButtonTemplate");
        buttonTemplate.gameObject.SetActive(false);

        BuildingTypeListSO buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);

        buttonTransformDictionary = new Dictionary<BuildingTypeSO, Transform>();

        int index = 0;


        arrowButton = Instantiate(buttonTemplate, transform);
        arrowButton.gameObject.SetActive(true);

        float offsetAmount = +130f;
        arrowButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);

        arrowButton.Find("Image").GetComponent<Image>().sprite = arrowSprite;
        arrowButton.Find("Image").GetComponentInParent<RectTransform>().sizeDelta = new Vector2(0, -35);

        arrowButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            BuildingManager.Instance.SetActiveBuildingType(null);
        });
        MouseEnterExitEvents mouseEnterExitEvents = arrowButton.GetComponent<MouseEnterExitEvents>();
        mouseEnterExitEvents.OnMouseEnter += (object sender, EventArgs e) =>
        {
            TooltipUI.Instance.Show("Arrow");
        };
        mouseEnterExitEvents.OnMouseExit += (object sender, EventArgs e) =>
        {
            TooltipUI.Instance.Hide();
        };
        index ++;


        foreach (BuildingTypeSO buildingType in buildingTypeList.list)
        {
            if (ignoreBuildingTypeList.Contains(buildingType)) continue; //Skips to the next foreach section
            Transform buttonTransform = Instantiate(buttonTemplate, transform);
            buttonTransform.gameObject.SetActive(true);

            offsetAmount = +130f;
            buttonTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index , 0);

            buttonTransform.Find("Image").GetComponent<Image>().sprite = buildingType.sprite;

            buttonTransform.GetComponent<Button>().onClick.AddListener(() =>
            {
                BuildingManager.Instance.SetActiveBuildingType(buildingType);
            });

            mouseEnterExitEvents = buttonTransform.GetComponent<MouseEnterExitEvents>();
            mouseEnterExitEvents.OnMouseEnter += (object sender, EventArgs e) =>
            {
                TooltipUI.Instance.Show(buildingType.nameString + "\n" + buildingType.GetConstructionResourceCostString());
            };
            mouseEnterExitEvents.OnMouseExit += (object sender, EventArgs e) =>
            {
                TooltipUI.Instance.Hide();
            };

            buttonTransformDictionary[buildingType] = buttonTransform;
            index++;
        }

    }

    private void Start()
    {
        BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnActiveBuildingTypeChanged;
        UpdateActiveBuildingTypeButton();
    }

    private void BuildingManager_OnActiveBuildingTypeChanged(object sender, BuildingManager.OnActiveBuildingTypeChangedEventArgs e)
    {
        UpdateActiveBuildingTypeButton();
    }

   
    private void UpdateActiveBuildingTypeButton()
    {
        arrowButton.Find("Selected").gameObject.SetActive(false);
        foreach (BuildingTypeSO buildingType in buttonTransformDictionary.Keys)
        {
            Transform buttonTransform = buttonTransformDictionary[buildingType];
            buttonTransform.Find("Selected").gameObject.SetActive(false);
        }

        BuildingTypeSO activeBuildingType = BuildingManager.Instance.GetActiveBuildingType();
        if (activeBuildingType == null)
        {
            arrowButton.Find("Selected").gameObject.SetActive(true);
        }
        else
        {
            buttonTransformDictionary[activeBuildingType].Find("Selected").gameObject.SetActive(true);
        }
    }
}
