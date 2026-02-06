using UnityEngine;

/// <summary>
/// Manages player death conditions based on physical collisions or screen boundaries
/// </summary>
public class BirdCollision : MonoBehaviour
{
    private const float TOP_BOUND = 5f;
    private const float BOTTOM_BOUND = -5f;

    // Update is called once per frame
    private void Update()
    {
        // Out of bounds check
        if (transform.position.y > TOP_BOUND || transform.position.y < BOTTOM_BOUND)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Die();
        }
    }

    /// <summary>
    /// Disables player controls and physics before triggering the global Game Over state
    /// </summary>
    private void Die()
    {
        // Disable movement to prevent further jumps
        BirdMover mover = GetComponent<BirdMover>();
        if (mover != null) mover.enabled = false;
        // Disable collider to prevent multiple death triggers
        GetComponent<Collider2D>().enabled = false;
        if (GameManager.Instance != null) 
        {
            GameManager.Instance.TriggerGameOver();
        }
        this.enabled = false;
    }
}
