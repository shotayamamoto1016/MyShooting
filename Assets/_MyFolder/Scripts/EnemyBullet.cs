using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    //弾のスピード
    [SerializeField] float speed = 5f;

    //弾の速度を増滅させる
    public void SpeedRate(float speedRate)
    {
        speed *= speedRate;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //移動
        transform.position += -transform.up * speed * Time.deltaTime;
    }

    //なにかと当たったら
    private void OnTriggerEnter2D(Collider2D other)
    {
        //敵と当たったら消滅させる
        if(other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
