using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ChangeDialog : MonoBehaviour
{
    public TMP_Text textField; // Поле для текста в интерфейсе (используем TMP_Text для TextMeshPro)
    public string[] textArray; // Массив текстов, которые будут меняться
    private int currentIndex = 0; // Текущий индекс в массиве текстов

    // Добавляем новый массив для сообщений, связанных с триггером
    public string[] powerTriggerTextArray;
    public string[] tpTriggerTextArray;

    void Start()
    {
        // Находим объект TMP_Text внутри этого GameObject, если он не был установлен
        if (textField == null)
            textField = GetComponentInChildren<TMP_Text>();

        // Проверка, чтобы убедиться, что массив текстов не пуст
        if (textArray.Length == 0)
        {
            Debug.LogError("Text array is empty. Please assign some texts to the array.");
            enabled = false; // Отключаем скрипт, так как нет текстов для отображения
        }

        // Устанавливаем начальный текст
        UpdateTextField();
    }

    void UpdateTextField()
    {
        // Проверяем, есть ли еще тексты в текущем массиве
        if (currentIndex < textArray.Length)
        {
            // Обновляем текстовое поле с текущим текстом из массива
            textField.text = textArray[currentIndex];
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void ChangeTextOnClick()
    {
        // При нажатии кнопки переходим к следующему тексту в массиве
        currentIndex = (currentIndex + 1);
        UpdateTextField();
    }

    // Метод для обновления текстового поля при событии триггера
    public void TriggerPowerEvent()
    {
        // Активируем текстовое поле, если у нас есть новые тексты для отображения
        if (powerTriggerTextArray.Length > 0)
        {
            gameObject.SetActive(true);
            // Устанавливаем новый массив текстов для отображения
            textArray = powerTriggerTextArray;
            currentIndex = 0;
            UpdateTextField();
        }
    }

    public void TriggerTpEvent()
    {
        // Активируем текстовое поле, если у нас есть новые тексты для отображения
        if (tpTriggerTextArray.Length > 0)
        {
            gameObject.SetActive(true);
            // Устанавливаем новый массив текстов для отображения
            textArray = tpTriggerTextArray;
            currentIndex = 0;
            UpdateTextField();
        }
    }
}