using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Exit : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, является ли объект, вошедший в триггер, игроком
        if (other.CompareTag("Player"))
        {
            // Вызываем функцию завершения игры
            EndGame();
        }
    }
    
    public void EndGame()
    {
        Debug.Log("Игра завершена!");
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }
}
