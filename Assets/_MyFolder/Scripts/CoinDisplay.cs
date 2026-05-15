using UnityEngine;
using TMPro;
using DG.Tweening; // DOTweenを使用

public class CoinDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;

    // 現在画面に表示されている数値
    private int currentDisplayValue; 

    void OnEnable()
    {
        if (DataManager.instance != null)
        {
            
            if (DataManager.instance.isFirstClearToday)
            {
                // クリア直後なら、獲得分を引いた「古い合計値」を最初に表示して待機する
                currentDisplayValue = DataManager.instance.totalCoins - DataManager.instance.lastEarnedCoins;
            }
            else
            {
                // 通常時は最新の合計値を表示
                currentDisplayValue = DataManager.instance.totalCoins;
            }
            UpdateText(currentDisplayValue);
        }
    }

   
    // 数値を即座に更新する
    
    public void UpdateCoinDisplay()
    {
        if (DataManager.instance != null)
        {
            currentDisplayValue = DataManager.instance.totalCoins;
            UpdateText(currentDisplayValue);
        }
    }

   
    // 指定した開始値から終了値までジャラジャラとカウントアップさせる
   
    public void StartCountUp(int startValue, int endValue)
    {
        // 以前のアニメーションが動いていたら止める
        DOTween.Kill(this);

        currentDisplayValue = startValue;

        //startValueからendValueまで、1秒間（duration）かけて変化させる
        DOTween.To(() => currentDisplayValue, x =>
        {
            currentDisplayValue = x;
            UpdateText(currentDisplayValue);
        }, endValue, 1.2f)
        .SetEase(Ease.OutQuad) // 終わりにかけて少しゆっくりにする
        .SetId(this)
        .SetUpdate(true); // タイムスケールが0でも動くようにする
    }

    // カンマ区切りでテキストを更新する共通処理
    private void UpdateText(int value)
    {
        if (coinText != null)
        {
            // 0以下の場合は0を表示する
            int displayVal = Mathf.Max(0, value);
            coinText.text = displayVal.ToString("N0");
        }
    }
}