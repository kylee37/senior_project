using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class XPManager : MonoBehaviour
{
    public int xp; //xp
    public TMP_Text xpText; //xp Ç¥½Ã UI
    public LevelManager levelManager;

    void Update()
    {
        xpText.text = xp.ToString();

        if(levelManager.totalXP > 0)
        {
            xp += levelManager.totalXP;

            levelManager.totalXP = 0;
        }
    }
}
