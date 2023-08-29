using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{

    public static ArrowProjectile Create(Vector3 position, Enemy enemy)
    {
        Transform pfArrowProjectile = Resources.Load<Transform>("pfArrowProjectile");
        Transform arrowTransform = Instantiate(pfArrowProjectile, position, Quaternion.identity);

        ArrowProjectile arrowProjectile = arrowTransform.GetComponent<ArrowProjectile>();
        arrowProjectile.SetTarget(enemy);
        return arrowProjectile;
    }

    private Enemy targetEnemy;
    private float moveSpeed;

    private Vector3 lastMoveDir;

    private float timeToDie = 2f;
    void Start()
    {
        moveSpeed = 20f; 
    }

    void Update()
    {
        Vector3 moveDir;
        if (targetEnemy != null)
        {
            moveDir = (targetEnemy.transform.position - transform.position).normalized;
            lastMoveDir = moveDir;
        }else
        {
            moveDir = lastMoveDir;
        }

        transform.position += moveDir * Time.deltaTime * moveSpeed;

        transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(moveDir));

        timeToDie -=Time.deltaTime; 
        if (timeToDie <= 0)
        {
            Destroy(gameObject);    
        }
    }

    private void SetTarget(Enemy targetEnemy)
    {
        this.targetEnemy = targetEnemy;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>(); 
        if (enemy != null)
        {
            //hit enemy
            int damageAmount = 10;
            enemy.GetComponent<HealthSystem>().Damage(damageAmount);
            Destroy(gameObject);
        }
    }
}
