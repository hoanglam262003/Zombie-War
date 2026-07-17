using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance;

    [SerializeField] private GameObject container;

    private void Awake()
    {
        Instance = this;

        container.SetActive(false);
    }

    public void Show()
    {
        container.SetActive(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(SceneIndex.MainMenu);
    }
}