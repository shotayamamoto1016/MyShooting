using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeItemUI : MonoBehaviour
{
    //項目の種類を定義
    public enum UpgradeType { ShotSpeed, ShotInterval, PlayerSpeed, PlayerLife, ShotLevel, OptionCount }
    public UpgradeType type;

    [Header("UI要素")]
    [SerializeField] TextMeshProUGUI levelText;    
    [SerializeField] TextMeshProUGUI priceText;    
    [SerializeField] Button upgradeButton;

    [Header("設定")]
    [SerializeField] int maxLevel = 10;
    [SerializeField] int basePrice = 100; //レベル1から2にする時の価格

    void OnEnable()
    {
        if (DataManager.instance == null)
        {
            Debug.LogWarning("DataManagerが見つかりません。タイトルから始めてください。");
            return;
        }

       
        RefreshUI();
    }

    //表示を更新する
    public void RefreshUI()
    {
        int currentLevel = GetCurrentLevel();
        int nextLevel = currentLevel + 1;
    
        //「初期値から何回上げたか」を基準に計算する
        int upgradeCount = GetUpgradeCount(currentLevel);
        int price = (upgradeCount + 1) * basePrice;

        //レベル表示
        if (currentLevel >= maxLevel)
        {
            //MAX時の表示
            levelText.text = GetLevelString(currentLevel, true);
            priceText.text = "---";
            upgradeButton.interactable = false;
        }
        else
        {
            //強化中の表示
            levelText.text = GetLevelString(currentLevel, false);
            priceText.text = price.ToString();
            upgradeButton.interactable = (DataManager.instance.totalCoins >= price);
        }
    }

    //ボタンが押された時の処理
    public void OnClickUpgrade()
    {
        int currentLevel = GetCurrentLevel();
        int price = currentLevel * basePrice;

        if (DataManager.instance.totalCoins >= price)
        {
            //コインを消費
            DataManager.instance.totalCoins -= price;

            if (GSound.Instance == null)
            {
                Debug.LogError("GSound.Instanceがnullです！");
            }
            else
            {
                Debug.Log("GSound.Instance正常");
                string seName = SoundData.SeType.powerUp.ToString();
                Debug.Log("SE名: " + seName);
                GSound.Instance.PlaySe(seName);
            }

            //レベルを上げる
            AddLevel();

            //保存
            DataManager.instance.SaveData();

            //コイン表示枠も更新
            Object.FindFirstObjectByType<CoinDisplay>()?.UpdateCoinDisplay();

            //コインが足りなくなった他のボタンも即座に暗くする
            UpgradeItemUI[] allItems = Object.FindObjectsByType<UpgradeItemUI>(FindObjectsSortMode.None);
            foreach (var item in allItems)
            {
                item.RefreshUI();
            }
        }
    }

    //DataManagerから現在のレベルを取得する
    int GetCurrentLevel()
    {
        if (DataManager.instance == null) return 1;

        switch (type)
        {
            case UpgradeType.ShotSpeed: return DataManager.instance.shotSpeedLevel;
            case UpgradeType.ShotInterval: return DataManager.instance.shotIntervalLevel;
            case UpgradeType.PlayerSpeed: return DataManager.instance.playerSpeedLevel;
            case UpgradeType.PlayerLife: return DataManager.instance.playerLifeLevel;
            case UpgradeType.ShotLevel: return DataManager.instance.shotPowerLevel;
            case UpgradeType.OptionCount: return DataManager.instance.optionCountLevel;
            default: return 1;
        }
    }

    //レベルを加算する
    void AddLevel()
    {
        switch (type)
        {
            case UpgradeType.ShotSpeed: DataManager.instance.shotSpeedLevel++; break;
            case UpgradeType.ShotInterval: DataManager.instance.shotIntervalLevel++; break;
            case UpgradeType.PlayerSpeed: DataManager.instance.playerSpeedLevel++; break;
            case UpgradeType.PlayerLife: DataManager.instance.playerLifeLevel++; break;
            case UpgradeType.ShotLevel: DataManager.instance.shotPowerLevel++; break;
            case UpgradeType.OptionCount: DataManager.instance.optionCountLevel++; break;
        }
    }

    //種類に合わせて表示する文字を作る専用のメソッド
    string GetLevelString(int current, bool isMax)
    {
        int next = current + 1;

        switch (type)
        {
            case UpgradeType.PlayerLife:
                //DataManagerの初期値が1（出撃分）なので、表示は -1 して「追加残機数」にする
                int currentExtraLife = current - 1;
                int nextExtraLife = next - 1;
                if (isMax) return $"残機 {currentExtraLife} (MAX)";
                return $"残機 {currentExtraLife} → {nextExtraLife}";

            case UpgradeType.OptionCount:
                //自機 + 0 → 1
                if (isMax) return $"自機 {current} (MAX)";
                return $"自機 {current} → {next}";

            default:
                //それ以外（ショット速度、Lvなど）は通常の Lv. 1 → 2
                if (isMax) return $"Lv. {current} (MAX)";
                return $"Lv. {current} → {next}";
        }
    }

    //強化回数を計算する（初期値からの差分）
    int GetUpgradeCount(int currentLvl)
    {
        switch (type)
        {
            case UpgradeType.OptionCount: return currentLvl - 0; //初期値0
            case UpgradeType.PlayerLife: return currentLvl - 1; //初期値1
            default: return currentLvl - 1; //他は初期値1
        }
    }
}