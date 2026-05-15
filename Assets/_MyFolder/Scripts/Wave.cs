using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading;

public class Wave : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawn
    {
        public Transform spawnPos;
        public GameObject enemyPrefab;
        public float delay;
    }

    [SerializeField] string detail;
    [SerializeField] EnemySpawn[] enemySpawns;
    CancellationTokenSource cancelToken;

    //デバッグ用フラグ
    private bool hasStarted = false;

    void OnEnable()
    {
        Debug.Log($"[Wave] OnEnable呼び出し - hasStarted: {hasStarted}");

        //既に開始済みなら再実行
        if (!hasStarted)
        {
            hasStarted = true;
            StartWaveAsync().Forget();
        }
        else
        {
            Debug.LogWarning("[Wave] 既に開始済みのため、再実行します");
            StartWaveAsync().Forget();
        }
    }

    async UniTaskVoid StartWaveAsync()
    {
        Debug.Log("[Wave] StartWaveAsync開始");

        //前回のトークンが残っていたら破棄
        if (cancelToken != null)
        {
            Debug.Log("[Wave] 既存のトークンをキャンセル");
            cancelToken.Cancel();
            cancelToken.Dispose();
        }

        //新しいトークンを作成
        cancelToken = new CancellationTokenSource();
        CancellationToken token = cancelToken.Token;

        Debug.Log($"[Wave] enemySpawnsの数: {enemySpawns.Length}");

        List<UniTask> spawnTasks = new List<UniTask>();

        //敵を出す
        foreach (EnemySpawn spawn in enemySpawns)
        {
            Debug.Log($"[Wave] Spawn追加 - enemy: {spawn.enemyPrefab?.name}, delay: {spawn.delay}");
            spawnTasks.Add(Spawn(spawn, token));
        }

        try
        {
            Debug.Log("[Wave] 全てのスポーンタスク開始");
            await UniTask.WhenAll(spawnTasks);
            Debug.Log("[Wave] 全てのスポーンタスク完了");

            //敵が生成されるまで少し待つ
            await UniTask.Delay(100, cancellationToken: token);
            Debug.Log($"[Wave] 子オブジェクト数: {transform.childCount}");

            //敵がいなくなるまで待機
            await UniTask.WaitUntil(() => transform.childCount == 0, PlayerLoopTiming.Update, token);
            Debug.Log("[Wave] 全ての敵が倒された");

            //キャンセルされていなければ破棄
            if (!token.IsCancellationRequested)
            {
                Debug.Log("[Wave] Wave完了 - オブジェクトを破棄");
                Destroy(gameObject);
            }
        }
        catch (System.OperationCanceledException)
        {
            Debug.Log("[Wave] Waveの処理が正常にキャンセルされました");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[Wave] エラー発生: {e.Message}\n{e.StackTrace}");
        }
    }

    async UniTask Spawn(EnemySpawn spawn, CancellationToken token)
    {
        Debug.Log($"[Spawn] 開始 - delay: {spawn.delay}秒");

        //指定の時間待つ
        await UniTask.Delay((int)(spawn.delay * 1000f), cancellationToken: token);

        Debug.Log($"[Spawn] 待機完了 - enemy: {spawn.enemyPrefab?.name}");

        if (this == null || token.IsCancellationRequested)
        {
            Debug.LogWarning("[Spawn] キャンセルされたため中断");
            return;
        }

        if (spawn.spawnPos != null)
        {
            GameObject enemy = Instantiate(spawn.enemyPrefab, spawn.spawnPos.position, Quaternion.identity);
            enemy.transform.SetParent(this.transform);

            Debug.Log($"[Spawn] 敵生成成功: {enemy.name} at {spawn.spawnPos.position}");

            var controller = enemy.GetComponent<EnemyController>();
          
          
            Destroy(spawn.spawnPos.gameObject);
        }
        else
        {
            Debug.LogError("[Spawn] spawnPosがnullです！");
        }
    }

    void OnDestroy()
    {
        Debug.Log("[Wave] OnDestroy呼び出し");
        if (cancelToken != null)
        {
            cancelToken.Cancel();
            cancelToken.Dispose();
        }
    }

    private void OnDisable()
    {
        Debug.Log("[Wave] OnDisable呼び出し");
        if (cancelToken != null)
        {
            cancelToken.Cancel();
        }
    }
}