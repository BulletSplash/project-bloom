using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManage : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pauseMenu;
    private bool isPausing = true;

    private void Awake()
    {
        pauseMenu.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPausing || Input.GetKeyDown(KeyCode.Escape) && !isPausing)
        {
            OpenMenu();
        }
    }
    private void OpenMenu()
    {
        isPausing = !isPausing;

        if (isPausing)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        pauseMenu.SetActive(isPausing);
    }
    public void BackToMenu()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex - 1);
        Time.timeScale = 1;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
