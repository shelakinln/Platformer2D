using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMenu : MonoBehaviour
{
    [Header("ћассив кнопок уровней в меню выбора уровн€")]
    [SerializeField] private GameObject[] levelButtons;
    private void OnEnable()
    {
        // јктиваци€ кнопок уровней в зависимости от
        // количества открытых уровней
        for (int i = 1; i < PlayerPrefs.GetInt("availableLevels"); i++)
        {
            levelButtons[i].SetActive(true);
        }
    }

    // «агрузка уровн€, выбранного в меню
    public void LoadScene(int _scene)
    {
        SceneManager.LoadScene( _scene );
        Time.timeScale = 1.0f;
    }
}
