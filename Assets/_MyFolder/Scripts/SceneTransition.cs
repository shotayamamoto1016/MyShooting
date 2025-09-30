using UnityEngine;
using Cysharp.Threading.Tasks;  
using DG.Tweening;
public class SceneTransition : MonoBehaviour
{

    //CanvasGroup保管用
    CanvasGroup canvas;

    //CanvasGroup取得用プロパティ
    public CanvasGroup Canvas
    {
        get
        {
            if (canvas == null)
            {
                canvas = GetComponent<CanvasGroup>();
            }

            return canvas;
        }
    }

    //画面を徐々に表示させる
    public void FadeIn(float duration)
    {
        Canvas.alpha = 1;

        Canvas.DOFade(0, duration)
            .SetLink(gameObject)
            .OnComplete(() => {
                Destroy(gameObject);
            });
    }

    //画面を徐々に白色にする
    public void FadeOut(float duration)
    {
        Canvas.alpha = 0;

        Canvas.DOFade(1, duration)
            .SetLink(gameObject);
    }

    //画面を徐々に表示させる（非同期処理）
    public async UniTask FadeInAsync(float duration)
    {
        bool isDone = false;

        Canvas.alpha = 1;

        Canvas.DOFade(0, duration)
            .SetLink(gameObject)
            .OnComplete(() => {
                isDone = true;
                Destroy(gameObject);
            });

        //フェードインが完了するまで待機
        await UniTask.WaitUntil(() => isDone == true);
    }

    //画面を徐々に白色にする（非同期処理）
    public async UniTask FadeOutAsync(float duration)
    {
        bool isDone = false;

        Canvas.alpha = 0;

        Canvas.DOFade(1, duration)
            .SetLink(gameObject)
            .OnComplete(() => {
                isDone = true;
            });

        //フェードインが完了するまで待機
        await UniTask.WaitUntil(() => isDone == true);
    }
}
