using DG.Tweening;  
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

public class EndingDirector : MonoBehaviour
{
    //BGM名
    string bgmName;

    //エンディングメッセージ
    [SerializeField] TextMeshProUGUI endingText;

    //エンディングイメージ
    [SerializeField] RectTransform endingImage;

    //スクリーンサイズ
    Vector3 screenSize;

    //エンドクレジット
    [SerializeField] RectTransform endCredit;

    //エンドクレジットのスクロール時間
    [SerializeField] float endCreditTime = 10f;

    //タイトルへ戻るボタン
    [SerializeField] GameObject titleToButton;

    //進歩管理
    int phaseNo = 0;

    //シーン遷移アニメーションプレハブ
    [SerializeField] GameObject sceneTransition;

    async void Start()
    {
        //スクリーンサイズ取得（デバイスによって異なる）
        float screenSize = UnityEngine.Screen.height;

        // 現在位置
        Vector2 startPos = endCredit.anchoredPosition;

        //エンディングイメージのサイズ
        Vector2 imageSize = endingImage.sizeDelta;

        //エンディングイメージの縦横比
        float ratio = imageSize.y / imageSize.x;

        var canvasRecter = endingImage.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        Vector2 canvasSize = canvasRecter.rect.size;

        //エンディングイメージを画面の横幅に合わせて引き伸ばして縦幅は画像比率を固定して引き延ばす
        endingImage.sizeDelta = new Vector2(canvasSize.x, canvasSize.x * ratio);


        //ステージBGM
        bgmName = SoundData.BgmType.title.ToString();

        GSound.Instance.PlayBgm(bgmName, true);

        Color color;

        //エンディングメッセージを透明で初期化
        color = endingText.color;
        color.a = 0;
        endingText.color = color;

        //DOTween処理をまとめるシークエンス
        Sequence sequence = DOTween.Sequence();

        //エンディングメッセージフェードイン
        //DOFade()の第一引数は0f = 完全透明で、1f = 完全表示である。
        //また第二引数はかける時間である。
        sequence.Append(endingText.DOFade(1f, 5f));
        //待機
        sequence.AppendInterval(20f);

        //エンディングメッセージフェードアウト
        sequence.Append(endingText.DOFade(0f, 5f));

        //DOTween実行
        sequence.SetLink(gameObject).Play();

        // Canvas の高さ（UI座標系）
        var canvasRect = endCredit.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        float canvasHeight = canvasRect.rect.height;

        // ピクセルをCanvas座標系に換算するスケール係数
        float scaleY = canvasHeight / screenSize;

        // Screen.height を UI座標に変換した値
        float screenHeightUI = screenSize * scaleY;

        // 終点位置 = 現在位置 + (クレジットの高さ + 画面高さUI分) を上方向に移動
        Vector2 endCreditFinishPos = startPos + Vector2.up * (endCredit.rect.height + screenHeightUI);

        //エンドクレジット以外は非表示で初期化
        endingImage.gameObject.GetComponent<Image>().DOFade(0f, 0f);
        endingImage.gameObject.SetActive(false);
        titleToButton.SetActive(false);

        //エンドクレジットスクロール開始
        endCredit.DOAnchorPos(endCreditFinishPos, endCreditTime)
            .SetEase(Ease.Linear)
            .SetLink(endCredit.gameObject)
            .OnComplete(() =>
            {
                //次のフェーズへ切り替える
                phaseNo++;
            });

        //進行度が進むまで待機
        await UniTask.WaitUntil(() => phaseNo > 0);

        //エンディングイメージを表示
        endingImage.gameObject.SetActive(true);

        endingImage.GetComponent<Image>().DOFade(1f, 3f)
                   .SetLink(endingImage.gameObject)
                   .OnComplete(() => 
                   {
                       //次のフェーズへ切り替える
                       phaseNo++;
                   });


        //進行度が進むまで待機
        await UniTask.WaitUntil(() => phaseNo > 1);

        //Delay時間待機
        await UniTask.Delay((int)(0.05f * 1000f));

        //タイトルへ戻るボタンを表示
        titleToButton.SetActive(true);
    }

    //タイトルへ戻るボタンが押された時
    public async void OnPressTitleToButton()
    {
        //画面フェードアウト（待機処理）
        await Instantiate(sceneTransition).GetComponent<SceneTransition>().FadeOutAsync(3);


        SceneManager.LoadScene("00_Title");
    }
    
    
}
