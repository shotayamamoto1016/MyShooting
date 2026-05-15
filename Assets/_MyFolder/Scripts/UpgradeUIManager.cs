using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UpgradeUIManager : MonoBehaviour
{
    [SerializeField] private RectTransform upgradePanel; //下から出てくるパネル
    [SerializeField] private CanvasGroup overlayGroup;   //背景の暗幕（CanvasGroupを付けておくとフェードが楽です）
    [SerializeField] private float slideDuration = 0.5f; //アニメーション時間

    private Vector2 hiddenPosition = new Vector2(0, -1200); //画面外（下）の座標
    private Vector2 visiblePosition = new Vector2(0, -35);    //画面中央の座標

    void Awake()
    {
        //起動時はパネルを画面外へ飛ばし、暗幕を透明にしておく
        upgradePanel.anchoredPosition = hiddenPosition;
        if (overlayGroup != null) overlayGroup.alpha = 0;

        //最初はCanvas自体を非表示にしておく
        gameObject.SetActive(false);
    }

    //タイトル画面の「強化ボタン」からこれを呼ぶ
    public void OpenUpgradeSystem()
    {
        //SE再生 
        string seName = SoundData.SeType.botton1.ToString();
        GSound.Instance.PlaySe(seName);


        gameObject.SetActive(true);

        //自機の動きとショットを止める
        if (GameDirector.instance != null)
        {
            GameDirector.instance.stopFlag = true;
        }

        //Canvasを表示
        gameObject.SetActive(true);

        //背景をふわっと暗くする
        overlayGroup.DOFade(1f, slideDuration);

        //パネルを下からスッと出す
        upgradePanel.DOAnchorPos(visiblePosition, slideDuration)
                    .SetEase(Ease.OutCubic); //少し飛び出すような気持ちいい動き
    }

    //閉じるボタンからこれを呼ぶ
    public void CloseUpgradeSystem()
    {
        //SE再生 
        string seName = SoundData.SeType.botton1.ToString();
        GSound.Instance.PlaySe(seName);

        //背景を透明に戻す
        overlayGroup.DOFade(0f, slideDuration);

        //パネルを下に隠す
        upgradePanel.DOAnchorPos(hiddenPosition, slideDuration)
                    .SetEase(Ease.InCubic)
                    .OnComplete(() => {
                        //アニメーション完了後にCanvasを非表示にする
                        gameObject.SetActive(false);

                        //自機の操作を再開させる
                        if (GameDirector.instance != null)
                        {
                            GameDirector.instance.stopFlag = false;
                        }
                    });
    }
}