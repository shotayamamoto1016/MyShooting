# 🚀 Sky Invaders

![Unity](https://img.shields.io/badge/Unity-2022.x-black?logo=unity)
![Language](https://img.shields.io/badge/Language-C%23-blue)
![Platform](https://img.shields.io/badge/Platform-WebGL-green)

## 🎮 ゲーム概要

**Sky Invaders** は、敵を倒して獲得したコインで自機を強化しながら進んでいく **育成型シューティングゲーム** です。  
全12ステージを収録しており、初心者から上級者まで幅広いプレイヤーが楽しめるよう設計しました。

## 🕹️ プレイURL

👉 [Sky Invaders - unityroom](https://unityroom.com/games/ysrpw_3027035)

## 🛠️ 使用技術

| 技術 | 用途 |
|---|---|
| Unity | ゲームエンジン |
| C# | スクリプト全般 |
| DOTween | UIアニメーション・演出 |
| UniTask | 非同期処理（敵のスポーン・Wave管理） |
| Waveシステム | ステージごとの敵出現管理 |
| キャンセルトークン | 非同期処理の安全なキャンセル |
| シングルトン | サウンド・データ管理 |
| TextMeshPro | UI文字・スプライト埋め込み |

## 📅 開発期間

**11ヶ月**（個人開発）

## ✨ こだわったポイント

### 🎨 UIの設計
- 誰でも直感的に操作できるようUI配置にこだわりました
- ステージ選択画面ではポップアップで出撃前にスコア報酬を確認できる設計にしました
- ショップ・メニューなどすべてのUIにアニメーションとSEを実装しました

### ⚙️ ゲームシステム
- 敵・プレイヤーの当たり判定を丁寧に実装しました
- Waveシステムとキャンセルトークンを組み合わせた安全な非同期処理を実現しました
- シングルトンパターンでサウンド管理・データ管理を一元化しました

### 🌟 演出面
- ステージクリア時の星の獲得アニメーション
- 新ステージ解放時の鍵が開くアニメーション
- コインが飛んでいく獲得演出
- タイトル画面の自機が奥へ飛んでいくDOTweenアニメーション

## 📁 ディレクトリ構成

```
Assets/
├── _MyFolder/
│   ├── Scripts/     # ゲームスクリプト
│   ├── Scenes/      # 全12ステージ + タイトル・選択・エンディング
│   ├── Prefabs/     # 敵・プレイヤー・Wave等のプレハブ
│   ├── Sounds/      # BGM・SE
│   └── ScriptableObjects/ # ステージデータ
└── Store/           # アセットストア素材
```

## 📝 詳細記事

開発で工夫した点・難しかった点などの詳細はQiitaにまとめています。  
👉 Qiitaの記事URL（https://qiita.com/shota20041016/private/6bfbb003faf2a669301a）

## 👤 開発者

- **名前**：山本　翔太
- **GitHub**：[shotayamamoto1016](https://github.com/shotayamamoto1016)

