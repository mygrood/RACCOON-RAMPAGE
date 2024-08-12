using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyStats enemyStats; 
    public GameObject player;
    private Animator animator;

    private bool isDead;

    private int HP;
    void Start()
    {
        HP = enemyStats.HP;
        Debug.Log($"HP {HP}");
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }   
    void Update()
    {
        if (!isDead)
        {
            FollowPlayer();
        }
             
    }

    //движение за игроком
    void FollowPlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.Normalize();
        Vector3 velocity = direction * enemyStats.Speed;
        transform.position += velocity * Time.deltaTime;

        animator.SetFloat("horizontal",velocity.x);
        animator.SetFloat("vertical",velocity.y);
    }     
    
    //получение урона
    public void TakeDamage(int damage)
    {        
       HP -= damage;
        animator.SetTrigger("damage");
        Debug.Log($"Damage {damage}, EnemyStatsHP {enemyStats.HP}, HP {HP}");
        if (HP <= 0)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            playerController.score += enemyStats.Points; //прибавляем очки
            //Destroy(gameObject,1f);
            animator.SetTrigger("death");
            isDead = true;
            
            Collider2D collider = GetComponent<Collider2D>(); 
            collider.enabled = false;    
            
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            playerController.TakeDamage(enemyStats.Damage);
        }
    }


}
