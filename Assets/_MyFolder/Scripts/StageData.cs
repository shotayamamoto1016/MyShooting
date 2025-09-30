using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "ScriptableObjects/Stage")]
public class StageData : ScriptableObject 
{
    //Waveの種類
    public enum  WaveType
    {
        enemy,
        boss,
        start, 
        clear, 
        ending,
    }
    //ステージ内にセットするWave情報
    [System.Serializable]
    public class WaveInfo
    {
        //Wave種類
        public WaveType waveType;

        //Waveプレハブ
        public GameObject wavePrefab;
        //Waveを生成するまでの待機時間
        public float delay;
        //すべての敵を倒さないと次へいけないフラグ
        public bool completFlg;
    }

    //ステージを構成するWave情報のリスト
    public List<WaveInfo> stage = new List<WaveInfo>();

    //背景プレハブ
   // public GameObject background;


}
