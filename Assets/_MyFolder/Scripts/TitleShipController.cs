using UnityEngine;
using DG.Tweening;

public class TitleShipController : MonoBehaviour
{
    [Header("時間設定")]
    public float fadeInDuration = 1.0f;
    public float goDeepDuration = 5.0f;
    public float returnDuration = 10.0f; 
    public float settleDuration = 0.25f;

    private Renderer[] shipRenderers;
    private Sequence seq;

    void Start()
    {
        shipRenderers = GetComponentsInChildren<Renderer>();

        transform.position = new Vector3(-6f, 0f, 20f);
        transform.eulerAngles = new Vector3(70f, 20f, 0f);
        transform.localScale = Vector3.one * 0.5f;

        SetAllAlpha(0f);

        var seq = DOTween.Sequence();
        seq.SetLink(gameObject);

        //フェードイン
        seq.Append(
            DOTween.To(() => 0f, SetAllAlpha, 1f, fadeInDuration)
        );

        //奥へ進む
        seq.Append(
            transform.DOMove(new Vector3(3.2f, 2.5f, 62f), goDeepDuration)
                .SetEase(Ease.Linear)
        );
        seq.Join(
            transform.DOScale(0.06f, goDeepDuration)
                .SetEase(Ease.Linear)
        );

        //弧を描いて戻る
        Vector3[] returnPath = new Vector3[]
        {
            new Vector3( 3.2f,  2.5f, 62f),
            new Vector3( 4.4f,  4f, 57f),
            new Vector3( 5.6f,  5.5f, 52f),
            new Vector3( 6.8f,  7.5f, 47f),
            new Vector3( 8f,  9.0f, 42f),
            new Vector3( 6f,  7.5f, 35f),
            new Vector3( 4f,  5.5f, 28f),
            new Vector3( 2f,  3.0f, 15f),
            new Vector3( 0f,  0.8f,   5f),
        };

        seq.Append(
            transform.DOPath(returnPath, returnDuration, PathType.CatmullRom)
                .SetEase(Ease.Linear)
        );
        seq.Join(
            transform.DOScale(1.0f, returnDuration)
                .SetEase(Ease.Linear)
        );

        // 弧が終わる前に向きが確定するようにする
        float rotationDelay = returnDuration * 0.001f;  
        float rotationChange = returnDuration * 0.4f;  

        seq.Join(
            DOVirtual.DelayedCall(rotationDelay, () =>
            {
                if (this == null || !gameObject.activeInHierarchy) return;
                transform.DORotate(new Vector3(-120f, 10f, 20f), rotationChange)
                    .SetEase(Ease.InOutSine)
                    .SetLink(gameObject);
            })
        );

        //停止バウンス
        seq.Append(
            transform.DOScale(1.05f, settleDuration)
                .SetEase(Ease.OutSine)
        );
        seq.Append(
            transform.DOScale(1.0f, settleDuration)
                .SetEase(Ease.InSine)
        );

        seq.Play();
    }

    void OnDestroy() 
    {
        seq?.Kill();
    }


    void SetAllAlpha(float alpha)
    {
        foreach (var r in shipRenderers)
        {
            foreach (var mat in r.materials)
            {
                Color c = mat.color;
                c.a = alpha;
                mat.color = c;
            }
        }
    }
}