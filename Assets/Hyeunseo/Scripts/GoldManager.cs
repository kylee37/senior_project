using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldManager : MonoBehaviour
{
    public float gold = 0; //���
    public TMP_Text goldText; //��� ǥ�� UI

    void Update()
    {
        goldText.text = gold.ToString();
    }
}
