using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;

    // ��������
    public bool isShotgun = false;
    public float maxDistance = 7f;

    //���������
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
        if (isGrenade) //���� �������
        {
            Vector2 position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            transform.position = position;
            // ������� ���� � ����������� ��������
            Vector2 direction = targetPosition - (Vector2)transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            if ((Vector2)transform.position == targetPosition)
            {
                Explode();//�����
            }
        }
        else 
        {
            transform.Translate(direction * speed * Time.deltaTime, Space.World);

            if (isShotgun) // ���� ��������, �� ��������� ��������� ������
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
        explosion.GetComponent<Explosion>().radius = radius;
        explosion.GetComponent<Explosion>().damage = damage;

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
            if (collision.gameObject.tag == "border") Destroy(gameObject); // ����������� ���� ��� ������������ � �������� �����
        }

    }
    void OnDrawGizmos() //��������� � ��������� ������� �������
    {
        if (isGrenade) 
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }       
    }
}

