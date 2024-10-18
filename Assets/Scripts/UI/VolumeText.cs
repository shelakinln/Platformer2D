using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeText : MonoBehaviour
{
    [Header("Название источника звука в PlayerPrefs")]
    [SerializeField] private string volumeName;
    [Header("Текст кнопки меню")]
    [SerializeField] private string textIntro;

    private Text txt;

    private void Awake()
    {
        txt = GetComponent<Text>();
    }

    private void Update()
    {
        UpdateVolume();
    }

    // Функция реализующая изменение текста
    // на пункте меню с регулировкой громкости
    private void UpdateVolume()
    {
        // Последняя громкость установленная пользователем
        float volumeValue = PlayerPrefs.GetFloat(volumeName) * 100;

        // Изменение самого текста
        txt.text = textIntro + volumeValue.ToString();
    }
}
