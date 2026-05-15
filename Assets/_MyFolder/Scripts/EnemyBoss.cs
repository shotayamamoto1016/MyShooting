using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyBoss : EnemyController
{
    //進行方向
    Vector3 moveDir;

    //水平方向の動きを開始する初期座標
    Vector3 firstPos;
    float horizontalTime;

    //水平方向の動きのスピード
    [SerializeField] float horizontalSpeed = 1;

    //水平方向の動く幅
    [SerializeField] float horizontalWidth = 1;

    //レーザー弾のプレハブ
    [SerializeField] GameObject laserPrefab;

    //レーザーを発射する間隔
    [SerializeField] float laserInterval = 5f;

    float laserDelta;

    //レーザーの発射位置
    [SerializeField] List<Transform> laserPositions;

    //連射弾プレハブ
    [SerializeField] GameObject rotateBulletPrefab;

    //連射攻撃する間隔
    [SerializeField] float rapidshotinterval = 5f;

    //連射弾を連射する間隔
    [SerializeField] float rapidInterval = 0.1f;

    //連射弾を一回の攻撃で連射する間隔
    [SerializeField] int rapidCount = 3;

    float rapidDelta;

    //連射弾の発射位置
    [SerializeField] List<Transform> rapidPositions;

    //連射弾開始フラグ
    bool rapidFlag;

    //前進モードフラグ
    bool moveForward = true;

    protected override  void Start()
    {
        base.Start();

        //初期座標
        firstPos = transform.position;
    }

   
    void Update()
    {
        //前進モード
        if (moveForward)
        {
            //下方向へ移動
            moveDir = new Vector3(0, -1, 0);

            //親クラスを継承
            base.Move(moveDir);

            //所定の位置まで行ったら前進モード終了
            if (transform.position.y < 3)
            {
                moveForward = false;
            }
        }

        else
        {
            //水平方向の動き
            Vector3 horizontalPos = transform.position;

            horizontalTime += Time.deltaTime;

            //Mathf.Sin()は三角関数のサイン波であり-1～1の値を周期的に変動する
            //firstPosを中心に、左右移動させる
            horizontalPos.x = horizontalWidth * Mathf.Sin(horizontalTime * horizontalSpeed) + firstPos.x;

            moveDir = horizontalPos - transform.position;

            base.Move(moveDir);

            //レーザーを一定間隔で発射する
            laserDelta += Time.deltaTime;

            if (laserDelta > laserInterval)
            {
                Shot(laserPositions, laserPrefab);
                laserDelta = 0;

                //ボスショット１
                string seName = SoundData.SeType.bossShot1.ToString();

                GSound.Instance.PlaySe(seName);
            }

            //連射弾を一定間隔で発射する
            if (rapidFlag)
            {
                rapidDelta += Time.deltaTime;

                if (rapidDelta > rapidshotinterval)
                {
                    //連射開始
                    RapidShot();

                    rapidDelta = 0;

                    //ボスショット２
                    string seName = SoundData.SeType.bossShot2.ToString();

                    GSound.Instance.PlaySe(seName);
                }
            }
        }
    }

    //連射弾開始
     async void RapidShot()
    {

        //弾速度を変化させる倍率
        float speedRate = 1.75f;

        //rapidCountの数だけ繰り返す
        for(int i = 0; i < rapidCount; i++)
        {
            //発射
            Shot(rapidPositions, rotateBulletPrefab, speedRate );

            //拡散弾連射間隔待つ
            await UniTask.Delay((int)(rapidInterval * 1000f));
            

            //弾速度を変化させる倍率を減少させる
            speedRate *= 0.75f;
        }
    }

    //ダメージ
    protected override void Damage(int d)
    {
        //前進モード中はダメージを食らわない
        if (moveForward) d = 0; ;

        //親クラスのDamageメソッド
        base.Damage(d);

        Debug.Log($"HP >> {base.currentHp} / {base.maxHp}");

        //現在のHPの割合
        //整数同士の割り算だと少数以下は切り捨てられるためキャスト(float)を入れる
        float hpratio = (float)base.currentHp / (float)base.maxHp;

        //残50%をきった
        if (hpratio < 0.5f)
        {
            rapidFlag = true;
            Debug.Log("連射弾モード発動");
        }

        if (currentHp <= 0)
        {
            //消滅したときのSE
            string seName = SoundData.SeType.enemyDie.ToString();

            GSound.Instance.PlaySe(seName);
        }

        else
        {
            //ダメージを受けたときのSE
            string seName = SoundData.SeType.enemyDamage.ToString();

            GSound.Instance.PlaySe(seName);
        }
    }
}
