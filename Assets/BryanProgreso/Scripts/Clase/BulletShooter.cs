using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    List<GameObject> bullets = new List<GameObject>();
    void createbullet()
    { 
    for (int i = 0; i < 10; i++)
        {
            if (bullets[i].activeInHierarchy)
            // reutilizar la bala
            bullets[i].transform.position = transform.position;
            bullets[i].transform.rotation = transform.rotation;
            bullets[i].SetActive(true);
            return;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            createbullet();
        }
    }
}
