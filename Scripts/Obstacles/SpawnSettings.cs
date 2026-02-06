using UnityEngine;

public class SpawnSettings : MonoBehaviour
{
    [Header("Pipes Moves")]
    [Tooltip("Defines how much the obstacle can move up or down from its spawn center")]
    [Range(0f, 3f)]
    public float MovementAmplitude = 1.0f;
}
