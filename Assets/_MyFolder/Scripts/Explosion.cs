using UnityEngine;

public class Explosion : MonoBehaviour
{
    //アニメーションが終了したら呼ぶイベント
    void OnAnimationFinish()
    {
        Destroy(gameObject);
    }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
    
}
