using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    [Header("Пункты выбранного меню")]
    [SerializeField] private RectTransform[] options;
    [Header("Звук перемещения стрелочки")]
    [SerializeField] private AudioClip changeSound;
    [Header("Звук выбора")]
    [SerializeField] private AudioClip interactSound;

    // Объект стрелочки
    private RectTransform rect;

    // Текущая позиция стрелочки
    private int currentPosition;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        // Перемещение стрелочки по меню
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) ChangePosition(-1);
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) ChangePosition(1);

        // Выбор одного из пунктов меню
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.E)) Interact();
    }

    // Функция, реализующая перемещение стрелочки
    private void ChangePosition(int _change)
    {
        // Изменение индекса выбранного пункта меню
        currentPosition += _change;

        // Воспроизведение звука перемещения стрелочки
        if (_change != 0) SoundManager.instance.PlaySound(changeSound);

        // Цикличность перемещений между пунктами меню
        if (currentPosition < 0) currentPosition = options.Length - 1;
        else if(currentPosition > options.Length - 1) currentPosition = 0;

        // Перемещение объекта стрелочки на к текущему пункту меню
        rect.position = new Vector3(rect.position.x, options[currentPosition].position.y, 0);
    }

    // Функция, реализующая выбор пункта меню
    private void Interact()
    {
        // Воспроизведение звука выбора
        SoundManager.instance.PlaySound(interactSound);

        // Взаимодействие с компонентом кнопки выбранного пункта меню
        options[currentPosition].GetComponent<Button>().onClick.Invoke();
    }
}
