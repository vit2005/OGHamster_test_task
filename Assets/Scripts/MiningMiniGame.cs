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
    [SerializeField] TextMeshProUGUI money;
    [SerializeField] TextMeshProUGUI hash;
    [SerializeField] TextMeshProUGUI difficulty;
    [SerializeField] TextMeshProUGUI speed;
    [SerializeField] TextMeshProUGUI log;
    [SerializeField] Slider pregressbar;
    [SerializeField] Button buyButton;
    [SerializeField] TextMeshProUGUI mineButtonText;

    private bool _isMining = false;
    private int _money = 0;
    private int _difficulty = 1;
    private int _speed = 1;
    private long _nonce = 0;
    private int _failTicks = 0;
    private Queue<string> _logLines = new Queue<string>();

    private const float BLOCK_TIME = 5f;
    private const int DIFFICULTY_DECREASE_CHANGE = 3;
    private const int DIFFICULTY_INCREASE_CHANGE = 5;
    private const int SPEED_PRICE = 3;
    private const int SPEED_INCREASE = 3;
    private const int FAIL_THRESHOLD = 20;
    private const int MAX_LOG_LINES = 30;


    private int _successes = 0;
    private int _failures = 0;
    private int _current_fail_threshold = 0;

    private float _mineTimer = 0f;

    private string _currentInput;

    private void Start()
    {
        GenerateNewBlockData();
        UpdateUI();
        buyButton.onClick.AddListener(OnBuyClick);
    }

    public void OnBuyClick()
    {
        if (_money < SPEED_PRICE) return;

        _money -= SPEED_PRICE;
        _speed += SPEED_INCREASE;
        UpdateUI();
        UpdateFontSizeBySpeed();
    }

    private void UpdateFontSizeBySpeed()
    {
        // speed = 1 → 30, speed = 14 → 15
        float t = Mathf.InverseLerp(1f, 14f, _speed);
        int fontSize = Mathf.RoundToInt(Mathf.Lerp(30f, 15f, t));

        log.fontSize = fontSize;
    }

    public void OnMineClick()
    {
        _isMining = !_isMining;
        if (_isMining)
            mineButtonText.SetText("STOP");
        else
            mineButtonText.SetText("START");
    }

    private void AddLog(string line)
    {
        _logLines.Enqueue(line);
        if (_logLines.Count > MAX_LOG_LINES)
            _logLines.Dequeue();

        AddLog(string.Join("\n", _logLines));
    }

    private void Update()
    {
        buyButton.interactable = _money >= SPEED_PRICE;

        if (!_isMining)
            return;

        // оновлення прогресбару
        pregressbar.value += Time.deltaTime / BLOCK_TIME;

        // оновлюємо таймер
        _mineTimer += Time.deltaTime;

        // час виконати наступну спробу?
        float interval = 1f / _speed;

        if (_mineTimer >= interval)
        {
            _mineTimer -= interval;
            TryMineOnce();
        }

        // якщо прогресбар заповнився — рахуємо фейли
        if (pregressbar.value >= 1f)
        {
            // якщо чекали занадто довго — блок знайшов хтось інший
            if (_failTicks >= _current_fail_threshold)
            {
                _current_fail_threshold = UnityEngine.Random.Range(0, FAIL_THRESHOLD);
                //log.text += "\n\n<b>BLOCK LOST</b>";
                GenerateNewBlockData();
                _failTicks = 0; // обнулення фейлів
                _failures++;
                if (_failures >= DIFFICULTY_DECREASE_CHANGE)
                {
                    _difficulty = Mathf.Max(0, _difficulty - 1);
                    AddLog(log.text += "\n<color=\"green\">DIFFICULTY DECREASED</color>\n");
                    _failures = 0;
                }
            }
        }
    }


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
            AddLog($"\n{HighlightHash(h, false)}");
            _failTicks++;
        }
            
    }

    private void OnBlockFound()
    {
        _money += 1;
        AddLog($"\n<color=\"green\">+1 SAT</color>");
        if (_money >= SPEED_PRICE)
            AddLog($"\n<color=\"green\">(BUY MORE SPEED)</color>");

        // блок знайдено швидше, ніж прогресбар встигнув заповнитись
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

        // корекція складності
        if (_successes >= DIFFICULTY_INCREASE_CHANGE)
        {
            _difficulty++;
            AddLog("\n\n<color=\"red\">DIFFICULTY INCREASED</color>");
            _successes = 0;
        }
        else if (_failures >= DIFFICULTY_DECREASE_CHANGE)
        {
            _difficulty = Mathf.Max(0, _difficulty - 1);
            _failures = 0;
        }

        _failTicks = 0;

        // підготувати новий блок
        GenerateNewBlockData();
        UpdateUI();
    }

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

    private void GenerateNewBlockData()
    {
        _nonce = 0;
        _currentInput = GenerateRandomInput();
        hash.SetText(_currentInput);
        pregressbar.value = 0;
        AddLog("\n\nNEW BLOCK\n");
    }

    private bool IsHashGood(string h)
    {
        if (_difficulty <= 0) return true;
        for (int i = 0; i < _difficulty; i++)
        {
            if (h[i] != '0')
            {
                //Debug.Log($"Checking hash {h} for difficulty {_difficulty} : {h[i]} != 0");
                return false;
            }
                
        }
        return true;
    }

    private void UpdateUI()
    {
        money.SetText($"{_money}");
        difficulty.SetText($"{_difficulty}");
        speed.SetText($"{_speed}");
    }

    private static readonly MD5 md5 = MD5.Create();
    private static readonly Encoding utf8 = Encoding.UTF8;

    private static string ShortHash(string input)
    {
        byte[] bytes = md5.ComputeHash(utf8.GetBytes(input));
        return $"{bytes[0]:x2}{bytes[1]:x2}{bytes[2]:x2}{bytes[3]:x2}{bytes[4]:x2}";
    }

    const string chars = "0123456789abcdef";
    private static string GenerateRandomInput()
    {
        StringBuilder sb = new StringBuilder(10);

        for (int i = 0; i < 10; i++)
            sb.Append(chars[UnityEngine.Random.Range(0, chars.Length)]);

        return sb.ToString();
    }

}
