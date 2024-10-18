using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ����������� �������� ������ � �������� �������� �� ����� �������
        if(collision.transform.tag == "Player")
        {
            // ����������� ���������� ��������� �������
            PlayerPrefs.SetInt("availableLevels", SceneManager.GetActiveScene().buildIndex + 1);

            // �������� ����� ���������� ������
            panel.SetActive(true);
        }
    }

    private void Next()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
