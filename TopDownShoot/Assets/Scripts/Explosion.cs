using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private Animator animator;
    private float animationDuration;
    public int damage;
    public float radius;
    void Start()
    {
        transform.localScale = new Vector3(radius, radius, 1);
        animator = GetComponent<Animator>();
        animationDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        Invoke("DestroyExplosion", animationDuration);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.tag == "enemy")
            {
                EnemyController enemy = collider.gameObject.GetComponent<EnemyController>();
                enemy.TakeDamage(damage);
            }
        }
    }

    void DestroyExplosion()
    {
        Destroy(gameObject);
    }
}
