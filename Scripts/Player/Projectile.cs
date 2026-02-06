using UnityEngine;

/// <summary>
/// Handles projectile movement, its limited lifespan, and interactions with game triggers
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _lifeTime = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        // Automatically clean up the projectile after a set duration to save resources
        Destroy(gameObject, _lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Balle a touché : {collision.gameObject.name} | Tag : {collision.tag}");
        // Ignore the player who shot the projectile and the scoring zones
        if (collision.CompareTag("Player") || collision.CompareTag("ScoreZone")) return;
        // Interaction with interactive buttons
        if (collision.CompareTag("Button"))
        {
            GateController gate = collision.GetComponentInParent<GateController>();
            if(gate != null)
            {
                gate.OpenGate();
            }
            Destroy(gameObject);
            return;
        }
        // Destroy on impact with any other solid obstacle
        Destroy(gameObject);
    }
}
