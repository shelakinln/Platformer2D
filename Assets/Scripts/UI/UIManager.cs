using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("���� ����� ����")]
    [SerializeField] private GameObject gameOverScreen;
    [Header("���� ������")]
    [SerializeField] private AudioClip gameOverSound;
    [Header("���� �����")]
    [SerializeField] private GameObject pauseScreen;
    [Header("����������� �������� ������")]
    [SerializeField] private GameObject healthBar;

    private void Awake()
    {
        if(gameOverScreen != null) gameOverScreen.SetActive(false);
        if(pauseScreen != null) pauseScreen.SetActive(false);
    }

    private void Update()
    {
        // ��������� / ����������� ���� �����
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

    // ��������� ���� ����� ����
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        healthBar.SetActive(false);
        Time.timeScale = 0;

        // ��������������� ����� ������ ���������
        SoundManager.instance.PlaySound(gameOverSound);
    }

    // ������������ ������ ������
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        healthBar.SetActive(true);
    }

    // ����� � ������� ����
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    // ����� �� �������� ����������
    public void Quit()
    {
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    // �����
    public void PauseGame(bool _status)
    {
        pauseScreen.SetActive(_status);
        healthBar.SetActive(!_status);
        if (_status) Time.timeScale = 0;
        else Time.timeScale = 1;
    }

    // ��������� ��������� ������ �� 20%
    public void SoundVolume()
    {
        SoundManager.instance.ChangeSoundVolume(0.2f);
    }

    // ��������� ��������� ������ �� 20%
    public void MusicVolume()
    {
        SoundManager.instance.ChangeMusicVolume(0.2f);
    }
}
