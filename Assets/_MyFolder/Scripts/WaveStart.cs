using UnityEngine;
//DOTween使用時に追加
using DG.Tweening;
using TMPro;

public class WaveStart : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI stageText;

    [SerializeField] TextMeshProUGUI startText;

    private Tween flashTween;
    
    void Start()
    {
        //DOTween処理　3秒間に7回点滅
        //DOColorは色を一定時間で変化させる拡張メソッド
        //RGBA(0,0,0,0)完全に透明な黒
        startText.DOColor(new Color(0, 0, 0, 0), 3f)
            //3秒間に7回点滅しながら透明化
          .SetEase(Ease.Flash, 7)
          //Tweenとunityオブジェクトをリンクさせる
          .SetLink(gameObject)
          //Tweenが完了したら呼ばれるコールバック
          .OnComplete(() =>
          {
              //点滅処理完了後このWaveを削除
              Destroy(gameObject);
          });
    }


    //点滅を一時停止
    public void PauseTween()
    {
        flashTween?.Pause();
    }

    //点滅を再開
    public void ResumeTween()
    {
        flashTween?.Play();
    }

    //ステージ数をセット
    public void SetStageNum(int stageNum)
    {
        stageText.text = "STAGE " + stageNum;
    }
}
