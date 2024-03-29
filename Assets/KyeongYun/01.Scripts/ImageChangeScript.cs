using UnityEngine;
using UnityEngine.UI;

public class ImageChangeScript : MonoBehaviour
{
    public Sprite newSprite;
    private Image imageComponent;
    private Sprite originalSprite; // ���� �̹����� �����ϱ� ���� ����
    [HideInInspector] 
    public bool isKeyPressed;

    void Start()
    {
        imageComponent = GetComponent<Image>();
        originalSprite = imageComponent.sprite;

        if (imageComponent == null)
        {
            Debug.LogError("imageComponent is NULL");
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
