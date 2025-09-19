using System;
using UnityEngine;

[System.Serializable]
public class tujian
{
   public string name;
   public float growMode;
   public float canGetSeed;
}
    
[System.Serializable]
public class FruitCfg
{
   public tujian[] tujian;
}
    