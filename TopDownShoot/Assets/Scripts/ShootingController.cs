using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    private Vector2 mousePosition;
    private float angle;

    public Weapon currentWeapon; //оружие
    public float shootCooldown = 2; //время перезарядки
    public GameObject bulletPrefab;

    public float rotateSpeed = 180f;

    public Sprite spritePistol;    
    public Sprite spriteGun;
    
    public Sprite spriteGrenade;
    
    public Sprite spriteShotgun;
    
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        DisplayWeapon();
    }

    void FixedUpdate()
    {
        if (shootCooldown > 0)
        {
            shootCooldown -= Time.deltaTime;
        }
        HandleShooting();

        
    }


    public void DisplayWeapon()
    {
        switch (currentWeapon.weaponName)
        {
            case "pistol":
                spriteRenderer.sprite = spritePistol;
                break;
            case "gun":
                spriteRenderer.sprite = spriteGun;
                break;
            case "shotgun":
                spriteRenderer.sprite = spriteShotgun;
                break;
            case "grenade":
                spriteRenderer.sprite = spriteGrenade;
                break;
        }
    }

    //поворот и стрельба
    void HandleShooting()
    {

        if (Input.GetMouseButton(0) && shootCooldown <= 0)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;
            angle = Vector2.SignedAngle(Vector2.right, direction);
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            transform.rotation = targetRotation;

            Shoot();
            shootCooldown = 1f / currentWeapon.fireRate;
        }
        float rotationZ = transform.eulerAngles.z;
        if ((rotationZ > 90 && rotationZ < 270))
        {
            spriteRenderer.flipY = true;
        }
        else
        {
            spriteRenderer.flipY = false;
        }


    }

    //тип выстрела в зависимости от оружия
    void Shoot()
    {
        switch (currentWeapon.weaponName)
        {
            case "pistol":
            case "gun":
                ShootBullet();
                break;
            case "shotgun":
                ShootShotgun();
                break;
            case "grenade":
                ShootGrenade();
                break;
        }
    }

    //обычный выстрел (пистолет,автомат)
    void ShootBullet()
    {
        GameObject bullet = Instantiate(currentWeapon.bulletPrefab, transform.position + transform.right * 0.5f, transform.rotation);
       
        Bullet bulletProp = bullet.GetComponent<Bullet>();
        bulletProp.SetDirection(transform.right);
        bulletProp.damage = currentWeapon.damage;

    }

    //выстрел из дробовика
    void ShootShotgun()
    {
        float startAngle = -currentWeapon.shotAngle / 2;
        float angleStep = currentWeapon.shotAngle / (currentWeapon.shotBulletsCount - 1);

        for (int i = 0; i < currentWeapon.shotBulletsCount; i++)
        {
            float currentAngle = startAngle + (angleStep * i);
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, currentAngle));
            Vector2 direction = rotation * transform.right;

            GameObject bullet = Instantiate(currentWeapon.bulletPrefab, transform.position, transform.rotation * rotation);
           
            Bullet bulletProp = bullet.GetComponent<Bullet>();

            bulletProp.isShotgun = true;
            bulletProp.damage = currentWeapon.damage;
            bulletProp.maxDistance = currentWeapon.shotRange;
            bulletProp.SetDirection(direction);
        }
    }

    //выстрел из гранатомёта
    void ShootGrenade()
    {
        GameObject grenade = Instantiate(currentWeapon.bulletPrefab, transform.position, Quaternion.identity);
        Bullet bulletProp = grenade.GetComponent<Bullet>();
        
        bulletProp.isGrenade = true;
        bulletProp.damage = currentWeapon.damage;
        bulletProp.SetTarget(mousePosition);
    }

}
