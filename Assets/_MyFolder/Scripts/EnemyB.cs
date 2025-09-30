using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class EnemyB : EnemyController
{
    //進行方向
    Vector3 moveDir;
    
    //弾プレハブ
    [SerializeField] GameObject bulletPrefab;

    //弾を打つ間隔
    [SerializeField] float shotInterval = 3f;
    float shotDelta;

    [SerializeField] List<Transform> shotPositions;

    protected override void Start()
    {

        base.Start();
    }

   
    void Update()
    {
        //移動方向
        moveDir = new Vector3(0, -1, 0);

        base.Move(moveDir);

        if (base.isRendered)
        {
            //弾を一定間隔で発射する
            shotDelta += Time.deltaTime;

            if (shotDelta > shotInterval)
            {

                Shot(shotPositions, bulletPrefab);
                shotDelta = 0;

                //敵ショットSE
                string seName = SoundData.SeType.enemyShot.ToString();

                GSound.Instance.PlaySe(seName);
            }
        }
    }

    //ダメージ処理をオーバーライドしてSE処理のみ子クラス側で実装
    protected override void Damage(int d)
    {
        base.Damage(d);

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
