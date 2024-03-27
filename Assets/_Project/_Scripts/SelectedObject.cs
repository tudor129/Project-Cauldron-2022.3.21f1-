using UnityEngine;
using UnityEngine.Serialization;

public class SelectedObject : MonoBehaviour
{
    [SerializeField] float _smoothingFactor = 10.0f;
    [SerializeField] float _targetOpacity = 0.5f;

    Renderer _renderer;
    Material _material;
    Color _originalColor;
    Color _targetColor;

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material;
        _originalColor = _material.color;
        _targetColor = new Color(_originalColor.r, _originalColor.g, _originalColor.b, _targetOpacity);
    }

    void Update()
    {
        if (gameObject.activeSelf)
        {
            Color currentColor = _material.color;
            _material.color = Color.Lerp(currentColor, _targetColor, Time.deltaTime * _smoothingFactor);
        }
        else
        {
            _material.color = _originalColor;
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
