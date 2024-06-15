using System.Collections;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class DialogTest : MonoBehaviour
{
	[SerializeField]
	private	DialogSystem	dialogSystem01;
	[SerializeField]
	private	TextMeshProUGUI	textCountdown;
	[SerializeField]
	private	DialogSystem	dialogSystem02;

	private IEnumerator Start()
	{
		textCountdown.gameObject.SetActive(false);

		// ù ��° ��� �б� ����
		yield return new WaitUntil(()=>dialogSystem01.UpdateDialog());

		// ��� �б� ���̿� ���ϴ� �ൿ�� �߰��� �� �ִ�.
		// ĳ���͸� �����̰ų� �������� ȹ���ϴ� ����.. ����� 3-2-1 ī��Ʈ�ٿ� ����
		textCountdown.gameObject.SetActive(true);
		int count = 3;
		while ( count > 0 )
		{
			textCountdown.text = count.ToString();
			count --;

			yield return new WaitForSeconds(1);
		}
		textCountdown.gameObject.SetActive(false);

		// �� ��° ��� �б� ����
		yield return new WaitUntil(()=>dialogSystem02.UpdateDialog());
		textCountdown.gameObject.SetActive(true);
		textCountdown.text = "The End";

		// 2�� ��
		yield return new WaitForSeconds(2);

		// ����Ƽ ������ �÷��� ����
		UnityEditor.EditorApplication.ExitPlaymode();
	}
}

