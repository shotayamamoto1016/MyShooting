using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;  
using TMPro;       

public class WaveClear : MonoBehaviour
{
    //クリアテキスト
    [SerializeField] TextMeshProUGUI clearText;

    void Start()
    {
        //メニューボタンを非表示 
        InGameMenuController menuController = Object.FindFirstObjectByType<InGameMenuController>();
        if (menuController != null)
            menuController.HideMenuButton();

        //文字を透明化
        Color color = clearText.color;

        color.a = 0;

        clearText.color = color;

        //DOTweenをまとめるシークエンス
        //SequenceはTweenを順番に並べて再生できるコンテナ
        //複数をアニメーションを順番に実行したいときに使う
        Sequence sequence = DOTween.Sequence();

        //フェードイン
        //1秒かけて透明度を1にする
        sequence.Append(clearText.DOFade(1f, 1f));

        //待機
        sequence.AppendInterval(5f);

        //フェードアウト
        //1秒かけて透明度を0にする(完全に透明)
        //.OnCompleteをつけてフェードアウト完了後に処理を追加
        sequence.Append(clearText.DOFade(0f, 1f)).OnComplete(() => {
            if (GameDirector.instance != null)
            {
                GameDirector.instance.OnStageClear();
            }
        });

        //DOTween実行
        //gameObjectがDestroyされたらシークエンスも廃棄する
        sequence.SetLink(gameObject).Play();
    }




}
