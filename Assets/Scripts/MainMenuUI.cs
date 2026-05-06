using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public Button startButton;

    private void OnEnable()
    {
        if (startButton != null)
            startButton.onClick.AddListener(LoadMainScene);
    }

    private void OnDisable()
    {
        if (startButton != null)
            startButton.onClick.RemoveListener(LoadMainScene);
    }

    private void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
