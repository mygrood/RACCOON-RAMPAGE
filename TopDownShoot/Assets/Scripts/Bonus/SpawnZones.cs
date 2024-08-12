using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZones : MonoBehaviour
{

    public GameObject[] zones;
    private Bounds mapBounds;

    public float minimumDistance = 3f;
    private List<Vector2> zonePositions = new List<Vector2>();

    void Start()
    {
        mapBounds = GetComponent<SpriteRenderer>().bounds;
        GenerateZones();
    }

    void GenerateZones()
    {
        foreach (var zonePrefab in zones) //цикл по всем зонам
        {
            Zone zoneType = zonePrefab.GetComponent<Zone>();
            Debug.Log("Zonedeath " + zoneType.isDeathZone);
            if (zoneType != null)
            {
                for (int i = 0; i < zoneType.zoneCount; i++) 
                {   
                    //выбираем новую точку спавна, пока не будет подходящей
                    Vector2 position;
                    bool validPositionFound = false;                    
                    do 
                    {
                        position = GetRandomPosition(zoneType.zoneRaduis);
                        if (IsValidPosition(position, zoneType.zoneRaduis))
                        {
                            validPositionFound = true;
                        }
                    } while (!validPositionFound);

                    GameObject zone = Instantiate(zonePrefab, position, Quaternion.identity);                    
                    zonePositions.Add(position);
                    
                }
            }
        }
    }

    Vector2 GetRandomPosition(float radius)
    {
        float xMin = mapBounds.min.x + radius + minimumDistance;
        float xMax = mapBounds.max.x - radius - minimumDistance;
        float yMin = mapBounds.min.y + radius + minimumDistance;
        float yMax = mapBounds.max.y - radius - minimumDistance;

        float x = Random.Range(xMin, xMax);
        float y = Random.Range(yMin, yMax);

        return new Vector2(x, y);
    }

    bool IsValidPosition(Vector2 position, float radius)
    {
        foreach (Vector2 existingPosition in zonePositions)
        {
            if (Vector2.Distance(position, existingPosition) < (radius * 2 + minimumDistance))
            {
                return false;
            }
        }
        return true;
    }
}
   

