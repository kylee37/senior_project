using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Food
{
    public int num;
    public int human;
    public int elf;
    public int dwarf;
}

[CreateAssetMenu(fileName = "FoodSO", menuName = "Scriptable Object/FoodSO")]
public class FoodSO : ScriptableObject
{
    public Food[] foods;
}
