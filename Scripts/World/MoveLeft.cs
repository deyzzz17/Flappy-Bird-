using UnityEngine;

/// <summary>
/// Moves the object to the left based on the global world speed
/// Handles self-destruction once the object is far off-screen
/// </summary>
public class MoveLeft : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Guard clause: stop movement if the game is over or hasn't started
        if (GameManager.Instance == null || !GameManager.Instance.IsGameActive) return;
        // Fetch the synchronized speed from the central manager
        float speed = WorldSpeedManager.Instance.CurrentSpeed;
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        // Simple pooling alternative: destroy the object when it's no longer visible
        if (transform.position.x < -15f)
        {
            Destroy(gameObject);
        }
    }
}
