using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Начало новой игры
    public void NewGame()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1.0f;
        PlayerPrefs.SetInt("availableLevels", 1);
    }

    // Выход из игрового приложения
    public void Quit()
    {
        Application.Quit();
    }
}
