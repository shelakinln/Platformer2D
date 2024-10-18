using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    [Header("Источник звуков игры")]
    private AudioSource soundSource;
    [Header("Источник музыки игры")]
    private AudioSource musicSource;

    private void Awake()
    {
        soundSource = GetComponent<AudioSource>();
        musicSource = transform.GetChild(0).GetComponent<AudioSource>();

        if (instance == null)
        {
            instance = this;
        }
    }

    // Однократное воспроизведение звука
    public void PlaySound(AudioClip _sound)
    {
        soundSource.PlayOneShot(_sound);
    }

    // Изменение громкости звуков игры
    public void ChangeSoundVolume(float _change)
    {
        ChangeSourceVolume(_change, "soundVolume", soundSource);
    }

    // Изменение громкости музыки игры
    public void ChangeMusicVolume(float _change)
    {
        ChangeSourceVolume(_change, "musicVolume", musicSource);
    }

    // Функция реализующая регулировку громкости
    private void ChangeSourceVolume(float _change, string _volumeName, 
        AudioSource _source)
    {
        // Изменённый уровень громкости
        float currentVolume = PlayerPrefs.GetFloat(_volumeName);
        currentVolume += _change;

        // Цикличность изменения громкости
        if (currentVolume > 1) currentVolume = 0;
        else if (currentVolume < 0) currentVolume = 1;

        // Изменение уровня громкости выбранного источника 
        _source.volume = currentVolume;

        // Запись текущей громкости в PlayerPrefs
        PlayerPrefs.SetFloat(_volumeName, currentVolume);
    }
}
