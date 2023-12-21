using UnityEngine;

public class Cabin : MonoBehaviour
{
    // Угловая скорость вращения объекта
    public float rotationSpeed = 30.0f;
    public UnityEngine.XR.Content.Interaction.XRKnob xrKnob;
    // Метод, вызываемый при изменении значения руля
    private void Start()
    {
        // Подписываемся на событие изменения значения руля
        xrKnob.onValueChange.AddListener(RotateObject);
    }
    public void RotateObject(float knobValue)
    {
        // Задаем угол вращения объекта в зависимости от значения руля
        float targetRotation = Mathf.Lerp(-90.0f, 90.0f, knobValue);
        Debug.Log(knobValue);

        // Плавно вращаем объект
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0.0f, targetRotation, 0.0f), Time.deltaTime * rotationSpeed);
    }
    
    private void OnDestroy()
    {
        // Важно отписываться от события при уничтожении объекта или компонента
        xrKnob.onValueChange.RemoveListener(RotateObject);
    }
}
