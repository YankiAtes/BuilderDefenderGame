using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;

    private Transform barTransform;

    private void Awake()
    {
        barTransform = transform.Find("Bar");
    }
    private void Start()
    {
        healthSystem.onDamaged += HealthSystem_onDamaged;
        healthSystem.onHealed += HealthSystem_onHealed;
        UpdateBar();
        UpdateHealthBarVisible();
    }

    private void HealthSystem_onHealed(object sender, System.EventArgs e)
    {
        UpdateBar();
        UpdateHealthBarVisible(); 
    }

    private void HealthSystem_onDamaged(object sender, System.EventArgs e)
    {
        UpdateBar();
        UpdateHealthBarVisible();
    }

    private void UpdateBar()
    {
        barTransform.localScale = new Vector3(healthSystem.GetHealthAmountNormalized(), 1, 1);
    }

    private void UpdateHealthBarVisible()
    {
        if (healthSystem.IsFullHealth())
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
