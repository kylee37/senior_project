using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FameManager : MonoBehaviour
{
    public static FameManager instance;

    // 각 종족별 초기 선호도를 저장할 변수
    public int _sumHuman;   // 10
    public int _sumDwarf;   // 10
    public int _sumElf;     // 20

    public Slider humanSlider;
    public Slider dwarfSlider;
    public Slider elfSlider;

    // 프로퍼티 이용하여 외부 스크립트에 의해 변수가 업데이트 될 수 있게.
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

    // 싱글톤
    private void Awake()
    {
        if (FameManager.instance == null)
        {
            FameManager.instance = this;
        }
    }

    void Start()
    {
        // 초기 선호도를 계산
        CalculateInitialPreferences();

        // 슬라이더 설정
        humanSlider.value = sumHuman;
        dwarfSlider.value = sumDwarf;
        elfSlider.value = sumElf;
    }

    private void Update()
    {
        humanSlider.value = sumHuman;
        dwarfSlider.value = sumDwarf;
        elfSlider.value = sumElf;
    }

    // 초기 선호도 계산 메서드
    private void CalculateInitialPreferences()
    {
        var sum = sumHuman + sumDwarf + sumElf;
        var totalPreference = visitation + (sumHuman + sumDwarf + sumElf) / 10;
        sumHuman = sumHuman * (totalPreference / (sum / 10)) / 10;
        sumDwarf = sumDwarf * (totalPreference / (sum / 10)) / 10;
        sumElf = sumElf * (totalPreference / (sum / 10)) / 10;
    }
}
