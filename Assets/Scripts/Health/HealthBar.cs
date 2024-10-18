using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("������ �������� ������")]
    [SerializeField] private Health playerHealth;
    [Header("������ ������������� �������� (������)")]
    [SerializeField] private Image totalHealthBar;
    [Header("������ �������� �������� (�������)")]
    [SerializeField] private Image currentHealthBar;
    [Header("������ ���������� ������")]
    [SerializeField] private Image livesBar;
    [Header("��������� ������")]
    [SerializeField] private PlayerMovement player;
    [Header("������ ���������")]
    [SerializeField] private Image boots;
    [Header("������ ���������")]
    [SerializeField] private Image shotgun;
    [Header("������ ��������")]
    [SerializeField] private Image rifle;

    // �������� ������������� ��������
    private float totalHealth;

    private void Start()
    {
        // ���������� �������
        totalHealthBar.fillAmount = playerHealth.currentHealth;
        totalHealth = playerHealth.currentHealth;
        livesBar.fillAmount = 4;
        boots.enabled = player.isBootsReceiced;
        shotgun.enabled = player.isShotGunReceiced;
        rifle.enabled = player.isRifleReceiced;
    }

    private void Update()
    {
        // ��������� ������� �������� ��������
        // � ����������� �� ��� ��������
        currentHealthBar.fillAmount = playerHealth.currentHealth / totalHealth;
        livesBar.fillAmount = playerHealth.currentLives / 4;
        boots.enabled = player.isBootsReceiced;
        shotgun.enabled = player.isShotGunReceiced;
        rifle.enabled = player.isRifleReceiced;
    }
}
