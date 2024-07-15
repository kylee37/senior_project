using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolDown : MonoBehaviour
{
    private float currentCooldown = 10f;    // 현재 남은 시간
    private float cooldown = 10f;           // 전체 쿨타임
    public Image disable;                   // 남은 시간을 표시 할 이미지

    private void Start()
    {
        StartCoroutine(CooldownFunc());
    }

    public IEnumerator CooldownFunc()
    {
        while(currentCooldown > 0.0f)
        {
            currentCooldown -= Time.fixedDeltaTime;

            // 쿨타임 동안 변경할 이미지
            disable.fillAmount = currentCooldown / cooldown;
            
            yield return new WaitForFixedUpdate();
        }
    }
}
