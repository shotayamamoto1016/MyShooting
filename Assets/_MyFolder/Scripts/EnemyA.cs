using Unity.VisualScripting;
using UnityEngine;

public class EnemyA : MonoBehaviour
{
    Vector3 moveDir;

    //敵のスピードを設定
    [SerializeField] float speed = 5f;

    //爆弾エフェクトプレハブを宣言
    [SerializeField] GameObject explosionPrefab;
  

   
    void Update()
    {
        

        //敵を下向きに動かす
        moveDir = new Vector3(0, -1, 0);

        //移動させる
        transform.position += moveDir * speed * Time.deltaTime;
    }

    //弾に当たったら敵を破壊する
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("当たる");
        if(other.gameObject.tag == "PlayerBullet")
        {
            Destroy(gameObject);

            //爆発エフェクトを作成
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            
        }
    }
}
