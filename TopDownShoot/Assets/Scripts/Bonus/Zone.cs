using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    public float zoneRaduis;//радиус
    public int zoneCount;//количество

    public bool isDeathZone = false;
    public bool isSlowZone = false;
    private float slowFactor = 0.6f;
    

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            PlayerController player = collider.gameObject.GetComponent<PlayerController>();
            if (isDeathZone)
            {
                player.Die();
            }
            else if (isSlowZone)
            {
                player.moveSpeed *= slowFactor;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (isSlowZone)
            {
                player.moveSpeed /= slowFactor;
            }
        }
    }
}
