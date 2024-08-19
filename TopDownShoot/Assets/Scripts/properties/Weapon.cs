using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName;
    public int damage;
    public float fireRate;

    public GameObject bulletPrefab;
    public AudioClip sound;
    public Sprite sprite;

    //дробовик
    public int shotBulletsCount;
    public float shotAngle; //угол
    public float shotRange; // дальность

    //гранатомёт
    public float grenadeRadius; 
}

