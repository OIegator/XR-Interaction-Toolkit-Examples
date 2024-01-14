using UnityEngine;

public class EndGame : MonoBehaviour
{
    // Specify the tag of the objects you want to trigger the function
    public string targetTag = "Sheep";

    void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object has the specified tag
        if (collision.gameObject.CompareTag(targetTag))
        {
            // Call your function here
            HandleCollision();
        }
    }

    // Your custom function to be called upon collision
    void HandleCollision()
    {
        // Do something when the collision with the specified tag occurs
        Debug.Log("Collision with object of tag " + targetTag + " detected.");
    }
}