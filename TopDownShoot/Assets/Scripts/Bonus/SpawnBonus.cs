using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBonus : MonoBehaviour
{
    public GameObject[] weaponBonusPrefabs; //бонусы оружия
    public float spawnWeaponInterval = 10f;

    public GameObject[] powerBonusPrefabs; //бонусы усиления   
    public float spawnPowerInterval = 27f;

    public Camera cam;
    [SerializeField] ShootingController shootingController;

    private void Start()
    {
        StartCoroutine(SpawnWeapon());
        StartCoroutine(SpawnPower());
    }

    private IEnumerator SpawnWeapon()
    {
        while (true)
        {
            SpawnW();
            yield return new WaitForSeconds(spawnWeaponInterval);
        }
    }

    private void SpawnW()
    {
        Weapon currentWeapon = shootingController.currentWeapon;//тип оружия у игрока


        //генерируем оружие до тех пор, пока не появится отличающееся от текущего
        GameObject bonusPrefab;
        do
        {
            bonusPrefab = weaponBonusPrefabs[Random.Range(0, weaponBonusPrefabs.Length - 1)];
        }
        while (bonusPrefab.GetComponent<BonusWeapon>().weapon == currentWeapon);

        Vector2 spawnPosition = GetRandomSpawnPosition(); // выбираем случайную позицию
        if (spawnPosition != Vector2.zero)
        {
            Instantiate(bonusPrefab, spawnPosition, Quaternion.identity); //спавн
        }
    }
    private IEnumerator SpawnPower()
    {
        while (true)
        {
            SpawnP();
            yield return new WaitForSeconds(spawnPowerInterval);
        }
    }

    private void SpawnP()
    {
        GameObject bonusPrefab;

        bonusPrefab = powerBonusPrefabs[Random.Range(0, powerBonusPrefabs.Length - 1)]; //выбор случайного бонуса

        Vector2 spawnPosition = GetRandomSpawnPosition();
        Instantiate(bonusPrefab, spawnPosition, Quaternion.identity);   

    }
    private Vector2 GetRandomSpawnPosition()
    {
        //границы видимости камеры        
        float cameraHeight = 2f * cam.orthographicSize;
        float cameraWidth = cameraHeight * cam.aspect;
        Vector2 cameraPosition = cam.transform.position;
        Rect cameraBounds = new Rect(cameraPosition.x - cameraWidth / 2, cameraPosition.y - cameraHeight / 2, cameraWidth, cameraHeight);

        //случайные координаты в рамках камеры
        float x = Random.Range(cameraBounds.min.x, cameraBounds.max.x);
        float y = Random.Range(cameraBounds.min.y, cameraBounds.max.y);
        return new Vector2(x, y);
    }
}
