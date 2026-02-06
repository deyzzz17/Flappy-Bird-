using UnityEngine;

/// <summary>
/// Detects when the player passes through score-triggering zones
/// Acts as a bridge between the physics system and the GameManager's score logic
/// </summary>
public class BirdScorer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object is a designated scoring zone (e.g., between pipes)
        if (collision.CompareTag("ScoreZone"))
        {
            // Inform the GameManager to increment the global score
            GameManager.Instance.AddScore();
        }
    }
}
