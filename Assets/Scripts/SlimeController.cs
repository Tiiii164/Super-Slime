using System;
using UnityEngine;
using UnityEngine.Audio;

public class SlimeController : MonoBehaviour
{
    [Header("Slime Stats")]
    [SerializeField] private int _size = 1;
    [SerializeField] private int _exp;
    [SerializeField] private int[] levelThresholds = {30, 100, 300, 2000, 6000, 20000, 50000};
    [Header("Growth")]
    [SerializeField] private float _scalePerSize = 0.1f;

    [Header("Visual")]
    [SerializeField] private Transform _visual;

    [SerializeField] ParticleSystem eatParticle;
    [SerializeField] AudioSource eatSfx;
    [SerializeField] AudioSource levelUpSfx;
    public static event Action LevelUp;
    public int Size => _size;
    private Animator animator;



    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        UpdateScale();
    }

    private void OnEnable()
    {
        Food.OnFoodEaten += PlayParticle;
    }

    private void OnDisable()
    {
        Food.OnFoodEaten -= PlayParticle;
    }

    private void PlayParticle(Vector3 pos)
    {
        Vector3 offset = new Vector3(0, 1f, 0);
        eatParticle.transform.position = pos + offset;
        eatParticle.Play();
    }

    public void Eat(int value)
    {
        animator.SetTrigger("eat");
        eatSfx.Play();

        _exp += value;

        int newSize = 1 ;
        for (int i = 0; i < levelThresholds.Length; i++)
        {
            if (_exp >= levelThresholds[i])
            {
                newSize = i + 1;
            }
        }

        if (newSize != _size)
        {
            _size = newSize;
            UpdateScale();
            levelUpSfx.Play();
        }
    }

    private void UpdateScale()
    {
        float scale = 1f + (_size - 1) * _scalePerSize;
        eatParticle.transform.localScale = Vector3.one * scale;
        _visual.localScale = Vector3.one * scale;
        CameraFollowController.Instance.AdjustZoomBasedOnTargetSize();
        LevelUp?.Invoke();
    }
}