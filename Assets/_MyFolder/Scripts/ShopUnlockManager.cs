using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUnlockManager : MonoBehaviour
{
    // 解放に必要なクリアステージ数
    [SerializeField] private int requiredStage = 3;

    private Color lockedColor = new Color(0.3f, 0.3f, 0.3f, 1.0f); 
    private Color normalColor = Color.white;


    private Button shopButton;
    

    [Header("制御する要素")]
    [SerializeField] private Image[] targetImages;      // ボタンに乗っているImage2枚を入れる
    [SerializeField] private TextMeshProUGUI hintText; // 「ステージ3で解放」のテキスト 

    void Start()
    {
        shopButton = GetComponent<Button>();

        UpdateShopStatus();
    }

    // ショップのボタン状態を更新
    public void UpdateShopStatus()
    {
        if (DataManager.instance == null) return;

        // clearedStageIndex が 3 以上なら解放
        bool isUnlocked = DataManager.instance.clearedStageIndex >= requiredStage;

        // ボタンの有効化、無効化
        if (shopButton != null)
        {
            shopButton.interactable = isUnlocked;
        }

        //ボタンに乗っているImage2枚の色をまとめて変更
        foreach (Image img in targetImages)
        {
            if (img != null)
            {
                img.color = isUnlocked ? normalColor : lockedColor;
            }
        }

        //ボタンのメイン画像の色も変更
        GetComponent<Image>().color = isUnlocked ? normalColor : lockedColor;

    }

}