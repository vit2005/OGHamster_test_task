using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiningMiniGame : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] TextMeshProUGUI hash;
    [SerializeField] TextMeshProUGUI difficulty;
    [SerializeField] TextMeshProUGUI speed;
    [SerializeField] TextMeshProUGUI log;
    [SerializeField] Slider pregressbar;
    [SerializeField] Button buyButton;
    [SerializeField] TextMeshProUGUI mineButtonText;

    [Header("Animation")]
    [SerializeField] CoinFlightManager animationManager;

    [Header("Config")]
    [SerializeField] MiningMiniGameConfig config;

    private bool _isMining = false;
    private int _difficulty = 1;
    private int _speed = 1;
    private long _nonce = 0;
    private int _failTicks = 0;
    private Queue<string> _logLines = new Queue<string>();



    private bool _showEachBlock = true;
    private int _successes = 0;
    private int _failures = 0;
    private int _current_fail_threshold = 0;

    private float _mineTimer = 0f;

    private string _currentInput;

    /// Initializes the mini-game by generating the first block, updating the UI, 
    /// and subscribing to money updates so the Buy button reacts correctly.
    private void Start()
    {
        GenerateNewBlockData();
        UpdateUI();
        MoneyProvider.OnMoneyChanged += OnMoneyChanged;
    }

    /// Enables or disables the Buy button depending on whether the player has enough money.
    private void OnMoneyChanged(int value)
    {
        buyButton.interactable = value >= config.SpeedUpPrice;
    }

    /// Purchases a mining speed upgrade to increase hash attempts per second.
    public void OnBuyClick()
    {
        if (AppController.Instance.Money.Amount < config.SpeedUpPrice) return;

        AppController.Instance.Money.DecreaseMoney(config.SpeedUpPrice);
        _speed += config.SpeedUpAmountIncrease;
        UpdateUI();
        UpdateFontSizeBySpeed();
    }

    /// Toggles displaying every generated hash (by default is true). Useful to toggle off on high speed.
    public void OnShowEachBlockToggle(bool value)
    {
        _showEachBlock = value;
    }

    /// Adjusts the log font size to keep text readable when speed becomes high and many log lines appear quickly.
    private void UpdateFontSizeBySpeed()
    {
        float t = Mathf.InverseLerp(1f, 14f, _speed);
        int fontSize = Mathf.RoundToInt(Mathf.Lerp(30f, 12f, t));

        log.fontSize = fontSize;
    }

    /// Starts or stops the mining process so the player can control when hashing occurs.
    public void OnMineClick()
    {
        _isMining = !_isMining;
        if (_isMining)
            mineButtonText.SetText("STOP");
        else
            mineButtonText.SetText("START");
    }

    /// Adds a line to the log while keeping the log size limited to prevent memory and UI bloat.
    private void AddLog(string line)
    {
        _logLines.Enqueue(line);
        if (_logLines.Count > config.MaxLogLines)
            _logLines.Dequeue();

        log.text = string.Join("", _logLines);
    }

    /// Runs the mining loop: advances the block timer, triggers hash attempts, 
    /// and resets the block when the timeout is reached by simulating that someone else found it first.
    private void Update()
    {
        if (!_isMining)
            return;

        pregressbar.value += Time.deltaTime / config.BlockTime;

        _mineTimer += Time.deltaTime;
        float interval = 1f / _speed;
        if (_mineTimer >= interval)
        {
            _mineTimer -= interval;
            TryMineOnce();
        }

        // if progressbar filled - counting fails
        if (pregressbar.value < 1f)
            return;
        
        // if mine failed too many times - block was found by antother miner
        if (_failTicks >= _current_fail_threshold)
        {
            _current_fail_threshold = UnityEngine.Random.Range(0, config.FailThrashold);

            GenerateNewBlockData();
            _failTicks = 0;
            _failures++;

            // if player didn't found block few times in streak - decrease difficulty
            if (_failures >= config.DifficultyDecreaseStreak)
            {
                _difficulty = Mathf.Max(0, _difficulty - 1);
                UpdateUI();
                AddLog("\n<color=\"green\">DIFFICULTY DECREASED</color>\n");
                _failures = 0;
            }
        }
    }

    /// Performs a single mining attempt by hashing the current input with a nonce and checking difficulty.
    private void TryMineOnce()
    {
        string attempt = _currentInput + _nonce;
        string h = ShortHash(attempt);
                
        _nonce++;

        if (IsHashGood(h))
        {
            AddLog($"\n{HighlightHash(h, true)}");
            OnBlockFound();
        }
        else
        {
            if (_showEachBlock) AddLog($"\n{HighlightHash(h, false)}");
            _failTicks++;
        }
            
    }

    /// Handles the event of successfully finding a block: plays animations, rewards the player,
    /// adjusts difficulty, and prepares the next block.
    private void OnBlockFound()
    {
        animationManager.LaunchCoin();

        AddLog($"\n<color=\"green\">+1 SAT</color>");
        if (AppController.Instance.Money.Amount >= config.SpeedUpPrice)
            AddLog($"\n<color=\"green\">(BUY MORE SPEED)</color>");

        // if block has been found faster then average - add to success streak, else - to fail streak
        if (pregressbar.value < 1f)
        {
            _successes++;
            _failures = 0;
        }
        else
        {
            _failures++;
            _successes = 0;
        }

        // if success or fail streak reached - change difficulty
        if (_successes >= config.DifficultyIncreaseStreak)
        {
            _difficulty++;
            UpdateUI();
            AddLog("\n\n<color=\"red\">DIFFICULTY INCREASED</color>");
            _successes = 0;
        }
        else if (_failures >= config.DifficultyDecreaseStreak)
        {
            _difficulty = Mathf.Max(0, _difficulty - 1);
            UpdateUI();
            AddLog("\n<color=\"green\">DIFFICULTY DECREASED</color>\n");
            _failures = 0;
        }

        _failTicks = 0;

        // підготувати новий блок
        GenerateNewBlockData();
        UpdateUI();
    }

    /// Highlights the leading characters of the hash to visually indicate whether they meet the difficulty requirement.
    private string HighlightHash(string h, bool positive)
    {
        if (_difficulty <= 0)
            return h;

        int n = _difficulty;
        string green = "";

        for (int i = 0; i < n; i++)
        {
            if (positive)
                green += "<color=\"green\">0</color>";
            else
                green += $"<color=\"red\">{h[i]}</color>";
        }

        return green + h.Substring(n);
    }

    /// Generates fresh block data, resets progress and nonce, and logs a “new block” marker.
    private void GenerateNewBlockData()
    {
        _nonce = 0;
        _currentInput = GenerateRandomInput();
        hash.SetText(_currentInput);
        pregressbar.value = 0;
        AddLog("\n\nNEW BLOCK\n");
    }

    /// Checks whether the hash meets the current difficulty level by ensuring the correct number of leading zeroes.
    private bool IsHashGood(string h)
    {
        if (_difficulty <= 0) return true;
        for (int i = 0; i < _difficulty; i++)
        {
            if (h[i] != '0') return false;
        }
        return true;
    }

    /// Updates the difficulty and speed text fields to reflect the current mining state.
    private void UpdateUI()
    {
        difficulty.SetText($"{_difficulty}");
        speed.SetText($"{_speed} H/s");
    }

    #region Hashing

    private static readonly MD5 md5 = MD5.Create();
    private static readonly Encoding utf8 = Encoding.UTF8;

    /// Computes a short MD5-based hash using only the first few bytes to keep mining fast and gameplay-friendly.
    private static string ShortHash(string input)
    {
        byte[] bytes = md5.ComputeHash(utf8.GetBytes(input));
        return $"{bytes[0]:x2}{bytes[1]:x2}{bytes[2]:x2}{bytes[3]:x2}{bytes[4]:x2}";
    }

    const string chars = "0123456789abcdef";

    /// Creates a random 10-character hexadecimal input string to serve as the seed for a new block.
    private static string GenerateRandomInput()
    {
        StringBuilder sb = new StringBuilder(10);

        for (int i = 0; i < 10; i++)
            sb.Append(chars[UnityEngine.Random.Range(0, chars.Length)]);

        return sb.ToString();
    }

    #endregion

}
