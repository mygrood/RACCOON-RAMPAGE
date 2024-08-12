using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusWeapon : MonoBehaviour
{
    public Weapon weapon; //��� ������    
    void Start()
    {
        Destroy(gameObject, 5);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ShootingController shootingController = collision.gameObject.GetComponentInChildren<ShootingController>();
            shootingController.currentWeapon = weapon; //�������� ������ ������
            shootingController.DisplayWeapon();
            Destroy(gameObject);
        }
    }

}
