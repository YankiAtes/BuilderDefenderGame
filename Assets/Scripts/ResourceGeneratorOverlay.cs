using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceGeneratorOverlay : MonoBehaviour
{
    [SerializeField] private ResourceGenerator resourceGenerator;

    private Transform barTransform;

    private void Start()
    {
        barTransform = transform.Find("Bar");
        ResourceGeneratorData resourceGeneratorData = resourceGenerator.GetResourceGeneratorData();
        transform.Find("Icon").GetComponent<SpriteRenderer>().sprite =resourceGeneratorData.resourceType.sprite;

        transform.Find("Text").GetComponent<TextMeshPro>()
            .SetText(resourceGenerator.GetAmountGeneratedPerSecond().ToString("F1")); //F1: 1 DECIMAL POINT  
    }

    private void Update()
    {
      barTransform.localScale = new Vector3(1 - resourceGenerator.GetTimerNormalized(), 1, 1);
    }
}
