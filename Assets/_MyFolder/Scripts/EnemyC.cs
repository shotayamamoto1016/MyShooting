using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading;

public class EnemyC : EnemyController
{
    [Header("EnemyC 固有設定")]
    [SerializeField] float horizontalDir = 1.0f; // 1で右斜め下、-1で左斜め下
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] List<Transform> shotPositions;

    [Header("攻撃リズム設定")]
    [SerializeField] int burstCount = 3;          // 1回に何連射するか
    [SerializeField] float shotDelay = 0.2f;      // 連射中の弾の間隔
    [SerializeField] float burstInterval = 2.0f;  // 次の3連射までの休み時間
    [SerializeField] float bulletSpeedRate = 1.0f; // 弾の速度倍率

    [Header("折り返し設定")]
    [SerializeField] float screenMargin = 0.5f; // 画面端からどれくらい手前で折り返すか

    private float currentXDir; // 現在の横移動方向
    private float screenLimitX; // 画面の端のX座標

    [SerializeField] float initialShotDelay = 2.0f;
    protected override void Start()
    {
        base.Start();

        // 最初の移動方向をセット
        currentXDir = horizontalDir;

        // 画面の右端のX座標を計算 (カメラの表示領域から算出)
        // ViewportToWorldPoint(1,0,0) で画面右端の座標が取れる
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));
        screenLimitX = rightEdge.x - screenMargin;

        // 攻撃ループを開始 (Destroy時に自動停止するトークンを渡す)
        ShotLoop(this.GetCancellationTokenOnDestroy()).Forget();
    }

    void Update()
    {
        // 右端に行き過ぎたら左へ
        if (transform.position.x > screenLimitX)
        {
            currentXDir = -1.0f;
        }
        // 左端に行き過ぎたら右へ
        else if (transform.position.x < -screenLimitX)
        {
            currentXDir = 1.0f;
        }

        // 移動方向を決定 (横は変化し、縦は常に下向き)
        Vector3 moveDir = new Vector3(currentXDir, -0.5f, 0).normalized;

        // 親クラスのMoveメソッドを利用して斜めに移動
        base.Move(moveDir);
    }

    
    private async UniTaskVoid ShotLoop(CancellationToken token)
    {
        // カメラに映るまで待機
        await UniTask.WaitUntil(() => isRendered, cancellationToken: token);

        //画面内に入ってから攻撃開始
        await UniTask.Delay((int)(initialShotDelay * 1000), cancellationToken: token);

        while (!token.IsCancellationRequested)
        {
            // カメラに映っていない間、またはゲーム停止中は攻撃しない
            if (!isRendered)
            {
                await UniTask.Yield(token);
                continue;
            }

            // 3連射
            for (int i = 0; i < burstCount; i++)
            {
                // 親クラスのShotメソッドを使用
                base.Shot(shotPositions, bulletPrefab, bulletSpeedRate);

                // 連射中の短い待ち時間
                await UniTask.Delay((int)(shotDelay * 1000), cancellationToken: token);
            }

            // 3連射した後の長いインターバル
            await UniTask.Delay((int)(burstInterval * 1000), cancellationToken: token);
        }
    }

    // ダメージ処理とSE
    protected override void Damage(int d)
    {
        base.Damage(d);

        if (currentHp <= 0)
        {
            string seName = SoundData.SeType.enemyDie.ToString();
            GSound.Instance.PlaySe(seName);
        }
        else
        {
            string seName = SoundData.SeType.enemyDamage.ToString();
            GSound.Instance.PlaySe(seName);
        }
    }
}