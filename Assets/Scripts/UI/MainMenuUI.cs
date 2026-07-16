using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(SceneIndex.GameScene);
    }

    public void Quit()
    {
        Application.Quit();
    }
}