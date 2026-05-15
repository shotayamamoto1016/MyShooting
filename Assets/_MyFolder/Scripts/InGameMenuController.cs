using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameMenuController : MonoBehaviour
{
    [Header("UI要素")]
    [SerializeField] private GameObject menuPopup;
    [SerializeField] private CanvasGroup overlayGroup;
    [SerializeField] private RectTransform popupPanel;
    [SerializeField] private GameObject confirmPopup;
    [SerializeField] private GameObject menuButton;

    //WaveStartの参照を保持 
    private WaveStart currentWaveStart;

    //GameDirectorから呼び出してWaveStartを登録する 
    public void SetWaveStart(WaveStart waveStart)
    {
        currentWaveStart = waveStart;
    }

    void Awake()
    {
        //起動時は非表示
        menuPopup.SetActive(false);
        confirmPopup.SetActive(false);
        if (overlayGroup != null) overlayGroup.alpha = 0;
    }

    //メニューボタンを押した時
    public void OnClickMenuButton()
    {
        // SE再生
        string seName = SoundData.SeType.botton1.ToString();
        GSound.Instance.PlaySe(seName);

        //ゲームを止める
        if (GameDirector.instance != null)
            GameDirector.instance.stopFlag = true;

        //DOTweenを全て一時停止
        DOTween.PauseAll();

        //時間を止める
        Time.timeScale = 0f;

        //点滅を一時停止
        if (currentWaveStart != null)
            currentWaveStart.PauseTween();


        //ポップアップをパッと表示
        menuPopup.SetActive(true);
    }

    //戦闘に戻るボタン
    public void OnClickResume()
    {
        //SE再生
        string seName = SoundData.SeType.botton1.ToString();
        GSound.Instance.PlaySe(seName);

        //ゲームを再開
        if (GameDirector.instance != null)
            GameDirector.instance.stopFlag = false;

        //DOTweenを全て再開 
        DOTween.PlayAll();

        //時間を再開 
        Time.timeScale = 1f;

        //点滅を再開 
        if (currentWaveStart != null)
            currentWaveStart.ResumeTween();

        //ポップアップをパッと非表示
        menuPopup.SetActive(false);
    }

    //タイトルへ戻るボタン
    public void OnClickReturnTitle()
    {
        //SE再生
        string seName = SoundData.SeType.botton1.ToString();
        GSound.Instance.PlaySe(seName);

        //確認ポップアップを表示
        confirmPopup.SetActive(true);
    }

    //確認ポップアップのはいボタン
    public async void OnClickYes()
    {
        string seName = SoundData.SeType.botton1.ToString();
        GSound.Instance.PlaySe(seName);

        //シーン遷移前にTimeScaleを戻す 
        Time.timeScale = 1f;


        await UniTask.Delay(300);

        DOTween.KillAll();
        SceneManager.LoadScene("01_Select");
    }

    //確認ポップアップのいいえボタン
    public void OnClickNo()
    {
        string seName = SoundData.SeType.botton1.ToString();
        GSound.Instance.PlaySe(seName);

        //確認ポップアップを閉じる
        confirmPopup.SetActive(false);
    }

    //メニューボタンを非表示にする
    public void HideMenuButton()
    {
        if (menuButton != null)
            menuButton.SetActive(false);
    }
}