using Unity.VisualScripting;
using UnityEngine;



public class EnemyA : EnemyController
{
    Vector3 moveDir;

    [SerializeField] int maxHP = 3;
    
    protected override void Start()
    {
        //親クラスのStart()を実行
        base.Start();
    }

    void Update()
    {       
        //敵を下向きに動かす
        moveDir = new Vector3(0, -1, 0);

        //親クラスのMoveメソッドを呼び出す
        base.Move(moveDir);
    }

    //ダメージ処理をオーバーライドしてSE処理のみ子クラス側で実装
    protected override void Damage(int d)
    {
        base.Damage(d);

        if(currentHp <= 0)
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
