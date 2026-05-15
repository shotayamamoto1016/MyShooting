using UnityEngine;

public class StageSettings : MonoBehaviour
{
    // 第何ステージか
    public int stageNumber; 

    [Header("スコアしきい値")]
    public int scoreForStar1 = 1000;
    public int scoreForStar2 = 2000;
    public int scoreForStar3 = 3000;

    [Header("獲得コイン枚数")]
    public int coinsForStar1 = 100;
    public int coinsForStar2 = 200;
    public int coinsForStar3 = 300;

    // 現在のスコアから「星の数」を計算する
    public int GetStars(int score)
    {
        if (score >= scoreForStar3) return 3;
        if (score >= scoreForStar2) return 2;
        if (score >= scoreForStar1) return 1;
        return 0;
    }

    // 現在のスコアから「コイン枚数」を計算する
    public int GetCoins(int score)
    {
        if (score >= scoreForStar3) return coinsForStar3;
        if (score >= scoreForStar2) return coinsForStar2;
        if (score >= scoreForStar1) return coinsForStar1;
        return 0;
    }

    
   
}
