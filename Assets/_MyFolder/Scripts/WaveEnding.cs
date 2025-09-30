using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;  
using TMPro;        


public class WaveEnding : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreLabel;

    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] TextMeshProUGUI endingText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Color color;

        //スコアラベルを透明で初期化
        color = scoreLabel.color;

        color.a = 0;

        scoreLabel.color = color;

        //スコアテキストを透明で初期化
        color = scoreText.color;

        color.a = 0;

        scoreText.color = color;

        //エンディングメッセージを透明で初期化
        color = endingText.color;

        color.a = 0;

        endingText.color = color;

        //DOTween処理をまとめるシークエンス
        Sequence sequence = DOTween.Sequence();

        //スコアラベルをフェードイン
        sequence.Append(scoreLabel.DOFade(1f, 1f));

        //待機
        sequence.AppendInterval(1f);

        //スコアテキストをフェードイン
        sequence.Append(scoreText.DOFade(1f, 1f));

        //待機
        sequence.AppendInterval(2f);

        //エンディングメッセージをフェードイン
        sequence.Append(endingText.DOFade(1f, 1f));

        //待機
        sequence.AppendInterval(5f);

        //エンディングメッセージをフェードアウト
        sequence.Append(endingText.DOFade(0f, 1f));

        //スコアテキストをフェードアウト
        sequence.Join(scoreText.DOFade(0f, 1f));

        //スコアラベルをフェードアウト
        sequence.Join(scoreLabel.DOFade(0f, 1f)).OnComplete(() =>{
            Destroy(gameObject);
        });

        //DOTween実行
        sequence.SetLink(gameObject).Play();
    }

   //スコアをセット
   public void SetScore( int score)
    {
        scoreText.text = score.ToString("D0");
    }
}
