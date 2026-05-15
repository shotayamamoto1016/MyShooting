using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro; //TextMeshProを使う場合
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultScreen : MonoBehaviour
{
    public TextMeshProUGUI TotalscoreText;

    public TextMeshProUGUI CoinText;

    public Button BackToSelectButton;

    public GameDirector gameDirector;

   

    // クリア時にこのメソッドを呼び出す
    void OnEnable()
    {
        //アニメーション前にすべてのサイズをゼロにする
        TotalscoreText.transform.localScale = Vector3.zero;

        CoinText.transform.localScale = Vector3.zero;

        BackToSelectButton.transform.localScale = Vector3.zero;

        // DataManagerから今回の結果をもらって表示
        if (GameDirector.instance != null)
            TotalscoreText.text = "Score : " + GameDirector.instance.totalScore.ToString();

        if (DataManager.instance != null)
        {
            //保存したlastEarnedCoinsを表示する
            CoinText.text = "Coins : " + DataManager.instance.lastEarnedCoins.ToString();
        }

        PlayAnimation();       
    }


    void PlayAnimation()
    {
        //シーケンスを作成
        Sequence resSeq = DOTween.Sequence();

        //1秒間だけ何もせず待つ
        resSeq.AppendInterval(1.0f);

        //スコアがポンッと出る
        resSeq.Append(TotalscoreText.transform.DOScale(1.0f, 0.5f).SetEase(Ease.OutBack));

        resSeq.AppendInterval(2f); 

        //コインがポンッと出る
        resSeq.Append(CoinText.transform.DOScale(1.0f, 0.5f).SetEase(Ease.OutBack));

        resSeq.AppendInterval(2f);

        //ボタンがポンッと出る
        resSeq.Append(BackToSelectButton.transform.DOScale(1.0f, 0.5f).SetEase(Ease.OutBack));

        //全ての演出で「時間停止(Time.timeScale=0)」の影響を受けないように
        resSeq.SetUpdate(true);

        resSeq.SetLink(gameObject);
        resSeq.Play();
    }

   


    //ボタンに割り当てる用
    public async void BackToStageSelect()
    {
        //SE再生
        string seName = SoundData.SeType.botton1.ToString();
        GSound.Instance.PlaySe(seName);

        //SEが鳴るまで少し待つ
        await UniTask.Delay(300);

        SceneManager.LoadScene("01_Select");
    }

    public void SetupResult(int score, StageSettings settings)
    {
        int stars = settings.GetStars(score);
        int coins = settings.GetCoins(score);

        //コインをDataManagerに加算
        DataManager.instance.totalCoins += coins;
        DataManager.instance.lastEarnedCoins = coins;

        //星の数を更新保存
        DataManager.instance.SaveStars(settings.stageNumber, stars);
        DataManager.instance.SaveData();    
       
    }
}