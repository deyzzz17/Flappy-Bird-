using UnityEngine;

/// <summary>
/// Simple oscillating vertical movement with screen boundary safety
/// </summary>
public class PingPongMovement : MonoBehaviour
{
    [Header("Paramètres de Mouvement")]
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _travelDistance = 1.5f;

    [Header("Sécurité (Ne pas sortir de l'écran)")]
    [SerializeField] private float _screenTop = 2.3f;
    [SerializeField] private float _screenBottom = -2.3f;

    private float _minY;
    private float _maxY;
    private int _direction = 1;

    private void Start()
    {
        float startY = transform.position.y;
        // Calculate clamped boundaries based on initial position and travel distance
        float targetMax = startY + _travelDistance;
        float targetMin = startY - _travelDistance;
        _maxY = Mathf.Min(targetMax, _screenTop);
        _minY = Mathf.Max(targetMin, _screenBottom);
        // Randomize initial direction for variety
        _direction = Random.Range(0, 2) == 0 ? 1 : -1;
    }

    private void Update()
    {
        transform.Translate(Vector3.up * _speed * _direction * Time.deltaTime);
        if (transform.position.y >= _maxY)
        {
            _direction = -1;
            SetY(_maxY);
        }
        else if (transform.position.y <= _minY)
        {
            _direction = 1;
            SetY(_minY);
        }
    }

    private void SetY(float y)
    {
        Vector3 newPos = transform.position;
        newPos.y = y;
        transform.position = newPos;
    }
}