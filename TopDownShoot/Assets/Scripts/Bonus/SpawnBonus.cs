using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnBonus : MonoBehaviour
{
    public GameObject[] weaponBonusPrefabs; //бонусы оружия
    public float spawnWeaponInterval = 10f;

    public GameObject[] powerBonusPrefabs; //бонусы усиления   
    public float spawnPowerInterval = 27f;

    public Camera cam;
    [SerializeField] ShootingController shootingController;

    private Bounds mapBounds;

    private void Start()
    {
        mapBounds = GetComponent<Tilemap>().localBounds;
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
    //Спавн внутри камеры
    /*
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
    */

    private Vector2 GetRandomSpawnPosition()
    {
        float cameraHeight = 2f * cam.orthographicSize;
        float cameraWidth = cameraHeight * cam.aspect;
        Vector2 cameraPosition = cam.transform.position;
        Rect cameraBounds = new Rect(cameraPosition.x - cameraWidth / 2, cameraPosition.y - cameraHeight / 2, cameraWidth, cameraHeight);

        Vector2 spawnPosition;
        int attempts = 0;
        const int maxAttempts = 100;
        float bufferDistance = 3f;

        do
        {  // Генерируем случайную позицию за границами камеры
            float x = Random.Range(cameraBounds.xMin - bufferDistance, cameraBounds.xMax + bufferDistance);
            float y = Random.Range(cameraBounds.yMin - bufferDistance, cameraBounds.yMax + bufferDistance);

            // Случайное смещение позиции за границы камеры
            if (x > cameraBounds.xMin && x < cameraBounds.xMax)
            {
                if (Random.value > 0.5f)
                    y = cameraBounds.yMax + bufferDistance; // сверху
                else
                    y = cameraBounds.yMin - bufferDistance; // снизу
            }
            else
            {
                if (Random.value > 0.5f)
                    x = cameraBounds.xMax + bufferDistance; // справа
                else
                    x = cameraBounds.xMin - bufferDistance; // слева
            }

            spawnPosition = new Vector2(x, y);
            attempts++;

            if (attempts > maxAttempts)
            {
                Debug.LogWarning("Could not find a valid spawn position after multiple attempts.");
                return Vector2.zero;
            }
        } while (cameraBounds.Contains(spawnPosition) || !IsPositionValid(spawnPosition));

        return spawnPosition;
    }

    private bool IsPositionValid(Vector2 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 2f); // Радиус проверяется на наличие коллайдеров
        foreach (Collider2D collider in colliders)
        {
            if (collider.isTrigger == false) // Игнорируем триггер-коллайдеры
            {
                return false;
            }
        }
        return true;
    }

}
