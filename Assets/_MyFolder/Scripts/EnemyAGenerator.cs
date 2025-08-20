using UnityEngine;

public class EnemyAGenerator : MonoBehaviour
{
    //“GƒvƒŒƒnƒu‚ð¶¬
    public GameObject EnemyAPrefab;

    float span = 1.0f;

    float delta = 0;

    void Update()
    {
        this.delta += Time.deltaTime;

        if (this.delta > this.span)
        {
            GameObject go = Instantiate(EnemyAPrefab);

            int px = Random.Range(-2, 3);

            go.transform.position = new Vector3(px, 6, 0);

            this.delta = 0;
        }
    }
}
