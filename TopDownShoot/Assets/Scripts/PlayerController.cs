using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    public PlayerStats playerStats;
       
    public float moveSpeed;
    public float HP = 100;    

    private bool isShield = false; //�������� ���

    private Vector2 moveInput;
    private Vector2 mousePosition;
    private float angle;

    public GameObject bubble;

    public int score = 0;
    public bool isDead = false;

    private void Start()
    {
        moveSpeed = playerStats.moveSpeed;
        HP = playerStats.HP;        
        anim = GetComponentInChildren<Animator>();
    }
    void FixedUpdate()
    {
        HandleMovement();
        Debug.Log(HP);
    }

    //��������
    void HandleMovement()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(moveInput.x, moveInput.y, 0f).normalized;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        anim.SetFloat("horizontal", moveDirection.x);
        anim.SetFloat("vertical", moveDirection.y);
        anim.SetFloat("speed", moveSpeed);
    }


    //�������� ������
    public void OnBonusPower(string bonus)
    {
        if (bonus == "Speed")
        {
            StartCoroutine(SpeedBoost());
        }
        else if (bonus == "Shield")
        {
            StartCoroutine(ShieldBoost());
        }
    }

    //���������� �������� �� 10 ���
    private IEnumerator SpeedBoost()
    {
        moveSpeed *= 1.5f;
        yield return new WaitForSeconds(10.0f);
        moveSpeed = playerStats.moveSpeed;
    }

    //������������ �� 10 ���
    private IEnumerator ShieldBoost()
    {
        bubble.SetActive(true);
        isShield = true;
        yield return new WaitForSeconds(10.0f);
        isShield = false;
        bubble.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        if (!isShield)
        {
            Debug.Log(HP);
            HP -= damage;
            if (HP <= 0)
            {
                Die();
            }
        }

    }


    public void Die()
    {

        isDead = true;

    }
}
