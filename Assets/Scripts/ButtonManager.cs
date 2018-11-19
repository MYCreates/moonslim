using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonManager : MonoBehaviour {

    [SerializeField]
    private GameObject pauseMenu;
    private bool pause = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (pause)
                ResumeGame();
            else
                Pause();
    }

    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pause = true;
        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pause = false;
        pauseMenu.SetActive(false);
    }
}
