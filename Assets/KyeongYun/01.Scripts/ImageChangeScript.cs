using UnityEngine;
using UnityEngine.UI;

public class ImageChangeScript : MonoBehaviour
{
    public Sprite newSprite;
    private Image imageComponent;
    private Sprite originalSprite; // 원래 이미지를 저장하기 위한 변수
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
        // 왼쪽 Shift 키가 눌렸을 때 이미지를 변경하고 isKeyPressed를 true로 설정
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ChangeImage();
            isKeyPressed = true;
        }

        // 왼쪽 Shift 키를 떼었을 때 isKeyPressed를 false로 설정하고 이미지를 원래 이미지로 복원
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
