using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム全体を管理
/// </summary>
public class GameDirector : MonoBehaviour
{
  
    [SerializeField ] StageData stageData;

    //キャンセルトクーン
     CancellationTokenSource cancelToken;

    //スコア管理
    public int totalScore;

    [SerializeField] TextMeshProUGUI ScoreText;
    //BGM名
    string bgmName;

    //獲得スコアを分割して一時待機させるキュー
    Queue<int> scoreQueue = new Queue<int>();

    //プレイヤーの残機
    int playerCount = 1;

    [Header("残機システム")]
    //プレイヤープレハブ
    [SerializeField] GameObject playerPrefab;
    //プレイヤー出現場所
    [SerializeField] Transform playerSpawn;
    //残機UI表示
    [SerializeField] Transform playerCountUI;
    [SerializeField] GameObject iconPrefab;

    

    //ゲームオバーUI
    [SerializeField] GameObject gameOverUI;

    //ゲームオバーフラグ
    bool gameOverFlg;

    //ストップフラグ
    public bool stopFlag;

    public static GameDirector instance;

    public GameObject ResultCanvas;

    [SerializeField ] int StageNumber = 1;

    // リザルト内のSTAGE 01用テキスト
    [Header("Result UI")]
    [SerializeField] private TextMeshProUGUI resultStageTitleText;


    void Awake()
    {  
        //  時間の流れを通常(1)に戻す
        Time.timeScale = 1f;

        // クリアフラグを確実に折る
        stopFlag = false;

        if (DataManager.instance != null)
        {
            playerCount = DataManager.instance.playerLifeLevel;
        }
        else
        {
            playerCount = 1;
        }
    }

    void OnEnable()
    {
        Debug.Log($"[GameDirector] OnEnable - targetStageNum: {stageData}");

        // 前回のトークンをキャンセル
        if (cancelToken != null)
        {
            cancelToken.Cancel();
            cancelToken.Dispose();
        }

        cancelToken = new CancellationTokenSource();

        // ステージ開始
        StartStageSequence().Forget();
    }



    async UniTaskVoid StartStageSequence()
    {
        //ゲームオバー非表示
        gameOverUI.SetActive(false);

        //スコア非表示
        instance = this;
        if (ResultCanvas != null) ResultCanvas.SetActive(false);

        //残機UIを表示
        UpdateLifeUI();

        //プレイヤー生成
        CreatePlayer();

        //キャンセルトークンの生成 
         cancelToken = new CancellationTokenSource();

        //キャンセルトークンの取得
        CancellationToken token = cancelToken.Token;

        //スコアの初期化
        ScoreText.text = totalScore.ToString("D0");

        // ステージデータの確認
        if (stageData == null)
        {
            Debug.LogError("[GameDirector] StageDataが設定されていません！インスペクターで設定してください。");
            return;
        }

       
        try
        {
                //ステージ読み込み開始
            await StageStart();

            Debug.Log("[GameDirector] ステージクリア！");

            if (!gameOverFlg)
            {
                
            }
        }

            //Tokenによってawaitしているタスクがキャンセルされた場合の例外処理
            catch (System.OperationCanceledException e)
            {
                Debug.Log($"ステージスタートがキャンセルされました >> " + e);

            }
        }

      
   

