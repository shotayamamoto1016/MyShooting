using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //出撃確認ポップアップ
    [Header("Popup Reference")]
    [SerializeField] private StageStartPopup startPopup;

    [Header("Stage Buttons")]
    public Button ButtonOne;
    public Button ButtonTwo;
    public Button ButtonThree;
    public Button ButtonFor;
    public Button ButtonFive;
    public Button ButtonSix;
    public Button ButtonSeven;
    public Button ButtonEight;
    public Button ButtonNine;
    public Button ButtonTen;
    public Button ButtonEleven;
    public Button Buttontwelve;

    public Button ReturnButton;

    //ステージ選択ボタンから呼ばれるメソッド
    public void StageOne()
    {
        //SE再生
        string seName = SoundData.SeType.botton1.ToString();
        GSound.Instance.PlaySe(seName);

        startPopup.OpenPopup(1, "04_Stage1");
    }

    public void StageTwo()
    {
        //SE再生
        string seName = SoundData.SeType.botton1.ToString();
        GSound.Instance.PlaySe(seName);

        startPopup.OpenPopup(2, "05_Stage2");
    }

    public void StageThree()
    {
        //SE再生
        string seName = SoundData.SeType.botton1.ToString();
        GSound.Instance.PlaySe(seName);

        startPopup.OpenPopup(3, "06_Stage3");
    }

    public void StageFor()
    {
        //SE再生
        string seName = SoundData.SeType.botton1.ToString();
        GSound.Instance.PlaySe(seName);

        startPopup.OpenPopup(4, "07_Stage4");
    }

    public void StageFive()
    {
        //SE再生
        string seName = SoundData.SeType.botton1.ToString();
        GSound.Instance.PlaySe(seName);

        startPopup.OpenPopup(5, "08_Stage5");
    }

    public void StageSix()
    {
        //SE再生
        string seName = SoundData.SeType.botton1.ToString();
        GSound.Instance.PlaySe(seName);

        startPopup.OpenPopup(6, "09_Stage6");
    }

    public void StageSeven()
    {
        //SE再生
        string seName = SoundData.SeType.botton1.ToString();
        GSound.Instance.PlaySe(seName);

        startPopup.OpenPopup(7, "10_Stage7");
    }

    public void StageEight()
    {
        //SE再生
        string seName = SoundData.SeType.botton1.ToString();
        GSound.Instance.PlaySe(seName);

        startPopup.OpenPopup(8, "11_Stage8");
    }

    public void StageNine()
    {
        //SE再生
        string seName = SoundData.SeType.botton1.ToString();
        GSound.Instance.PlaySe(seName);

        startPopup.OpenPopup(9, "12_Stage9");
    }

    public void StageTen()
    {
        //SE再生
        string seName = SoundData.SeType.botton1.ToString();
        GSound.Instance.PlaySe(seName);

        startPopup.OpenPopup(10, "13_Stage10");
    }

    public void StageEleven()
    {
        //SE再生
        string seName = SoundData.SeType.botton1.ToString();
        GSound.Instance.PlaySe(seName);

        startPopup.OpenPopup(11, "14_Stage11");
    }

    public void StageTwelve()
    {
        //SE再生
        string seName = SoundData.SeType.botton1.ToString();
        GSound.Instance.PlaySe(seName);

        startPopup.OpenPopup(12, "15_Stage12");
    }

    public void ReturnTitle()
    {
        //SE再生
        string seName = SoundData.SeType.botton1.ToString();
        GSound.Instance.PlaySe(seName);

        //// SEが鳴るまで少し待つ
        //await System.Threading.Tasks.Task.Delay(300);

        DOVirtual.DelayedCall(0.2f, () => {

            SceneManager.LoadScene("00_Title");

        });
    }
}