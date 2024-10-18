using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Пересечение коллизий игрока и триггера перехода на новый уровень
        if(collision.transform.tag == "Player")
        {
            // Запоминание количества доступных уровней
            PlayerPrefs.SetInt("availableLevels", SceneManager.GetActiveScene().buildIndex + 1);

            // Загрузка сцены следующего уровня
            panel.SetActive(true);
        }
    }

    private void Next()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
