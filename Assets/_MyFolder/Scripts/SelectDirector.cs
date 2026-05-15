using UnityEngine;

public class SelectDirector : MonoBehaviour
{
    // BGM名
    string bgmName;

    private void Start()
    {
        // ステージ選択BGM
        bgmName = SoundData.BgmType.select.ToString();
        GSound.Instance.PlayBgm(bgmName, true);
    }

    private void LateUpdate()
    {
        GSound.Instance.CheckSeQueue();
    }
}