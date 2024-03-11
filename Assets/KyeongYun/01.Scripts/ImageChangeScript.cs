using UnityEngine;
using UnityEngine.UI;

public class ImageChangeScript : MonoBehaviour
{
    public Sprite newSprite;
    private Image imageComponent;
    private Sprite originalSprite; // ���� �̹����� �����ϱ� ���� ����
    private bool isKeyPressed = false;

    void Start()
    {
        imageComponent = GetComponent<Image>();
        originalSprite = imageComponent.sprite;

        if (imageComponent == null)
        {
            Debug.LogError("�� ��ũ��Ʈ�� ����Ϸ��� Image ������Ʈ�� ���� ������Ʈ�� �߰��Ǿ�� �մϴ�.");
        }
    }

    void Update()
    {
        // ���� Shift Ű�� ������ �� �̹����� �����ϰ� isKeyPressed�� true�� ����
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ChangeImage();
            isKeyPressed = true;
        }

        // ���� Shift Ű�� ������ �� isKeyPressed�� false�� �����ϰ� �̹����� ���� �̹����� ����
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isKeyPressed = false;
            RestoreOriginalImage();
        }
    }

    void ChangeImage()
    {
        if (imageComponent != null && newSprite != null)
        {
            imageComponent.sprite = newSprite;
        }
    }

    void RestoreOriginalImage()
    {
        if (imageComponent != null)
        {
            imageComponent.sprite = originalSprite;
        }
    }
}
