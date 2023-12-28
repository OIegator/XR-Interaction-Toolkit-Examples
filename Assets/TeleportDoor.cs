using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportDoor : MonoBehaviour
{
    public Transform teleportDestination; // Место, куда игрок будет перемещен
    public GameObject objectToHide;
    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, является ли объект, вошедший в триггер, игроком
        if (other.CompareTag("Player"))
        {
            // Вызываем функцию перемещения игрока
            TeleportPlayer(other.transform);
            HideObject();
        }
    }

    private void HideObject()
    {
        // Проверяем, что объект для скрытия был назначен
        if (objectToHide != null)
        {
            // Выключаем видимость объекта
            objectToHide.SetActive(false);
        }
    }
    private void TeleportPlayer(Transform playerTransform)
    {
        // Перемещаем игрока в указанное место
        playerTransform.parent.transform.position = teleportDestination.position;
        playerTransform.transform.localPosition = Vector3.zero;
        playerTransform.rotation = teleportDestination.rotation;
    }


}