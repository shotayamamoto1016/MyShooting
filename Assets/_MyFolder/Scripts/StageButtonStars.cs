using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StageButtonStars : MonoBehaviour
{
    public int stageNumber;

    //3つの星の画像を表示
    public GameObject[] stars;

    //星の元のサイズを保存
    private Vector3[] originalStarScales;

    //鍵の画像
    public GameObject lockImage;

    private Button myButton;

    //色の設定
    private Color lockedColor = new Color(0.5f, 0.5f, 0.5f, 1.0f); 
    private Color normalColor = Color.white;

    private Image buttonImage;


    void Awake()
    {
        myButton = GetComponent<Button>();
        buttonImage = GetComponent<Image>();

        //初期色を白に
        if (buttonImage != null)
        {
            buttonImage.color = Color.white;
        }

        originalStarScales = new Vector3[stars.Length];
        for (int i = 0; i < stars.Length; i++)
        {
            if (stars[i] != null)
            {
                originalStarScales[i] = stars[i].transform.localScale;
            }
        }
    }

    void OnEnable() //画面が開くたびに実行
    {
        if (DataManager.instance == null) return;

        if (buttonImage != null)
        {
            buttonImage.DOKill();
        }
        if (lockImage != null)
        {
            lockImage.transform.DOKill();
        }

        int savedStars = DataManager.instance.GetStars(stageNumber);
        Debug.Log($"[ボタン{stageNumber}] 保存されている星の数: {savedStars}");
        Debug.Log($"[DataManager] 最後にクリアした番号: {DataManager.instance.lastClearedStageNumber}, フラグ: {DataManager.instance.isFirstClearToday}");



        //データの取得
        int currentBestStars = DataManager.instance.GetStars(stageNumber); //今の最高記録
        int prevStars = DataManager.instance.prevStarCount;             //プレイ前の記録
        bool isUnlocked = stageNumber <= DataManager.instance.clearedStageIndex + 1;

        if (myButton != null) myButton.interactable = isUnlocked;

        if (buttonImage != null)
        {
            //解放されていれば通常の色、ロックされていれば暗い色にする
            buttonImage.color = isUnlocked ? normalColor : lockedColor;
        }

        if (lockImage != null)
        {
            lockImage.SetActive(!isUnlocked);
        }


        // 今クリアして戻ってきたボタンの場合（星のアニメーション）
        if (DataManager.instance.isFirstClearToday && DataManager.instance.lastClearedStageNumber == stageNumber)
        {
            if (lockImage != null) lockImage.SetActive(false);

            if (buttonImage != null) buttonImage.color = normalColor;

            SetStarsDisplay(prevStars); //最初は前回の星を表示する

            //星のアニメーションと報酬の連続演出を開始
            PlayRewardSequence(prevStars, currentBestStars);
        }
        //今まさに解放されたボタンの場合（鍵開けアニメーション）
        else if (isUnlocked && DataManager.instance.justUnlocked && stageNumber == DataManager.instance.clearedStageIndex + 1)
        {
            

            //ここでアニメーションを開始し初期状態は暗い色にしておく
            if (buttonImage != null) buttonImage.color = lockedColor;

            //鍵を表示したまま演出へ
            if (lockImage != null) lockImage.SetActive(true);

            //星はまだ0個のはずなので、非表示にしておく
            SetStarsDisplay(0);

            PlayUnlockAnimation();
        }

        else
        {
            if (lockImage != null) lockImage.SetActive(!isUnlocked);
            if (buttonImage != null) buttonImage.color = isUnlocked ? normalColor : lockedColor;
            SetStarsDisplay(currentBestStars); //現在の最高星数をそのまま表示
        }

    }

    //星をアニメーションなしでパッと表示するための共通メソッド
    void SetStarsDisplay(int count)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            if (stars[i] != null)
            {
                stars[i].SetActive(i < count);
                if (i < count) stars[i].transform.localScale = originalStarScales[i];
            }
        }
    }


    //新しく獲得した星だけを順番に出す演出
    void PlayStarAnimation(int oldStars, int newStars)
    {
        Sequence seq = DOTween.Sequence().SetUpdate(true);

        for (int i = 0; i < stars.Length; i++)
        {
            if (stars[i] == null) continue;
            stars[i].transform.DOKill();

            if (i < oldStars)
            {
                //すでに持っていた星は最初から表示しておく
                stars[i].SetActive(true);
                stars[i].transform.localScale = originalStarScales[i];
            }
            else if (i < newStars)
            {
                //今回新しく獲得した星だけ、サイズ0から順番にアニメーション
                stars[i].SetActive(true);
                stars[i].transform.localScale = Vector3.zero;

                int index = i;
                seq.Append(stars[index].transform.DOScale(originalStarScales[index] * 1.5f, 0.4f).SetEase(Ease.OutBack));
                seq.Append(stars[index].transform.DOScale(originalStarScales[index], 0.1f));
                seq.AppendInterval(0.1f);
            }
            else
            {
                //まだ未獲得の星は非表示
                stars[i].SetActive(false);
            }
        }

        seq.SetLink(gameObject).Play();
    }

    //鍵が揺れて消える演出
    void PlayUnlockAnimation()
    {
        if (lockImage == null) return;

        lockImage.SetActive(true);
        if (myButton != null) myButton.interactable = false;

        Sequence seq = DOTween.Sequence();
        seq.SetUpdate(true);

        seq.AppendInterval(3.5f);

        seq.AppendCallback(() => {
            if (myButton != null) myButton.interactable = true;
            Debug.Log("鍵開け演出を開始します");
        });

        seq.Append(lockImage.transform.DOShakePosition(1.0f, 10f, 10));

        if (buttonImage != null)
        {
            seq.Join(buttonImage.DOColor(normalColor, 1.0f));
        }

        seq.Append(lockImage.transform.DOScale(0f, 0.5f).SetEase(Ease.InBack));
        seq.OnComplete(() => {
            //終わったらボタンを有効化し、鍵を完全に消す
            if (myButton != null) myButton.interactable = true;
            lockImage.SetActive(false);
            DataManager.instance.justUnlocked = false;

            //アニメーション完了後も念のためnormalColorに設定
            if (buttonImage != null)
            {
                buttonImage.color = normalColor;
            }
        });

        seq.SetLink(gameObject);
        seq.Play();
    }

    async void PlayRewardSequence(int oldStars, int newStars)
    {
        
        int earnedCoins = DataManager.instance.lastEarnedCoins;
        int newTotal = DataManager.instance.totalCoins;
        int oldTotal = newTotal - earnedCoins; //増える前の合計値

        //まず星のアニメーション
        PlayStarAnimation(oldStars, newStars);

        //星が出るのを待つ
        int addedStars = newStars - oldStars;
        int waitTime = Mathf.Max(1500, addedStars * 600); 
        await UniTask.Delay(waitTime);

        //コインを飛ばす演出
        if (earnedCoins > 0)
        {
            CoinFlyEffect flyEffect = Object.FindAnyObjectByType<CoinFlyEffect>();
            if (flyEffect != null)
            {
                //コイン枠へ飛ばす
                await flyEffect.PlayCoinFlyAnimation(transform.position, oldTotal, earnedCoins);
            }

            //最後に合計金額表示
            CoinDisplay coinDisplay = Object.FindAnyObjectByType<CoinDisplay>();
            if (coinDisplay != null)
            {
                //カウントアップ開始
                coinDisplay.StartCountUp(oldTotal, newTotal);
            }
        }

        //すべての演出が終わったらフラグを下ろす
        DataManager.instance.isFirstClearToday = false;
    }

}

