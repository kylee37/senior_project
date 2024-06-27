using System.Collections;
using UnityEngine;
using TMPro;

public class GoldManager : MonoBehaviour
{
    public float gold; //골드
    public TMP_Text goldText; //골드 표시 UI
    public GameObject goldGainTextPrefab; //골드 증가 텍스트 프리팹

    void Update()
    {
        goldText.text = gold.ToString();
    }

    // 골드 증가 애니메이션 메서드
    public void ShowGoldGain(float amount)
    {
        StartCoroutine(GoldGainAnimation(amount));
    }

    private IEnumerator GoldGainAnimation(float amount)
    {
        // 골드 텍스트 아래에 증가 텍스트를 생성
        Vector3 startPosition = goldText.transform.position + new Vector3(0, -40f, 0); // 초기 위치를 조금 더 낮춤
        GameObject goldGainInstance = Instantiate(goldGainTextPrefab, startPosition, Quaternion.identity, goldText.transform);
        TMP_Text goldGainText = goldGainInstance.GetComponent<TMP_Text>();
        goldGainText.text = $"+{amount}";

        // 텍스트 페이드 인 및 이동 애니메이션
        Vector3 endPosition = startPosition + new Vector3(0, 15f, 0); // 최종 위치는 이전보다 높게 설정
        float duration = 1.0f;
        float elapsedTime = 0;
        Color color = goldGainText.color;
        color.a = 0;
        goldGainText.color = color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            // 텍스트 페이드 인
            color.a = Mathf.Lerp(0, 1, t);
            goldGainText.color = color;

            // 텍스트 위로 이동
            goldGainInstance.transform.position = Vector3.Lerp(startPosition, endPosition, t);

            yield return null;
        }

        // 텍스트를 잠시 유지한 후 제거
        yield return new WaitForSeconds(.2f);
        Destroy(goldGainInstance);
    }
}
