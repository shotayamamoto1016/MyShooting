using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class CoinFlyEffect : MonoBehaviour
{
    [SerializeField] GameObject coinPrefab; 
    [SerializeField] Transform targetTransform; //飛ばし先の「コイン表示枠」のトランスフォーム
    [SerializeField] int coinCount = 10; //飛ばす枚数
    [SerializeField] float spreadRadius = 40f; //散らばる範囲

    public async UniTask PlayCoinFlyAnimation(Vector3 startPos, int startAmount, int addAmount)
    {
        List<GameObject> coins = new List<GameObject>();

        //指定した枚数のコインを生成
        for (int i = 0; i < coinCount; i++)
        {
            GameObject coin = Instantiate(coinPrefab, transform); // Canvasの子にする
            coin.transform.position = startPos;
            coin.transform.localScale = Vector3.zero;
            coins.Add(coin);
        }

        //コインをバラバラと散らしてから飛ばす
        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < coins.Count; i++)
        {
            GameObject coin = coins[i];

            // 散らばる範囲をspreadRadiusで制御
            Vector3 randomPos = startPos + (Vector3)Random.insideUnitCircle * spreadRadius;

            sequence.Insert(0, coin.transform.DOScale(1.0f, 0.3f).SetEase(Ease.OutBack)); // スケールも1.0くらいに
            sequence.Insert(0, coin.transform.DOMove(randomPos, 0.5f).SetEase(Ease.OutQuart));

            // 目標地点へ飛ばす（少しずつ時間をずらす）
            float delay = 0.5f + (i * 0.05f);
            sequence.Insert(delay, coin.transform.DOMove(targetTransform.position, 0.8f).SetEase(Ease.InBack));
            sequence.Insert(delay + 0.5f, coin.transform.DOScale(0f, 0.3f));

            
            // sequence.InsertCallback(delay + 0.8f, () => PlayCoinSE());
        }

        //全てのコインが飛び終わるのを待つ
        await sequence.SetLink(gameObject).Play().AsyncWaitForCompletion();

        //数字をジャラジャラ増やすカウントアップ演出
        foreach (var c in coins) Destroy(c);
    }
}