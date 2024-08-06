using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    public Button forwardButton;
    public Button backButton;

    public GameObject currentImage;
    public TextMeshProUGUI currentText;

    public List<GameObject> images;
    public List<string> texts;

    private int n = 0;
    private int limit;

    void Start()
    {
        limit = images.Count - 1;

        UpdateCurrentImageText();

        forwardButton.onClick.AddListener(OnForwardButtonClick);
        backButton.onClick.AddListener(OnBackButtonClick);

        UpdateButtonStates();
    }

    void UpdateButtonStates()
    {
        forwardButton.interactable = n < limit;
        backButton.interactable = n > 0;
    }

    void UpdateCurrentImageText()
    {
        for (int i = 0; i <= limit; i++)
        {
            images[i].SetActive(i == n);
        }
        currentText.text = texts[n];
    }

    public void OnForwardButtonClick()
    {
        if (n < limit)
        {
            n += 1;
            UpdateCurrentImageText();
            UpdateButtonStates();
        }
    }

    public void OnBackButtonClick()
    {
        if (n > 0)
        {
            n -= 1;
            UpdateCurrentImageText();
            UpdateButtonStates();
        }
    }
}
