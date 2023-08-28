using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePositionSortingOrder : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private bool runOnce;
    [SerializeField] float positionOffsetY;  
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void LateUpdate()
    {
        float presicionMultiplier = 5f;
        spriteRenderer.sortingOrder = (int)-((transform.position.y + positionOffsetY) * presicionMultiplier);
         
        if (runOnce)
        {
            Destroy(this); //Destroying the script  
        }
    }
}