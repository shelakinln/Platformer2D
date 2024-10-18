using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMenu : MonoBehaviour
{
    [Header("������ ������ ������� � ���� ������ ������")]
    [SerializeField] private GameObject[] levelButtons;
    private void OnEnable()
    {
        // ��������� ������ ������� � ����������� ��
        // ���������� �������� �������
        for (int i = 1; i < PlayerPrefs.GetInt("availableLevels"); i++)
        {
            levelButtons[i].SetActive(true);
        }
    }

    // �������� ������, ���������� � ����
    public void LoadScene(int _scene)
    {
        SceneManager.LoadScene( _scene );
        Time.timeScale = 1.0f;
    }
}
