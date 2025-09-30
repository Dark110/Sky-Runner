using System;
using System.Collections;
using UnityEngine;

public class FXManager : MonoBehaviour
{
public FX_Pool
   public void setFx()
    {

    }

[Serializable]




}


void createFx(int _indexofpool)
{
    for (int i = 0; i < fxPools[_indexofpool].pool.Count; i++) //checar si hay fx disponible

    {
       if (!fxPools[_indexofpool].pool[i].activeInHierarchy)
        {
            fxPools[_indexofpool].pool[i].SetActive(true);
            return;
        }
    
    
    }
}