using UnityEngine;

public class SlimeController : MonoBehaviour
{
    [Header("Slime Stats")]
    [SerializeField] private int _size = 1;
    [SerializeField] private int _exp;

    [Header("Growth")]
    [SerializeField] private float _scalePerSize = 0.1f;

    [Header("Visual")]
    [SerializeField] private Transform _visual;

    public int Size => _size;

    private void Start()
    {
        UpdateScale();
    }

    public void Grow(int value)
    {
        _exp += value;

        int newSize = 1 + _exp / 5;

        if (newSize != _size)
        {
            _size = newSize;
            UpdateScale();
        }
    }

    private void UpdateScale()
    {
        float scale = 1f + (_size - 1) * _scalePerSize;
        _visual.localScale = Vector3.one * scale;
    }
}