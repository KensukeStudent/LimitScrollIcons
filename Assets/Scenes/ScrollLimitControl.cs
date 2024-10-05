using System;
using UnityEngine;

/// <summary>
/// スクロール時の左右領域範囲を制御します
/// </summary>
[RequireComponent(typeof(ScrollLimitRect))]
public class ScrollLimitControl : MonoBehaviour
{
    private ScrollLimitRect mainScroll = null;

    [SerializeField]
    private ScrollLimitData limitData = null;

    private Vector2 originContent;

    private float currentPercent = 0;

    private void Awake()
    {
        TryGetComponent(out mainScroll);
    }

    private void Start()
    {
        originContent = mainScroll.content.anchoredPosition;

        mainScroll.Init(
        originContent.x + limitData.limitValue,
        originContent.x - limitData.limitValue,
        ScrollLimitRect.Direction.Horizontal
        );
        limitData.Init();
    }

    private void Update()
    {
        // 書式例
        // 0 - 250 = 最大座標 = max (250)
        // 稼働領域 = 250
        // 座標が50, 50 - 0 = 50 / 250

        // 50 - 250 = 最大座標 = max (300)
        // 稼働領域 = 250
        // 座標が100, 100 - 50 = 50 / 250

        // -50 - 250 = 最小座標 = min (-300)
        // 稼働領域 = 250
        // 座標が-70, -50 - -70 = 20/250

        // 20 - 250 = 最小座標 = min (-230)
        // 稼働領域 = 250
        // 座標が -50, 20 - -50 = 70/250

        var normalizedValue = mainScroll.content.anchoredPosition.x - originContent.x; // 現在の移動領域量
        currentPercent = Mathf.Clamp(normalizedValue / limitData.limitValue, -1, 1);

        // 各サブオブジェクトの稼働運動
        for (int i = 0; i < limitData.subLimits.Length; i++)
        {
            var sub = limitData.subLimits[i];
            var pos = sub.rect.anchoredPosition;
            var min = sub.origin.x - sub.minValue;
            var max = sub.origin.x + sub.maxValue;

            // 50 - 250 = 300
            // 0.7%, 250*0.7=175, 50+175=225
            var addX = currentPercent;
            addX *= currentPercent > 0 ? sub.maxValue : sub.minValue;
            pos.x = Mathf.Clamp(sub.origin.x + addX, min, max);

            sub.rect.anchoredPosition = pos;
        }
    }
}

[Serializable]
public class ScrollLimitData
{
    /// <summary>
    /// スクロール全体の稼働限界値
    /// </summary>
    [SerializeField]
    public float limitValue;

    /// <summary>
    /// メインの可動域に応じて各自制御されたスクロールするリスト
    /// </summary>
    [SerializeField]
    public SubLimit[] subLimits = null;

    public void Init()
    {
        for (int i = 0; i < subLimits.Length; i++)
        {
            subLimits[i].Init();
        }
    }
}

[Serializable]
public class SubLimit
{
    public RectTransform rect = null;

    public Vector2 origin { private set; get; }

    /// <summary>
    /// 上か右への稼働量
    /// </summary>
    [Min(0)]
    public float maxValue;

    /// <summary>
    /// 下か左への稼働量
    /// </su[mmary>
    [Min(0)]
    public float minValue;

    public void Init()
    {
        origin = rect.anchoredPosition;
    }
}