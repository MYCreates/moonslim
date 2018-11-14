using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonManager : MonoBehaviour {

    [SerializeField]
    private GameObject pauseMenu;
    private bool pause = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            pause = pause ? ResumeGame() : Pause();
    }

    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public bool Pause()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        return true;
    }

    public bool ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        return false;
    }
}
