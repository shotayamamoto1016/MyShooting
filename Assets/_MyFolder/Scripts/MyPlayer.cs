using UnityEngine;

public class MyPlayer 
{
    //シングルトン化
    private static MyPlayer instance;

    public static MyPlayer Instance
    {
        get
        {
            if (instance == null) instance = new MyPlayer();

            return instance;
        }
    }

    //ステージ数
    public int STAGE_NUM;
}
