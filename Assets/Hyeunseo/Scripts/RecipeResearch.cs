using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeResearch : MonoBehaviour
{
    public int recipeCode; //������ �ڵ�
    public int price; //����
    int xp; //���� xp
    public GameObject textUi; //ǥ�� UI
    public TMP_Text printText; //ǥ�� UI �ؽ�Ʈ
    private bool research = false; //�����ߴ��� ���ߴ��� üũ�ϴ� �� ����

    public XPManager xpManager; //xp�Ŵ��� ��ũ��Ʈ
    public MenuUnlocked menuUnlocked; //�޴� �ر� ��ũ��Ʈ

    void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnClick);
        }
    }

    void Update()
    {
        xp = xpManager.xp; //xp�Ŵ������� xp �����ϴ� ���� ������
    }

    void OnClick()
    {
        if (!research) //���� �� ���� ���ٸ�
        {
            if (price <= xp) //�ʿ�xp���� xp�� ũ�ų� ������
            {
                xpManager.xp -= price; //���� xp���� �ʿ�xp��ŭ�� xp�� ��
                research = true;
                menuUnlocked.HandleRecipeCode(recipeCode); //�޴� �ر� ��ũ��Ʈ�� �� ����

                printText.text = "������ ���� ����!"; //ǥ�� UI�� ���� �ֱ�
            }
            else if (price > xp) //�ʿ�xp���� xp�� ������
            {
                printText.text = "������ �����մϴ�."; //ǥ�� UI�� ���� �ֱ�
            }
        }
        else if(research)
        {
            printText.text = "�̹� �����Ͽ����ϴ�."; //ǥ�� UI�� ���� �ֱ�
        }
        textUi.SetActive(true); //ǥ�� UI Ȱ��ȭ
        Invoke("CloseUI", 0.5f); //0.5�� �� UI ����
    }

    void CloseUI()
    {
        // UI�� ��Ȱ��ȭ
        if (textUi != null)
        {
            textUi.SetActive(false);
        }
    }
}
