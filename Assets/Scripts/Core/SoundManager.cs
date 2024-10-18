using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    [Header("�������� ������ ����")]
    private AudioSource soundSource;
    [Header("�������� ������ ����")]
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

    // ����������� ��������������� �����
    public void PlaySound(AudioClip _sound)
    {
        soundSource.PlayOneShot(_sound);
    }

    // ��������� ��������� ������ ����
    public void ChangeSoundVolume(float _change)
    {
        ChangeSourceVolume(_change, "soundVolume", soundSource);
    }

    // ��������� ��������� ������ ����
    public void ChangeMusicVolume(float _change)
    {
        ChangeSourceVolume(_change, "musicVolume", musicSource);
    }

    // ������� ����������� ����������� ���������
    private void ChangeSourceVolume(float _change, string _volumeName, 
        AudioSource _source)
    {
        // ��������� ������� ���������
        float currentVolume = PlayerPrefs.GetFloat(_volumeName);
        currentVolume += _change;

        // ����������� ��������� ���������
        if (currentVolume > 1) currentVolume = 0;
        else if (currentVolume < 0) currentVolume = 1;

        // ��������� ������ ��������� ���������� ��������� 
        _source.volume = currentVolume;

        // ������ ������� ��������� � PlayerPrefs
        PlayerPrefs.SetFloat(_volumeName, currentVolume);
    }
}
