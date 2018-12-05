using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonManager : MonoBehaviour {

    [SerializeField]
    private GameObject pauseMenu;
    private bool pause = false;
    public bool pauseAvailable;

    [SerializeField]
    public int checkpoint = 0;

    private void Start()
    {
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (!pauseAvailable) return;
        if (Input.GetKeyDown(KeyCode.Escape))
            if (pause)
                ResumeGame();
            else
                Pause();
    }

    public void ChangeScene(string scene)
    {
        switch (checkpoint) {
            case 1 :
                // TODO : change to new scene
                Debug.Log("Squalala");
                SceneManager.LoadScene(scene + "_check1");
                break;
            case 0:
            default:
                SceneManager.LoadScene(scene);
                break;
        } 
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