    //ステージのwave読み込み開始
    async UniTask StageStart()
    {
        //現在のステージデータから敵をスポーンさせる
        foreach (StageData.WaveInfo waveInfo in stageData.stage)
        {

            //Delay時間待機
            await UniTask.Delay((int)(waveInfo.delay * 1000f), false, PlayerLoopTiming.Update, cancelToken.Token);

            //Wave生成
            GameObject wave = Instantiate(waveInfo.wavePrefab);

            //ステージ開始Waveの場合
            if (waveInfo.waveType == StageData.WaveType.start)
            {
                //ステージ数をセット
                WaveStart waveStart = wave.GetComponent<WaveStart>();
                waveStart.SetStageNum(StageNumber);

                // InGameMenuControllerに登録 
                InGameMenuController menuController = Object.FindFirstObjectByType<InGameMenuController>();
                if (menuController != null)
                    menuController.SetWaveStart(waveStart);

                //ステージBGM
                bgmName = SoundData.BgmType.start.ToString();

                GSound.Instance.PlayBgm(bgmName, true);
            }

            //ステージクリアのWaveの場合
            else if (waveInfo.waveType == StageData.WaveType.clear)
            {
                //クリアBGM
                bgmName = SoundData.BgmType.clear.ToString();

                GSound.Instance.PlayBgm(bgmName, false);
              
            }

            //ウェーブエンディングの場合
            else if (waveInfo.waveType == StageData.WaveType.ending)
            {
                //スコアをセット
                WaveEnding waveEnding = wave.GetComponent<WaveEnding>();

                waveEnding.SetScore(totalScore);

                //エンディングBGM
                bgmName = SoundData.BgmType.clear.ToString();

                GSound.Instance.PlayBgm(bgmName, false);
            }

            //ボスWaveの場合
            else if(waveInfo .waveType == StageData.WaveType.boss)
            {
                //ボスBGM
                bgmName = SoundData.BgmType.boss.ToString();

                GSound.Instance.PlayBgm(bgmName, true);
            }

            //すべての敵を倒さないと次へいけない場合
            if (waveInfo.completFlg)
            {
                try
                {
                    //現在のWaveが破棄されるまで待機
                    await UniTask.WaitUntil(() => wave == null, PlayerLoopTiming.Update, cancelToken.Token);
                }
                catch (System.OperationCanceledException e)
                {
                    Debug.Log("Wave破棄までの待機処理がキャンセルされました" + e);

                    //UniTaskの停止
                    cancelToken.Cancel();

                    //foreachのループ処理を終わらせる
                    break;
                }
            }
        }
    
    }

    //スコアをセット
    public void AddScore(int score)
    {
  
        int splitScore = score / 50;

        //分割したスコアを一つずつキューに追加
        while(score > 0)
        {
            scoreQueue.Enqueue(splitScore);

            score -= splitScore;

            //分割で最後の余りを消去
            if(score < splitScore)
            {
                scoreQueue.Enqueue(score);

                score = 0;
            }
        }
    }

   

    //Update後に処理されるUpdate
    private void LateUpdate()
    {
        //キューに追加せれている未追加スコアをチェック
        CheckScoreQueue();

        //SEデータがあれば1フレームごとに１つのSEを鳴らす
        GSound.Instance.CheckSeQueue();

        
    }

    //キューに格納された分割スコアを一つずつスコア表記
    private void CheckScoreQueue()
    {
        //キューにデータがあれば一つだけ呼び出して処理する
        if(scoreQueue .Count > 0)
        {
            //キューから分割スコアを取り出す
            int s = scoreQueue.Dequeue();

            //取り出した分割スコアをトータルスコアへ追加
            totalScore += s;

            //3桁ごとにカンマを入れた表記
            ScoreText.text = totalScore.ToString("D0");
        }
    }

    //キューに残っているスコアをすべて一気に加算する
    public void FlushScoreQueue()
    {
        while (scoreQueue.Count > 0)
        {
            totalScore += scoreQueue.Dequeue();
        }
        ScoreText.text = totalScore.ToString("D0");
    }

    //プレイヤーの残機管理
    public void CreatePlayer()
    {
        //残機が残っている場合
        if(playerCount > 0)
        {
            playerCount--;

            Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity);

            // 残機UIの更新
            UpdateLifeUI();

        }

        //残機0でゲームオーバー
        else
        {
            gameOverUI.SetActive(true);

            gameOverFlg = true;

            stopFlag = true;
        }

