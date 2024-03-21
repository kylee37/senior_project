using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUnlocked : MonoBehaviour
{
    public List<GameObject> uiButtons = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < uiButtons.Count; i++)
        {
            uiButtons[i].SetActive(false);
        }
    }

    public void HandleRecipeCode(int recipeCode)
    {
        uiButtons[recipeCode-1].SetActive(true);
    }
}
