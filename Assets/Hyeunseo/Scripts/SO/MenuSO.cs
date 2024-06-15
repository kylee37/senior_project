using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Menu
{
    public int num;
    public string name;
    public string explanation;
    public int price;
}

[CreateAssetMenu(fileName = "MenuSO", menuName = "Scriptable Object/MenuSO")]
public class MenuSO : ScriptableObject
{
    public Menu[] menus;
}