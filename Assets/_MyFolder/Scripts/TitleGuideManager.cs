using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TitleGuideManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TitleHintText; //テキスト本体
    [SerializeField] private GameObject speechBubble;  //吹き出しの画像
    [SerializeField] private GameObject characterObj; //自機の画像

    // staticにすることで、シーンをまたいでも「次はこの番号」という記憶を保持する
    private static int earlyIndex = 0;
    private static int shopIndex = 0;
    private static int completeIndex = 0;


    //ステージ3クリア前のメッセージ
    private string[] earlyHints = {
        "ステージ３をクリアすれば、自機の強化ができるようになるぞ！",
        "敵がだんだん強くなってくるぞ。まずはステージ３を目指そう！",
        "君ならできる！諦めずにステージ３まで突っ走るんだ！"
    };

    private string[] shopHints = {
        "ショット速度を上げれば、敵に素早く弾が届くぞ！",
        "連射間隔を短くして、弾幕で敵を圧倒しよう！",
        "移動速度が上がれば、敵の攻撃を避けやすくなるぞ。",
        "残機を増やして、粘り強く戦い抜くんだ！",
        "ショットレベルを上げれば、攻撃範囲が一気に広がるぞ。",
        "追従機を連れれば、攻撃力は実質２倍だ！"
    };

    //全ステージクリア後のメッセージ
    private string[] completeHints = {
        "全ステージクリア達成！！君は最高のパイロットだ！",
        "伝説のパイロット誕生だ！全ステージ制覇おめでとう！",
        "すべての敵を倒したな。この銀河に君の敵はもういないぞ！"
    };

    void Start()
    {

        //ふわふわアニメーションの開始
        if (speechBubble != null)
        {
            speechBubble.transform.localScale = Vector3.zero;

            speechBubble.transform.DOLocalMoveY(10f, 3f)
                .SetRelative()
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(speechBubble); 
        }
        else
        {
            Debug.LogError("TitleGuideManager: SpeechBubbleがインスペクターで設定されていません！");
        }

        // データの更新
        UpdateGuide();
    }

    void Update()
    {
        if (!TitleHintText.gameObject.activeSelf)
        {
            Debug.LogWarning("hintTextが非表示になっています！");
            
        }
    }

    public void UpdateGuide()
    {
        if (DataManager.instance == null || TitleHintText == null || speechBubble == null)
        {
            TitleHintText.text = "データの準備中...";
            return;
        }

       

        int clearedCount = DataManager.instance.clearedStageIndex;

        //表示する文字を一時的に入れる変数
        string targetText = "";

        

        Debug.Log($"[ガイド更新］現在のステージ数：{clearedCount}");

        //ランダムではなく順番に取得するロジック
        if (clearedCount >= 12)
        {
            // インデックスのセリフを取得し、次に備えて＋１する
            targetText = completeHints[completeIndex];
            completeIndex = (completeIndex + 1) % completeHints.Length;
        }
        else if (clearedCount >= 3)
        {
            targetText = shopHints[shopIndex];
            shopIndex = (shopIndex + 1) % shopHints.Length;
        }
        else
        {
            targetText = earlyHints[earlyIndex];
            earlyIndex = (earlyIndex + 1) % earlyHints.Length;
        }


        // 前のアニメーションをリセット
        TitleHintText.DOKill();
        speechBubble.transform.DOKill(true); // trueを入れると完了状態にしてから止める

        // 吹き出しの「ポンッ！」演出
        speechBubble.transform.localScale = Vector3.one * 0.8f;
        speechBubble.transform.DOScale(1.0f, 0.5f).SetEase(Ease.OutBack);

        // 文字列を先にセットし、表示文字数を0にする
        TitleHintText.text = targetText;
        TitleHintText.maxVisibleCharacters = 0;

        // maxVisibleCharactersを、0 から 文字数までアニメーションさせる
        DOTween.To(() => TitleHintText.maxVisibleCharacters,
                   x => TitleHintText.maxVisibleCharacters = x,
                   targetText.Length,
                   1.5f)
               .SetEase(Ease.Linear);

        Debug.Log($"[ガイド更新] 表示内容：{targetText}");

        // 強制的に表示状態をリセット
        TitleHintText.color = new Color(TitleHintText.color.r, TitleHintText.color.g, TitleHintText.color.b, 1f);
        TitleHintText.enabled = true;
        Debug.Log($"[ガイド更新] テキスト設定完了：{TitleHintText.text}");
    }
}