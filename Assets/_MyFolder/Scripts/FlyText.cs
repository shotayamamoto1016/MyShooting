using UnityEngine;
using TMPro;
using DG.Tweening;

public class FlyText : MonoBehaviour
{
    TextMeshPro textMesh;

    //上昇時間
    [SerializeField] float duration = 0.5f;

    //上昇幅
    [SerializeField] float moveY = 0.5f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //DOTweenで移動アニメーション
        transform.DOMove(new Vector3(0, moveY, 0), duration)
            .SetRelative(true)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                //フライテキストを消去
                Destroy(gameObject);
            });
    }

   //テキストをセット
   public void SetText(string str)
    {
        textMesh = GetComponent<TextMeshPro>();

        textMesh.text = str;
    }
}
