using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    //コライダーの範囲から敵が出たら敵を破壊する
    private void OnTriggerExit2D(Collider2D other)
    {
        Destroy(other.gameObject);
    }
    
}
