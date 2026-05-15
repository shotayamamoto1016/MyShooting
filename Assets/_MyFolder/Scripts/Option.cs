using UnityEngine;

public class Option : MonoBehaviour
{
    //プレイヤ
    PlayerController player;

    //遅延フレーム数
    public int delayFrame = 15;

    //行先に到達するまでの時間
    [SerializeField] float smoothTime = 0.05f;

    //オプションの移動先
    Vector3 target;

    //移動速度
    Vector2 currentVelocity;

    //弾プレハブ
    [SerializeField] GameObject bulletPrefab;

    //弾の発射位置
    [SerializeField] Transform shotPos;

    void Start()
    {
        //プレイヤ取得
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        //遅延フレーム分遅れたプレイヤの位置
        if(player .playerPosHistory .Count > delayFrame)
        {
            //プレイヤの履歴からコピー
            Vector3[] array = player.playerPosHistory.ToArray();

            //オプションの行先位置
            target = array[array.Length - delayFrame];
        }

        else
        {
            //プレイヤの履歴が数を満たしていない場合はプレイヤと同じ場所にオプションを生成
            target = player.transform.position;
        }

        //行先位置へスムーズに移動
        Vector2 currentPos = transform.position;

        currentPos = Vector2.SmoothDamp(currentPos, target, ref currentVelocity, smoothTime);

        transform.position = currentPos;
    }

    //弾を発射
    public void Shot()
    {
        Instantiate(bulletPrefab, shotPos.position, shotPos.rotation);
    }
}
