using System.Collections;
using UnityEngine;
using TMPro;

public class GoldManager : MonoBehaviour
{
    public float gold; //���
    public TMP_Text goldText; //��� ǥ�� UI
    public GameObject goldGainTextPrefab; //��� ���� �ؽ�Ʈ ������

    void Update()
    {
        goldText.text = gold.ToString();
    }

    // ��� ���� �ִϸ��̼� �޼���
    public void ShowGoldGain(float amount)
    {
        StartCoroutine(GoldGainAnimation(amount));
    }

    private IEnumerator GoldGainAnimation(float amount)
    {
        // ��� �ؽ�Ʈ �Ʒ��� ���� �ؽ�Ʈ�� ����
        Vector3 startPosition = goldText.transform.position + new Vector3(0, -40f, 0); // �ʱ� ��ġ�� ���� �� ����
        GameObject goldGainInstance = Instantiate(goldGainTextPrefab, startPosition, Quaternion.identity, goldText.transform);
        TMP_Text goldGainText = goldGainInstance.GetComponent<TMP_Text>();
        goldGainText.text = $"+{amount}";

        // �ؽ�Ʈ ���̵� �� �� �̵� �ִϸ��̼�
        Vector3 endPosition = startPosition + new Vector3(0, 15f, 0); // ���� ��ġ�� �������� ���� ����
        float duration = 1.0f;
        float elapsedTime = 0;
        Color color = goldGainText.color;
        color.a = 0;
        goldGainText.color = color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            // �ؽ�Ʈ ���̵� ��
            color.a = Mathf.Lerp(0, 1, t);
            goldGainText.color = color;

            // �ؽ�Ʈ ���� �̵�
            goldGainInstance.transform.position = Vector3.Lerp(startPosition, endPosition, t);

            yield return null;
        }

        // �ؽ�Ʈ�� ��� ������ �� ����
        yield return new WaitForSeconds(.2f);
        Destroy(goldGainInstance);
    }
}
