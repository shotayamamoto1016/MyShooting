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

        
    }

    //スタートボタンが押された時
    public async void OnPressStartButton()
    {
        //画面フェードアウト
        await Instantiate(sceneTransition).GetComponent<SceneTransition>().FadeOutAsync(1);

        SceneManager.LoadScene("02_Game");
    }
}
