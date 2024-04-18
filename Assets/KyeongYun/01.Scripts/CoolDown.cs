using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolDown : MonoBehaviour
{
    private float currentCooldown = 10f;    // ���� ���� �ð�
    private float cooldown = 10f;           // ��ü ��Ÿ��
    public Image disable;                   // ���� �ð��� ǥ�� �� �̹���

    private void Start()
    {
        StartCoroutine(CooldownFunc());
    }

    public IEnumerator CooldownFunc()
    {
        while(currentCooldown > 0.0f)
        {
            currentCooldown -= Time.fixedDeltaTime;

            // ��Ÿ�� ���� ������ �̹���
            disable.fillAmount = currentCooldown / cooldown;
            
            yield return new WaitForFixedUpdate();
        }
    }
}
