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
    }

    public void SetFx(FX_TYPE _fxType)
    {
        for (int i = 0; i < fX_Pools.Length; ++i)
        {
            if (fX_Pools[i].poolType == _fxType)
            { 
                if (fX_Pools[i].poolType == _fxType)
                {
                    createFx(i);
                }
            }
        }
    }

    public void SetFx(FX_TYPE _fxType, Vector3 _pos, Quaternion _rot)
    {
        for (int i = 0; i < fX_Pools.Length; ++i)
        {
            if (fX_Pools[i].poolType == _fxType)
            {
                GameObject fx = CreateFX(i);
                fx.transform.position = _pos;
                fx.transform.rotation = _rot;
            }
        }
    }


}

[Serializable]

public class FX_pool
{
    public FX_TYPE poolType;
    public GameObject bullerPrefab;
    List<GameObject> bullets = new List<GameObject>();
}

public enum FX_TYPE
{
    Bullet,
    enemy,
    speed,
}