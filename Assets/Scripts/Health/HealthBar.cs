using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Объект здоровья игрока")]
    [SerializeField] private Health playerHealth;
    [Header("Спрайт максимального здоровья (чёрный)")]
    [SerializeField] private Image totalHealthBar;
    [Header("Спрайт текущего здоровья (красный)")]
    [SerializeField] private Image currentHealthBar;
    [Header("Спрайт количества жизней")]
    [SerializeField] private Image livesBar;
    [Header("Компонент игрока")]
    [SerializeField] private PlayerMovement player;
    [Header("Спрайт пистолета")]
    [SerializeField] private Image boots;
    [Header("Спрайт дробовика")]
    [SerializeField] private Image shotgun;
    [Header("Спрайт винтовки")]
    [SerializeField] private Image rifle;

    // Значение максимального здоровья
    private float totalHealth;

    private void Start()
    {
        // Заполнение спрайта
        totalHealthBar.fillAmount = playerHealth.currentHealth;
        totalHealth = playerHealth.currentHealth;
        livesBar.fillAmount = 4;
        boots.enabled = player.isBootsReceiced;
        shotgun.enabled = player.isShotGunReceiced;
        rifle.enabled = player.isRifleReceiced;
    }

    private void Update()
    {
        // Изменение спрайта текущего здоровья
        // в зависимости от его значения
        currentHealthBar.fillAmount = playerHealth.currentHealth / totalHealth;
        livesBar.fillAmount = playerHealth.currentLives / 4;
        boots.enabled = player.isBootsReceiced;
        shotgun.enabled = player.isShotGunReceiced;
        rifle.enabled = player.isRifleReceiced;
    }
}
