using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleDirector : MonoBehaviour
{
    //BGM名
    string bgmName;

    //シーン遷移アニメーションプレハブ
    [SerializeField] GameObject sceneTransition;

    private void Start()
    {
        //ステージBGM
        bgmName = SoundData.BgmType.title.ToString();

        GSound.Instance.PlayBgm(bgmName, true);

        // SE音量を上げる
        GSound.Instance.seVolume = 3.0f;
    }

    private void LateUpdate()
    {
        GSound.Instance.CheckSeQueue();
    }

    //スタートボタンが押された時
    public async void OnPressStartButton()
    {
        string seName = SoundData.SeType.botton1.ToString();
        GSound.Instance.PlaySe(seName);

        //画面フェードアウト
        await Instantiate(sceneTransition).GetComponent<SceneTransition>().FadeOutAsync(1);

        SceneManager.LoadScene("01_Select");
    }
}
