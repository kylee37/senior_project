using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Furniture
{
    public int num;
    public int human;
    public int elf;
    public int dwarf;
    public string name;
    public int price;
}

[CreateAssetMenu(fileName = "FurnitureSO", menuName = "Scriptable Object/FurnitureSO")]
public class FurnitureSO : ScriptableObject
{
    public Furniture[] furnitures;
}
