using UnityEngine;
using DG.Tweening;

public class Item : MonoBehaviour
{
    //アイテムタイプ
    public enum ItemType
    {
        //オプションがパワーアップ
        powerUp,
        //オプションが一つ増える
        option,
    }

    public ItemType itemtype = ItemType.powerUp;

    //移動開始フラグ
    bool moveStart;

   
    void Start()
    {
        //出撃時ふわっと上昇するアニメーション
        transform.DOMove(new Vector3(0, 0.5f, 0), 0.5f)
            .SetRelative(true)
            .SetEase(Ease.OutQuint)
            .OnComplete(() =>
            {
                //落下移動開始
                moveStart = true;
            });
    }

    
    void Update()
    {
        //落下移動
        if (moveStart)
        {
            transform.position += transform.up * -1 * Time.deltaTime;
        }
    }

    //なにかと触れたら
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            //プレイヤを廃棄
            Destroy(gameObject);
        }
    }
}
