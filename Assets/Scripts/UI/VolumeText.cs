using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeText : MonoBehaviour
{
    [Header("�������� ��������� ����� � PlayerPrefs")]
    [SerializeField] private string volumeName;
    [Header("����� ������ ����")]
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

    // ������� ����������� ��������� ������
    // �� ������ ���� � ������������ ���������
    private void UpdateVolume()
    {
        // ��������� ��������� ������������� �������������
        float volumeValue = PlayerPrefs.GetFloat(volumeName) * 100;

        // ��������� ������ ������
        txt.text = textIntro + volumeValue.ToString();
    }
}
