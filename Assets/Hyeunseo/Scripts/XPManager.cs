using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class XPManager : MonoBehaviour
{
    public float xp = 0; //xp
    public TMP_Text xpText; //xp ǥ�� UI

    void Start()
    {
        
    }

    void Update()
    {
        xpText.text = xp.ToString();
    }
}
