using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldManager : MonoBehaviour
{
    public float gold; //°ñµå
    public TMP_Text goldText; //°ñµå Ç¥½Ã UI

    void Update()
    {
        goldText.text = gold.ToString();
    }
}
