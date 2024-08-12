using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private Animator animator;
    private float animationDuration;

    void Start()
    {
        animator = GetComponent<Animator>();
        animationDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        Invoke("DestroyExplosion", animationDuration);
    }

    void DestroyExplosion()
    {
        Destroy(gameObject);
    }
}
