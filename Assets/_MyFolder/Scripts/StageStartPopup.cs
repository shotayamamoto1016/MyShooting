using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageStartPopup : MonoBehaviour
{
    [Header("UI要素の割り当て")]
    [SerializeField] private TextMeshProUGUI stageTitleText; //STAGE 01用
    [SerializeField] private Image[] starImages;            //3つの星の「Image」
    [SerializeField] private TextMeshProUGUI rewardText;     //スコア表用
    [SerializeField] private Button sortieButton;           //出撃ボタン
    [SerializeField] private Button closeButton;            //閉じるボタン

    [System.Serializable]
    public struct StageRewardData
    {
        public int score1, reward1;
        public int score2, reward2;
        public int score3, reward3;
    }

    [Header("全12ステージの報酬データ")]
    //インスペクターで12個分のスコアとコインを入力できます
    [SerializeField] private StageRewardData[] allStageRewards = new StageRewardData[12];

    private string nextSceneName;

    void Awake()
    {
        //閉じるボタンに機能を登録
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(() => ClosePopup());
        }
    }

    public void OpenPopup(int stageNum, string sceneName)
    {
        nextSceneName = sceneName;

        //タイトルの更新
        stageTitleText.text = "STAGE " + stageNum.ToString("D2");

        //星の更新
        int earnedStars = DataManager.instance.GetStars(stageNum);
        for (int i = 0; i < starImages.Length; i++)
        {
            if (i < earnedStars)
                starImages[i].color = Color.white; //獲得済み：元の色
            else
                starImages[i].color = Color.black; //未獲得：黒
        }

        //報酬テキストの作成
        if (stageNum <= allStageRewards.Length)
        {
            StageRewardData data = allStageRewards[stageNum - 1];
            rewardText.text = $"score | reward\n\n" +
                              $"{data.score1}  : <space=12><voffset=9><size=120%><sprite name=\"CoinSprite_0\"></size></voffset><space=-12>{data.reward1}\n\n" +
                              $"{data.score2}  : <space=12><voffset=9><size=120%><sprite name=\"CoinSprite_0\"></size></voffset><space=-12>{data.reward2}\n\n" +
                              $"{data.score3}  : <space=12><voffset=9><size=120%><sprite name=\"CoinSprite_0\"></size></voffset><space=-12>{data.reward3}";
        }

        //出撃ボタンの設定
        sortieButton.onClick.RemoveAllListeners();
        sortieButton.onClick.AddListener( () => {
            //シーン移動前に少し演出を入れたい場合はここに
            //SE再生
            string seName = SoundData.SeType.botton1.ToString();
            GSound.Instance.PlaySe(seName);

            ////SEが鳴るまで少し待つ
            //await System.Threading.Tasks.Task.Delay(400);

            DOVirtual.DelayedCall(0.2f, () => {

                DOTween.KillAll();
            SceneManager.LoadScene(nextSceneName);

            });
        });

        //ポップアップを表示
        gameObject.SetActive(true);

        //ポップアップが出る時のアニメーション
        transform.localScale = Vector3.zero;
        transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack).SetUpdate(true).SetLink(gameObject);
    }

    public void ClosePopup()
    {
        //SE再生
        string seName = SoundData.SeType.botton1.ToString();
        GSound.Instance.PlaySe(seName);

        transform.DOKill();
        //閉じる時のアニメーション
        transform.DOScale(0f, 0.2f).SetEase(Ease.InBack).SetUpdate(true).SetLink(gameObject).OnComplete(() => {
            if (this != null && gameObject != null)
            {
                gameObject.SetActive(false);
            }
        });
    }
}