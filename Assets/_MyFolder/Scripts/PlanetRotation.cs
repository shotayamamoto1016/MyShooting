using UnityEngine;
using DG.Tweening;

public class PlanetRotation : MonoBehaviour
{
    void Start()
    {
        // 自身のピボットを中心に、Z軸（2Dの回転）を無限ループで回す
        // RotateMode.LocalAxisAdd を使うのがポイントです
        transform.DOLocalRotate(new Vector3(0, 0, -360f), 10f, RotateMode.LocalAxisAdd)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear);
    }
}