using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleController : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            Destroy(gameObject);
            LevelManager levelManager = FindObjectOfType<LevelManager>();
            levelManager.LastLevel();

        }
    }

}
