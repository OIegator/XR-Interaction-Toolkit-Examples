using UnityEngine;

public class CameraFade : MonoBehaviour
{
    public float speedScale = 1f;
    public Color fadeColor = Color.black;
    public AnimationCurve Curve = new AnimationCurve(new Keyframe(0, 1),
        new Keyframe(0.5f, 0.5f, -1.5f, -1.5f), new Keyframe(1, 0));
    public bool startFadedOut = false;

    private float alpha = 0f;
    private Texture2D texture;
    private int direction = 0;
    private float time = 0f;

    private System.Action onFadeComplete; // Callback function for when the fade is complete

    private void Start()
    {
        if (startFadedOut) alpha = 1f;
        else alpha = 0f;

        texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
        texture.Apply();
    }

    private void Update()
    {
        // Check for collision to initiate the fade
    }

    public void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object has a specific tag or condition
        if (collision.gameObject.CompareTag("Sheep"))
        {
            StartFade(EndGame); // Pass the EndGame function as the callback
        }
    }

    private void StartFade(System.Action onComplete)
    {
        if (alpha >= 1f) // Fully faded out
        {
            alpha = 1f;
            time = 0f;
            direction = 1;
        }
        else // Fully faded in
        {
            alpha = 0f;
            time = 1f;
            direction = -1;
        }

        onFadeComplete = onComplete; // Set the callback function
    }

    public void OnGUI()
    {
        if (alpha > 0f) GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
        if (direction != 0)
        {
            time += direction * Time.deltaTime * speedScale;
            alpha = Curve.Evaluate(time);
            texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
            texture.Apply();

            if (alpha <= 0f || alpha >= 1f)
            {
                direction = 0;

                // Invoke the callback function when the fade is complete
                if (onFadeComplete != null)
                {
                    onFadeComplete.Invoke();
                }
            }
        }
    }

    public void EndGame()
    {
        Debug.Log("Игра завершена!");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
