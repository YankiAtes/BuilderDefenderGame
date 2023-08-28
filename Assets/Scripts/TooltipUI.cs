using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TooltipUI : MonoBehaviour
{

    public static TooltipUI Instance { get; private set; }





    [SerializeField] private RectTransform canvasRectTransform;


    private RectTransform rectTransform;
    private TextMeshProUGUI textMeshPro;
    private RectTransform backgroundRectTransform;
    private void Awake()
    {
        Instance = this;    
        rectTransform = GetComponent<RectTransform>();
        textMeshPro = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();

        Hide();
    }

    private void Update()
    {
        Vector2 anchoredPosition = rectTransform.anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;
        
        if (anchoredPosition.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width)
        {
            anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
        }

        if (anchoredPosition.y + backgroundRectTransform.rect.height > canvasRectTransform.rect.height)
        {
            anchoredPosition.y = canvasRectTransform.rect.height - backgroundRectTransform.rect.height;
        }

        rectTransform.anchoredPosition = anchoredPosition;
    }

    private void SetText(string tooltipText)
    {
        textMeshPro.SetText(tooltipText);
        textMeshPro.ForceMeshUpdate();

        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        Vector2 padding = new Vector2(8, 8);
        backgroundRectTransform.sizeDelta = textSize + padding;
    }

    public void Show(string tooltipText)
    {
        gameObject.SetActive(true);
        SetText(tooltipText);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