            Debug.Log($"プレイヤー残機　>> {playerCount}");
    }

    void OnWaveComplete()
    {
        // データを保存する
        if (DataManager.instance != null)
        {
            // 今クリアしたのがステージ1なら「1」、ステージ2なら「2」を渡す
            int currentStage = 1;

            // クリア状況を更新
            if (DataManager.instance.clearedStageIndex < currentStage)
            {
                DataManager.instance.clearedStageIndex = currentStage;
            }

            // コインも加算
            DataManager.instance.totalCoins += 100;

            // セーブ実行
            DataManager.instance.SaveData();
        }
    }

    // ステージクリア判定のスクリプト内
    void StageClear()
    {
        //現在のスコアを取得（あなたのゲームのスコア変数に合わせてください）
        int finalScore = totalScore;

        //DataManagerに送ってコインに変換
        if (DataManager.instance != null)
        {
            DataManager.instance.AddCoinsFromScore(finalScore);

            // クリア状況も更新
            int currentStage = 1;
            if (DataManager.instance.clearedStageIndex < currentStage)
            {
                DataManager.instance.clearedStageIndex = currentStage;
                DataManager.instance.SaveData();
            }
        }

       

        // ゲームを止める
        Time.timeScale = 0;
    }



    private void UpdateLifeUI()
    {
        if (playerCountUI == null || iconPrefab == null) return;

        // 一旦すべてのアイコンを削除
        foreach (Transform icon in playerCountUI)
        {
            Destroy(icon.gameObject);
        }

        // 現在の残機の数だけアイコンを生成
        for (int i = 0; i < playerCount; i++)
        {
            Instantiate(iconPrefab, playerCountUI);
        }
    }



    public void OnStageClear()
    {
        //まずキューのスコアをすべて確定
        FlushScoreQueue();

        StageSettings settings = Object.FindFirstObjectByType<StageSettings>();
        if (settings == null) return;

        int score = totalScore;
        int newStars = settings.GetStars(score);
        int oldStars = DataManager.instance.GetStars(settings.stageNumber);

        int rewardCoins = 0;

        // もし新記録なら差分報酬を計算
        if (newStars > oldStars)
        {
            // 増えた星の分だけ報酬を加算
            for (int i = oldStars + 1; i <= newStars; i++)
            {
                if (i == 1) rewardCoins += settings.coinsForStar1;
                if (i == 2) rewardCoins += settings.coinsForStar2;
                if (i == 3) rewardCoins += settings.coinsForStar3;
            }

            // 星アニメーション用のフラグ
            DataManager.instance.isFirstClearToday = true;
        }
        else
        {
            // 星が増えなかった場合は報酬0
            DataManager.instance.isFirstClearToday = false;
        }

        if (DataManager.instance.clearedStageIndex < settings.stageNumber)
        {
            // クリア済みインデックスを更新
            DataManager.instance.clearedStageIndex = settings.stageNumber;

            // 新しいステージが解放されたので、鍵アニメフラグを立てる
            DataManager.instance.justUnlocked = true;
        }
        else
        {
            
            DataManager.instance.justUnlocked = false;
        }


        // 演出用データをDataManagerに預ける
        DataManager.instance.lastClearedStageNumber = settings.stageNumber;
        DataManager.instance.lastEarnedCoins = rewardCoins;

        //星を保存
        DataManager.instance.SaveStars(settings.stageNumber, newStars);

        //次にコインを加算して物理保存
        DataManager.instance.totalCoins += rewardCoins;
        DataManager.instance.SaveData();

        Debug.Log($"クリア確定：獲得{rewardCoins}コイン / 星{newStars}個 / フラグ{DataManager.instance.isFirstClearToday}");

        //リザルト画面の枠内の文字をセット
        if (resultStageTitleText != null)
        {
            resultStageTitleText.text = "STAGE " + settings.stageNumber.ToString("D2");
        }

        // リザルト表示へ
        if (ResultCanvas != null)
        {
            ResultCanvas.SetActive(true);
        }
    }

    //リトライボタン
    public async void OnPressRetryButton()
    {
        //SE再生
        string seName = SoundData.SeType.botton1.ToString();
        GSound.Instance.PlaySe(seName);

        // SEが鳴るまで少し待つ
        await UniTask.Delay(300);

        //UniTaskの停止
        cancelToken.Cancel();

        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    //タイトルへ戻るボタン
    public async void OnPressSelectButton()
    {
        //SE再生
        string seName = SoundData.SeType.botton1.ToString();
        GSound.Instance.PlaySe(seName);

        // SEが鳴るまで少し待つ
        await UniTask.Delay(300);

        //UniTaskの停止
        cancelToken.Cancel();

        //ステージ数リセット
        MyPlayer.Instance.STAGE_NUM = 0;

        SceneManager.LoadScene("01_Select");
    }


    //破棄された時に自動的に呼ばれる
    void OnDestroy()
    {
        if (cancelToken != null)
        {
            cancelToken.Cancel();
        }
    }

    //オブジェクトが非アクティブになったとき
    private void OnDisable()
    {
        if (cancelToken != null)
        {
            cancelToken.Cancel();
        }
    }

    //アプリが終了したとき
    private void OnApplicationQuit()
    {
        if (cancelToken != null)
        {
            cancelToken.Cancel();
        }
    }

}
