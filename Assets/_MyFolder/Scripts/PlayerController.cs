using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    //移動速度
    //[SerializeField]はprivateでもインスペクターに表示・保存可能にする
    //publicより安全でほかのスクリプトからアクセスできない
    [SerializeField] private float speed = 5f;

    //弾プレハブ
   [SerializeField] GameObject bulletPrefab;

   //連射速度
   [SerializeField] float shotInterval = 3.0f;
   float delta;

   //弾の発射位置
   [SerializeField] Transform singleShot;
    
    //爆弾エフェクトプレハブを宣言
    [SerializeField] GameObject explosionPrefab;

    //参照
    Animator _animator;

    //画面制限域
    Vector3 screenSize;
    Vector3 worldSize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //コンポーネント取得
        _animator = GetComponent<Animator>();

        //スクリーンサイズをピクセル座標に変換する
         screenSize = new Vector3(Screen.width, Screen.height, 0);
        //ピクセル座標をワールド座標へ変換
        //ScreenToWorldPointとはカメラが見ているスクリーン(ピクセル)上の座標を、ゲーム内のワールド空間に変換する関数。
         worldSize = Camera.main.ScreenToWorldPoint(screenSize);
    }

    // Update is called once per frame
    void Update()
    {
        delta += Time.deltaTime;

        //左右入力値
        //Input.GetAxisRaw("Horizontal")はA / D,左右キーの入力値を取得する
        float moveX = Input.GetAxisRaw("Horizontal");

        //上下入力値
        //Input.GetAxisRaw("Vertical")はW / S.上下キーの入力値を取得する
        float moveY = Input.GetAxisRaw("Vertical");

        //現在の位置から毎フレームごと同じ速度で移動する
        transform.Translate(moveX * speed * Time.deltaTime, moveY * speed * Time.deltaTime, 0);

        //左右移動アニメーション
        //アニメーターにMoveHという値を渡すことで、moveXの値が変わるとアニメーションも切り替えられる
        _animator.SetFloat("MoveH", moveX);

        //弾を発射　
        //左Ctrl(Macはcontrolキー及びマウス左クリック及びZキー)
         if (Input.GetButton("Fire1") || Input.GetKey(KeyCode.Z))
        {
            if (delta > shotInterval)
          {
              //弾をSingleShotの位置と向きで生成
              Instantiate(bulletPrefab, singleShot.position, singleShot.rotation);
              delta = 0;
          }
        }
        

        //画面外にでないように制限
         Vector3 playerPos = transform.position;
        //X（左右）の位置を -worldSize.x ? worldSize.x の範囲に制限
        //Mathf.Clamp(value,min,max)は、valueがminより小さいとminを返し、valueがmaxより大きいとmaxを返す
        playerPos.x = Mathf.Clamp(playerPos.x, -worldSize.x, worldSize.x);
         playerPos.y = Mathf.Clamp(playerPos.y, -worldSize.y, worldSize.y);
         transform.position = playerPos;
    }

    //敵と触れたら自分を破壊する
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);

            //爆弾エフェクトを作成
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
    }
}
