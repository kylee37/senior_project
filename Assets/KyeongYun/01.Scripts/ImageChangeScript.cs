using UnityEngine;
using UnityEngine.UI;

public class ImageChangeScript : MonoBehaviour
{
    public Sprite newSprite;
    private Image imageComponent;
    private Sprite originalSprite; // 원래 이미지를 저장하기 위한 변수
    private bool isKeyPressed = false;

    void Start()
    {
        imageComponent = GetComponent<Image>();
        originalSprite = imageComponent.sprite;

        if (imageComponent == null)
        {
            Debug.LogError("이 스크립트를 사용하려면 Image 컴포넌트가 게임 오브젝트에 추가되어야 합니다.");
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
