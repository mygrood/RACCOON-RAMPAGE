using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnBonus : MonoBehaviour
{
    public GameObject[] weaponBonusPrefabs; //������ ������
    public float spawnWeaponInterval = 10f;

    public GameObject[] powerBonusPrefabs; //������ ��������   
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
        Weapon currentWeapon = shootingController.currentWeapon;//��� ������ � ������


        //���������� ������ �� ��� ���, ���� �� �������� ������������ �� ��������
        GameObject bonusPrefab;
        do
        {
            bonusPrefab = weaponBonusPrefabs[Random.Range(0, weaponBonusPrefabs.Length - 1)];
        }
        while (bonusPrefab.GetComponent<BonusWeapon>().weapon == currentWeapon);

        Vector2 spawnPosition = GetRandomSpawnPosition(); // �������� ��������� �������
        if (spawnPosition != Vector2.zero)
        {
            Instantiate(bonusPrefab, spawnPosition, Quaternion.identity); //�����
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

        bonusPrefab = powerBonusPrefabs[Random.Range(0, powerBonusPrefabs.Length - 1)]; //����� ���������� ������

        Vector2 spawnPosition = GetRandomSpawnPosition();
        Instantiate(bonusPrefab, spawnPosition, Quaternion.identity);   

    }
    //����� ������ ������
    /*
     private Vector2 GetRandomSpawnPosition()
    {
        //������� ��������� ������        
        float cameraHeight = 2f * cam.orthographicSize;
        float cameraWidth = cameraHeight * cam.aspect;
        Vector2 cameraPosition = cam.transform.position;
        Rect cameraBounds = new Rect(cameraPosition.x - cameraWidth / 2, cameraPosition.y - cameraHeight / 2, cameraWidth, cameraHeight);

        //��������� ���������� � ������ ������
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
        {  // ���������� ��������� ������� �� ��������� ������
            float x = Random.Range(cameraBounds.xMin - bufferDistance, cameraBounds.xMax + bufferDistance);
            float y = Random.Range(cameraBounds.yMin - bufferDistance, cameraBounds.yMax + bufferDistance);

            // ��������� �������� ������� �� ������� ������
            if (x > cameraBounds.xMin && x < cameraBounds.xMax)
            {
                if (Random.value > 0.5f)
                    y = cameraBounds.yMax + bufferDistance; // ������
                else
                    y = cameraBounds.yMin - bufferDistance; // �����
            }
            else
            {
                if (Random.value > 0.5f)
                    x = cameraBounds.xMax + bufferDistance; // ������
                else
                    x = cameraBounds.xMin - bufferDistance; // �����
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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 2f); // ������ ����������� �� ������� �����������
        foreach (Collider2D collider in colliders)
        {
            if (collider.isTrigger == false) // ���������� �������-����������
            {
                return false;
            }
        }
        return true;
    }

}
