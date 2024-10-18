using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Меню конца игры")]
    [SerializeField] private GameObject gameOverScreen;
    [Header("Звук смерти")]
    [SerializeField] private AudioClip gameOverSound;
    [Header("Меню паузы")]
    [SerializeField] private GameObject pauseScreen;
    [Header("Отображение здоровья игрока")]
    [SerializeField] private GameObject healthBar;

    private void Awake()
    {
        if(gameOverScreen != null) gameOverScreen.SetActive(false);
        if(pauseScreen != null) pauseScreen.SetActive(false);
    }

    private void Update()
    {
        // Активация / деактивация меню паузы
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseScreen.activeInHierarchy)
            {
                Time.timeScale = 1;
                PauseGame(false);
            }
            else
            {
                PauseGame(true);
                Time.timeScale = 0;
            }
        }
    }

    // Активация меню коцна игры
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        healthBar.SetActive(false);
        Time.timeScale = 0;

        // Воспроизведение звука смерти персонажа
        SoundManager.instance.PlaySound(gameOverSound);
    }

    // Перезагрузка уровня заново
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        healthBar.SetActive(true);
    }

    // Выход в главное меню
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    // Выход из игрового приложение
    public void Quit()
    {
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    // Пауза
    public void PauseGame(bool _status)
    {
        pauseScreen.SetActive(_status);
        healthBar.SetActive(!_status);
        if (_status) Time.timeScale = 0;
        else Time.timeScale = 1;
    }

    // Изменение громкости звуков на 20%
    public void SoundVolume()
    {
        SoundManager.instance.ChangeSoundVolume(0.2f);
    }

    // Изменение громкости музыки на 20%
    public void MusicVolume()
    {
        SoundManager.instance.ChangeMusicVolume(0.2f);
    }
}
