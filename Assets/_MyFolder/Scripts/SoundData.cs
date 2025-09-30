using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundData", menuName = "ScriptableObjects/SoundData")]
public class SoundData : ScriptableObject
{
    //BGM定義
    public enum BgmType
    {
        none,
        start,
        boss,
        clear,
        gmameOver,
        title,
    }

    //SE定義
    public enum SeType
    {
        none, 
        shot,
        item1,
        item2,
        die,
        lazer,
        enemyShot,
        enemyDie,
        enemyDamage,
        bossShot1,
        bossShot2,
    }

    //BGMを管理するクラス
    [System.Serializable ]
    public class BgmSound
    {
        //タイプ
        public BgmType name = BgmType.none;

        //オーディタイプ
        public AudioClip clip;
    }

    //SE情報を管理するクラス
    [System .Serializable ]
    public class SeSound
    {
        //タイプ
        public SeType name = SeType.none;

        //オーディタイプ
        public AudioClip clip;
    }

    //BGM情報を保管するリスト
    public List<BgmSound> BgmSounds = new List<BgmSound>();

    //SE情報を保管するリスト
    public List<SeSound> SeSounds = new List<SeSound>();
}
