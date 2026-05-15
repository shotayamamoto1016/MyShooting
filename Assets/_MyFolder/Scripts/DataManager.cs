using UnityEngine;


public class DataManager : MonoBehaviour
{
    // どこからでもアクセスできるように「シングルトン」という仕組みにします
    public static DataManager instance;

    [Header("プレイヤーデータ")]
    public int clearedStageIndex = 0; // どこまでクリアしたか（1ならステージ1クリア済み＝2が解放）
    public int shotSpeedLevel = 1;
    public int shotIntervalLevel = 1;
    public int playerSpeedLevel = 1;
    public int playerLifeLevel = 1; 
    public int shotPowerLevel = 1;
    public int optionCountLevel = 0;
    public int totalCoins = 0;
    [HideInInspector] public int lastEarnedCoins = 0;

    public int lastClearedStageNumber; // 最後のリザルトでクリアした番号
    public bool isFirstClearToday;     // 星アニメを流すフラグ
    public bool justUnlocked;          // 鍵アニメを流すフラグ

    public int prevStarCount;       // プレイ前の星の数
    public int currentNewStarCount; // 今回獲得した星の数


    // データを保存する際のキー名を作成する関数
    string StarKey(int stageNum) => "StageStar_" + stageNum;


    void Awake()
    {
        // シングルトンの設定（このオブジェクトが1つだけ存在するようにする）
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // シーンを切り替えても消さない
            LoadData(); // 起動時にデータを読み込む
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // データを保存するメソッド
    public void SaveData()
    {
        PlayerPrefs.SetInt("ClearedStage", clearedStageIndex);
        PlayerPrefs.SetInt("ShotSpeedLevel", shotSpeedLevel);
        PlayerPrefs.SetInt("ShotIntervalLevel", shotIntervalLevel);
        PlayerPrefs.SetInt("PlayerSpeedLevel", playerSpeedLevel);
        PlayerPrefs.SetInt("PlayerLifeLevel", playerLifeLevel);
        PlayerPrefs.SetInt("ShotPowerLevel", shotPowerLevel);
        PlayerPrefs.SetInt("OptionCountLevel", optionCountLevel);
        PlayerPrefs.SetInt("TotalCoins", totalCoins);
        PlayerPrefs.Save(); // 確実に書き込む
        Debug.Log("データを保存しました");
    }

    // データを読み込むメソッド
    public void LoadData()
    {
        // 第二引数は「データがなかった時の初期値」
        clearedStageIndex = PlayerPrefs.GetInt("ClearedStage", 0);
        shotSpeedLevel = PlayerPrefs.GetInt("ShotSpeedLevel", 1);
        shotIntervalLevel = PlayerPrefs.GetInt("ShotIntervalLevel", 1);
        playerSpeedLevel = PlayerPrefs.GetInt("PlayerSpeedLevel", 1);
        playerLifeLevel = PlayerPrefs.GetInt("PlayerLifeLevel", 1);
        shotPowerLevel = PlayerPrefs.GetInt("ShotPowerLevel", 1);
        optionCountLevel = PlayerPrefs.GetInt("OptionCountLevel", 0);
        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        Debug.Log("データを読み込みました");
    }

    // 開発用：データをリセットしたい時に使う
    [ContextMenu("Reset Data")]
    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
        clearedStageIndex = 0;
        shotSpeedLevel = 1;
        shotIntervalLevel = 1;
        playerSpeedLevel = 1;
        playerLifeLevel = 0;
        shotPowerLevel = 1;
        optionCountLevel = 0;
        totalCoins = 0;
        Debug.Log("データをリセットしました");
    }

    // DataManager.csに追記

    public void AddCoinsFromScore(int score)
    {

        // 総所持金に加算
        totalCoins += lastEarnedCoins;

        // セーブ実行
        SaveData();
    }

    // 星の数を保存
    public void SaveStars(int stageNum, int starCount)
    {
        // 今までのベストスコア（星の数）より高い場合だけ更新
        string key = "StageStar_" + stageNum;
        int currentBest = GetStars(stageNum);

        // 今回のプレイ前の記録を保存（演出用）
        prevStarCount = currentBest;
        

        if (starCount > currentBest)
        {
            PlayerPrefs.SetInt(key, starCount);
            PlayerPrefs.Save();
        }
    }

    // 保存された星の数を取得
    public int GetStars(int stageNum)
    {
        return PlayerPrefs.GetInt(StarKey(stageNum), 0);
    }

}
