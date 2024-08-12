using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;

    // дробовик
    public bool isShotgun = false;
    public float maxDistance = 7f;

    //гранатомёт
    public bool isGrenade = false;
    public float radius = 2f;
    private Vector2 targetPosition;

    private Vector2 direction;
    private Vector3 startPosition;

    public GameObject explosionPrefab;

    void Start()
    {
        startPosition = transform.position;
    }
    void Update()
    {
        if (isGrenade) //если граната
        {
            Vector2 position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            transform.position = position;

            if ((Vector2)transform.position == targetPosition)
            {
                Explode();//взрыв
            }
        }
        else 
        {
            transform.Translate(direction * speed * Time.deltaTime, Space.World);

            if (isShotgun) // если дробовик, то проверяем дальность полета
            {
                if (Vector3.Distance(startPosition, transform.position) > maxDistance)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }
    public void SetTarget(Vector2 target)
    {
        targetPosition = target;
    }

    void Explode()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        explosion.transform.localScale = new Vector3(radius, radius, 1);
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.tag == "enemy")
            {
                EnemyController enemy = collider.gameObject.GetComponent<EnemyController>();
                enemy.TakeDamage(damage);
            }
        }
        Destroy(gameObject); 
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isGrenade)
        {
            if (collision.gameObject.tag == "enemy")
            {
                Debug.Log("Enemy hit by bullet");
                EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
                enemy.TakeDamage(damage);
                Destroy(gameObject);
            }
            if (collision.gameObject.tag == "border") Destroy(gameObject); // уничтожение пули при столкновении с границей карты
        }

    }
    void OnDrawGizmos() //отрисовка в редакторе радиуса гранаты
    {
        if (isGrenade) 
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }       
    }
}

