using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates a volume icon's sprite based on the current slider value
/// Provides visual feedback for different volume levels (e.g., Mute vs Full)
/// </summary>
public class VolumeSliderVisual : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider _slider;
    [SerializeField] private Image _backgroundImage;

    [Header("Sprites")]
    [Tooltip("List of sprites corresponding to slider integer values (0 to Max)")]
    [SerializeField] private Sprite[] _volumeSprites;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Safe component fetching
        if (_slider == null) _slider = GetComponent<Slider>();
        if (_backgroundImage == null) return;
        // Register listener and initialize the first frame
        _slider.onValueChanged.AddListener(UpdateVisual);
        UpdateVisual(_slider.value);
    }

    /// <summary>
    /// Swaps the icon sprite based on the rounded slider value
    /// </summary>
    private void UpdateVisual(float value)
    {
        int index = Mathf.RoundToInt(value);
        // Ensure index stays within array bounds
        if (index >= 0 && index < _volumeSprites.Length)
        {
            _backgroundImage.sprite = _volumeSprites[index];
        }
    }
}
