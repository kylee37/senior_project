using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FameManager : MonoBehaviour
{
    public static FameManager instance;

    // �� ������ �ʱ� ��ȣ���� ������ ����
    public int _sumHuman;   // 10
    public int _sumDwarf;   // 10
    public int _sumElf;     // 20

    // ������Ƽ �̿��Ͽ� �ܺ� ��ũ��Ʈ�� ���� ������ ������Ʈ �� �� �ְ�.
    public int sumHuman
    {
        get { return _sumHuman; }
        set { _sumHuman = value; }
    }

    public int sumDwarf
    {
        get { return _sumDwarf; }
        set { _sumDwarf = value; }
    }

    public int sumElf
    {
        get { return _sumElf; }
        set { _sumElf = value; }
    }

    public int visitation = 0;

    // �̱���
    private void Awake()
    {
        if (FameManager.instance == null)
        {
            FameManager.instance = this;
        }
    }

    void Start()
    {
        // �ʱ� ��ȣ���� ���
        CalculateInitialPreferences();
    }

    // �ʱ� ��ȣ�� ��� �޼���
    private void CalculateInitialPreferences()
    {
        var sum = sumHuman + sumDwarf + sumElf;
        var totalPreference = visitation + (sumHuman + sumDwarf + sumElf) / 10;
        sumHuman = sumHuman * (totalPreference / (sum / 10)) / 10;
        sumDwarf = sumDwarf * (totalPreference / (sum / 10)) / 10;
        sumElf = sumElf * (totalPreference / (sum / 10)) / 10;
    }
}