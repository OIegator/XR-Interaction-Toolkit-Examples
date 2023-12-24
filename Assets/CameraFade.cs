using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class CameraFade : MonoBehaviour
{
    public float speedScale = 1f;
    public Color fadeColor = Color.magenta; // Розовый цвет
    public AnimationCurve Curve = new AnimationCurve(new Keyframe(0, 1),
        new Keyframe(0.5f, 0.5f, -1.5f, -1.5f), new Keyframe(1, 0));
    public bool startFadedOut = false;

    private float alpha = 0f;
    private Texture2D texture;
    private int direction = 0;
    private float time = 0f;

    private void Start()
    {
        if (startFadedOut) alpha = 1f; else alpha = 0f;
        texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
        texture.Apply();
    }
    
    public void FadeOutAndEndGame()
    {
        // Начинаем затенение
        FadeOut(() =>
        {
            // Callback: Завершаем игру после затенения
            EndGame();
        });
    }

    public void FadeOut(System.Action callback)
    {
        // Начинаем затенение
        StartCoroutine(FadeOutRoutine(callback));
    }

    private IEnumerator FadeOutRoutine(System.Action callback)
    {
        if (alpha >= 1f) // Полностью затенено
        {
            alpha = 1f;
            time = 0f;
            direction = 1;
        }

        while (direction != 0)
        {
            time += direction * Time.deltaTime * speedScale;
            alpha = Curve.Evaluate(time);
            texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
            texture.Apply();
            if (alpha <= 0f || alpha >= 1f)
            {
                direction = 0;
                if (alpha >= 1f)
                {
                    // Если затенение завершено (полностью затенено), вызываем колбек
                    if (callback != null)
                    {
                        callback.Invoke();
                    }
                }
            }

            yield return null;
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
