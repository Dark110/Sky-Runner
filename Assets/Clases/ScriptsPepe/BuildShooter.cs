using System.Collections.Generic;
using UnityEngine;

public class BuildShooter : MonoBehaviour
{
    public GameObject bullerPrefab;
    List<GameObject> bullets = new List<GameObject>();
    void CreateBullet()
    {
        for(int i = 0; i < bullets.Count; i++)
        {
            if (!bullets[i].activeInHierarchy)
            {
                bullets[i].transform.position = transform.position;
                bullets[i].transform.rotation = transform.rotation;
                bullets[i].SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(bullerPrefab, transform.position, transform.rotation);
        }
    }
}
