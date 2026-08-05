# 🚀 Sky Invaders

![Unity](https://img.shields.io/badge/Unity-6000.0.49f1-black?logo=unity)
![Language](https://img.shields.io/badge/Language-C%23-blue)
![Platform](https://img.shields.io/badge/Platform-WebGL-green)
![Period](https://img.shields.io/badge/開発期間-11ヶ月-orange)

## 🎮 ゲーム概要

**Sky Invaders** は、敵を倒して獲得したコインで自機を強化しながら進んでいく **育成型シューティングゲーム** です。  
全12ステージを収録しており、初心者から上級者まで幅広いプレイヤーが楽しめるよう設計しました。

## 🕹️ プレイURL

👉 [Sky Invaders - unityroom](https://unityroom.com/games/ysrpw_3027035)

---

## 🛠️ 使用技術

| 技術 | 用途 |
|---|---|
| Unity 6.0 (6000.0.49f1) | ゲームエンジン |
| C# | スクリプト全般 |
| DOTween | UIアニメーション・演出全般 |
| UniTask | 非同期処理（敵のスポーン・Wave管理） |
| Waveシステム | ステージごとの敵出現管理 |
| キャンセルトークン | 非同期処理の安全なキャンセル |
| シングルトンパターン | サウンド管理・データ管理の一元化 |
| TextMeshPro | UI文字・スプライト埋め込み |
| ScriptableObject | ステージデータの管理 |
| PlayerPrefs | セーブ・ロード機能 |

---

## 📅 開発期間

**9ヶ月**（個人開発）

---

## ✨ こだわったポイント

### 🏠 タイトル画面

#### パワーアップショップ
- 誰でも直感的にわかりやすいUIボタンを設計・配置しました
- ボタンを押すと下からひょっこりと出てくるアニメーションを実装し、閉じるボタンを押すと下に戻る演出にしました
- パワーアップショップ内の各強化項目がそれぞれ何の項目かわかりやすいようにUIの配置と見た目を工夫しました
- パワーアップボタンを押した際に強化された実感が伝わるようなSEを選定・実装しました

#### ガイドテキスト
- パワーアップショップがステージ3クリアまで開放されない仕様をプレイヤーが疑問に思わないようガイドテキストを設置しました（パワーバランス調整のための意図的な設計）
- ショップ開放後も各強化項目の説明をガイドテキストで表示しプレイヤーが迷わないよう設計しました
- 全ステージクリア後にプレイヤーへの称賛メッセージを3種類ランダムで表示しプレイヤーに達成感と笑顔を与えました

#### 3D自機アニメーション
- タイトル画面の自機を3Dモデルで作成し、プレイヤーイメージをよりリアルでかっこよくしました
- DOTweenを使い、奥へ進んで弧を描きながら戻ってくる軌道アニメーションを実装しタイトル画面のクオリティを高めました

---

### 🗺️ ステージ選択画面

#### UIの配置
- ボタンや背景など見た目を重視したデザインで統一感のある画面を作成しました

#### ステージボタン押下後のポップアップ
- ステージボタンを押した際にそのままステージに遷移するのではなく、スコアに対するコイン報酬を事前に確認できるポップアップを実装しました
- ポップアップ内のコイン表示にはTextMeshProのSpriteAsset機能を活用しコイン画像をテキスト内に埋め込みました
- パネルのデザインや表示イメージにもこだわり見た目のクオリティを高めました

#### ステージクリア後のアニメーション
以下の3つの演出を実装しました。

| 演出 | 内容 |
|---|---|
| 星の獲得アニメーション | 獲得した星を1つずつ順番に表示するアニメーションを実装しました |
| コイン飛翔アニメーション | 獲得コインがステージボタンからトータルコイン表示へ吸い寄せられるように飛んでいき、ジャラジャラと増えるカウントアップ演出を実装しました |
| 鍵が開くアニメーション | ステージ解放時に鍵が揺れながら開く演出を実装しただ鍵が消えるだけでなく視覚的に楽しい演出にしました |

---

### ⚔️ 各ステージ画面

#### 背景
- 背景を多重構造にしそれぞれ異なる速度でスクロールさせることでプレイヤーが前進しているような臨場感を演出しました

#### スコア表示
- 敵を倒した際に獲得スコアがひょっこりと上に表示される演出を実装し、何点獲得したか直感的にわかるようにしました
- トータルスコアは獲得スコアを50分割してキューに入れることでジャラジャラとスコアが増えていく演出を実装しました

#### インゲームメニュー
- 戦闘中に別のステージへ戻りたい場合やパワーアップし忘れた場合に対応するためメニューボタンを各ステージに配置しました
- メニューのステージ選択へ戻るボタンには誤操作防止のための確認ポップアップを実装しました

---

### 👾 敵の実装

4種類の敵を実装しました。

| 敵の種類 | 特徴 |
|---|---|
| 突進型 | まっすぐ突進してくる敵 |
| 射撃型 | ゆっくり進みながら弾を発射する敵 |
| 連射型 | 左右に移動しながら3発連続で弾を発射する敵 |
| ボス | 左右に移動しながら弾を発射し体力減少でパワーアップする敵 |

- 各敵の弾の発射位置・発射スピード・移動スピード・体力をそれぞれ調整しゲームバランスを整えました
- Waveシステムで敵の出現を管理することで各ステージごとに工夫した編成を実現しました

---

### 💾 データ管理

- シングルトンパターンをサウンド管理（GSound）とデータ管理（DataManager）に活用しシーンをまたいだデータの一元管理を実現しました
- PlayerPrefsを使用してステージクリア状況・獲得星数・所持コインをセーブ・ロードする仕組みを実装しました

---

## 📦 使用アセット

| アセット名 | 用途 |
|---|---|
| Vertical 2D Shooting BE4 | プレイヤー・敵のスプライト |
| Dynamic Space Background | 背景 |
| Planets with Space Background in Flat Style | 背景 |
| Space_Exploration_GUI_Kit | UI素材 |
| FreeButtonSet | ボタン素材 |
| LevelMapModule | ステージ選択マップ |
| Puzzle Blocks Icon Pack | アイコン素材 |
| Violet Theme Ui | UIテーマ |
| _Heathen Engineering | ユーティリティ |

---

## 📁 ディレクトリ構成

```
Assets/
├── _MyFolder/
│   ├── Scripts/                    # ゲームスクリプト
│   │   ├── 【ゲーム管理】
│   │   ├── GameDirector.cs             # ゲーム全体管理
│   │   ├── GameAdministrator.cs        # ゲーム進行管理
│   │   ├── GameManager.cs              # ステージ選択管理
│   │   ├── DataManager.cs              # データ管理（シングルトン）
│   │   ├── MyPlayer.cs                 # プレイヤーデータ管理
│   │   ├── SoundData.cs                # サウンドデータ定義
│   │   ├── GSound.cs                   # サウンド管理（シングルトン）
│   │   ├── SoundDirector.cs            # サウンド再生管理
│   │   ├── SceneTransition.cs          # シーン遷移管理
│   │   │
│   │   ├── 【プレイヤー】
│   │   ├── PlayerController.cs         # プレイヤー操作・制御
│   │   ├── PlayerSpawn.cs              # プレイヤー生成
│   │   ├── StartPlayerController.cs    # タイトル自機制御
│   │   ├── TitleShipController.cs      # タイトル3D自機アニメーション
│   │   ├── Bullet.cs                   # 弾制御
│   │   ├── Option.cs                   # オプション制御
│   │   ├── RotateBullet.cs             # 回転弾制御
│   │   │
│   │   ├── 【敵】
│   │   ├── EnemyController.cs          # 敵基底クラス
│   │   ├── EnemyA.cs                   # 突進型敵
│   │   ├── EnemyAGenerator.cs          # 突進型敵生成
│   │   ├── EnemyB.cs                   # 射撃型敵
│   │   ├── EnemyC.cs                   # 連射型敵
│   │   ├── EnemyBoss.cs                # ボス敵
│   │   ├── EnemyBullet.cs              # 敵弾制御
│   │   ├── Explosion.cs                # 爆発エフェクト
│   │   ├── DestroyArea.cs              # 画面外削除エリア
│   │   │
│   │   ├── 【Wave管理】
│   │   ├── Wave.cs                     # Wave制御
│   │   ├── WaveStart.cs                # ステージ開始演出
│   │   ├── WaveClear.cs                # ステージクリア演出
│   │   ├── WaveEnding.cs               # エンディング演出
│   │   ├── StageData.cs                # ステージデータ定義
│   │   ├── StageSettings.cs            # ステージ設定
│   │   │
│   │   ├── 【UI】
│   │   ├── TitleDirector.cs            # タイトル画面管理
│   │   ├── TitleGuideManager.cs        # タイトルガイドテキスト管理
│   │   ├── SelectDirector.cs           # ステージ選択画面管理
│   │   ├── StageStartPopup.cs          # ステージ開始ポップアップ
│   │   ├── StageButtonStars.cs         # ステージボタン星表示
│   │   ├── InGameMenuController.cs     # インゲームメニュー制御
│   │   ├── ResultScreen.cs             # リザルト画面管理
│   │   ├── EndingDirector.cs           # エンディング画面管理
│   │   ├── UpgradeItemUI.cs            # パワーアップショップUI
│   │   ├── UpgradeUIManager.cs         # パワーアップショップ管理
│   │   ├── ShopUnlockManager.cs        # ショップ開放管理
│   │   ├── HintTextWatcher.cs          # ヒントテキスト管理
│   │   ├── CoinDisplay.cs              # コイン表示管理
│   │   ├── CoinFlyEffect.cs            # コイン飛翔演出
│   │   ├── FlyText.cs                  # スコアポップアップテキスト
│   │   ├── Background.cs               # 背景スクロール管理
│   │   ├── PlanetRotation.cs           # 惑星回転演出
│   │   └── Item.cs                     # アイテム管理
│   │
│   ├── Scenes/                     # 全シーン
│   │   ├── 00_Title.unity              # タイトル画面
│   │   ├── 01_Select.unity             # ステージ選択画面
│   │   ├── 02_Game.unity               # ゲーム画面
│   │   ├── 03_Ending.unity             # エンディング
│   │   └── 04_Stage1.unity ～ 15_Stage12.unity
│   │
│   ├── Prefabs/                    # プレハブ
│   │   ├── Players/                    # プレイヤー関連
│   │   ├── Enemys/                     # 敵関連
│   │   └── Wave/                       # Wave関連
│   │
│   ├── Sounds/                     # BGM・SE
│   │   ├── BGM/
│   │   └── SE/
│   │
│   └── ScriptableObjects/          # ステージデータ
│
└── Store/                          # アセットストア素材
```

---

## 📝 詳細記事

開発で工夫した点・難しかった点などの詳細はQiitaにまとめています。  
👉 [ゲーム開発ポートフォリオ(Sky Invaders) - Qiita](https://qiita.com/shota20041016/items/6bfbb003faf2a669301a)

## 👤 開発者

- **GitHub**：[shotayamamoto1016](https://github.com/shotayamamoto1016)
