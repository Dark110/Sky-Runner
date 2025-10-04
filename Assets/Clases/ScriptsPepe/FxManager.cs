using System.Collections.Generic;
using UnityEngine;
using System;

public class FxManager : MonoBehaviour
{
    public FX_pool[] fX_Pools;

    private static FxManager _instance;

    public static FxManager GetInstance()
    {
        return _instance;
    }

    private void Awake()
    {
        _instance = this;

        // Inicializamos las pools
        for (int i = 0; i < fX_Pools.Length; i++)
        {
            fX_Pools[i].Init(transform);
        }
    }

    public void SetFx(FX_TYPE _fxType)
    {
        for (int i = 0; i < fX_Pools.Length; ++i)
        {
            if (fX_Pools[i].poolType == _fxType)
            {
                CreateFX(i); // aquí usamos el método corregido
            }
        }
    }

    public void SetFx(FX_TYPE _fxType, Vector3 _pos, Quaternion _rot)
    {
        for (int i = 0; i < fX_Pools.Length; ++i)
        {
            if (fX_Pools[i].poolType == _fxType)
            {
                GameObject fx = CreateFX(i); // creamos o reusamos
                fx.transform.position = _pos;
                fx.transform.rotation = _rot;
                fx.SetActive(true);
            }
        }
    }

    // 🔥 Método que faltaba
    private GameObject CreateFX(int index)
    {
        return fX_Pools[index].GetFx();
    }
}

[Serializable]
public class FX_pool
{
    public FX_TYPE poolType;
    public GameObject bulletPrefab;
    public int poolSize = 10;

    private List<GameObject> bullets = new List<GameObject>();

    // Inicializamos el pool
    public void Init(Transform parent)
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = GameObject.Instantiate(bulletPrefab, parent);
            obj.SetActive(false);
            bullets.Add(obj);
        }
    }

    // Obtenemos un FX disponible
    public GameObject GetFx()
    {
        foreach (GameObject obj in bullets)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // Si no hay disponibles, instanciamos uno nuevo
        GameObject newObj = GameObject.Instantiate(bulletPrefab);
        newObj.SetActive(true);
        bullets.Add(newObj);
        return newObj;
    }
}

public enum FX_TYPE
{
    Bullet,
    Enemy,
    Speed,
}