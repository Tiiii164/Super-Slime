using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlimeController : MonoBehaviour
{
    [Header("Slime Stats")]
    [SerializeField] private int _size = 1;
    [SerializeField] private int _exp;
    [SerializeField] private int[] levelThresholds = { 30, 100, 300, 2000, 6000, 20000, 50000 };

    [Header("Growth")]
    [SerializeField] private float _scalePerSize = 0.1f;

    [Header("Visual")]
    [SerializeField] private Transform _visual;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI expText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Slider expSlider;
    [SerializeField] private GameObject levelUpUI;
    [SerializeField] private GameObject endScreen;

    [Header("Audio")]
    [SerializeField] AudioSource eatSfx;
    [SerializeField] AudioSource levelUpSfx;
    [SerializeField] ParticleSystem eatParticle;

    public static event Action LevelUp;
    public int Size => _size;
    private Animator animator;
    private float timeLeft = 30f;


    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        UpdatePlayer();
    }

    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;

            int seconds = Mathf.FloorToInt(timeLeft);
            int centisecond = Mathf.FloorToInt((timeLeft - seconds) * 100);

            timerText.text = string.Format("{0:00}:{1:00}", seconds, centisecond);
        }
        else
        {
            EndGame();
        }
    }

    void EndGame()
    {
        timerText.text = "00:00";
        endScreen.SetActive(true);
        Time.timeScale = 0f;
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
        UpdateExpUI();

        _exp += value;

        int newSize = 1;
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
            UpdatePlayer();
            StartCoroutine(PlayLevelUpImage());
            levelUpSfx.Play();
        }
    }

    private void UpdatePlayer()
    {
        float scale = 1f + (_size - 1) * _scalePerSize;
        eatParticle.transform.localScale = Vector3.one * scale;
        _visual.localScale = Vector3.one * scale;
        CameraFollowController.Instance.AdjustZoomBasedOnTargetSize();
        LevelUp?.Invoke();
        levelText.text = _size.ToString();
    }

    private System.Collections.IEnumerator PlayLevelUpImage()
    {
        levelUpUI.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        levelUpUI.SetActive(false);
    }

    private void UpdateExpUI()
    {
        int nextThreshold;

        if (_size < levelThresholds.Length)
        {
            nextThreshold = levelThresholds[_size];
        }
        else
        {
            nextThreshold = _exp;
        }

        expText.text = "" + _exp + " / " + nextThreshold;
        float progress = (float)_exp / nextThreshold;
        expSlider.value = progress;

    }
}
