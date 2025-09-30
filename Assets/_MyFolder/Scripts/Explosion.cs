using UnityEngine;

public class Explosion : MonoBehaviour
{
    //アニメーションが終了したら呼ぶイベント
    void OnAnimationFinish()
    {
        Destroy(gameObject);
    }
      
    //サイズ変更
    //SetSize(2.0f)と呼び出すとsizeの値も2.0fになる
    public void SetSize(float size)
    {
        transform.localScale = new Vector3(size, size, size);
    }
    
}
