using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//定数を纏めています
public static class ConstManager
{
    /* プレイヤーアクション系のInputSystemのActionMap文字列 */
    //InputSystemのActionMap - リフトアップアクション
    public const string _liftUpInput = "LiftUp";
    //InputSystemのActionMap - リフトダウンアクション
    public const string _liftDownInput = "LiftDown";
    //InputSystemのActionMap - リペアアクション
    public const string _repairInput = "Repair";
    //InputSystemのActionMap - 起動アクション
    public const string _launchInput = "LaunchCrystal";
    //InputSystemのActionMap - 選択アクション
    public const string _selectInput = "SelectCrystal";
    //InputSystemのActionMap - ランクアップアクション
    public const string _rankUpInput = "RankUp";
    //InputSystemのActionMap - 移動
    public const string _moveInput = "Move";

    //プレイヤーが使用できるクリスタル種類の数
    public const int _possettionCrystalAmount = 5;
    
    //コストの画面表示用ラベル
    public const string _costLabel = "Cost:";
    public const string _countLabel = "StageMoveCount:";

    //モーション用名称
    public const string _moveAction = "_move";

    //クリスタルコアのクラス名
    public const string _shippingCrystalName = "ShippingCrystalCore";

    //MapCostの計算用ステータス　- open:計算完了　closed:計算終了　none:未計算
    public const string _costMapOpenStr = "open";
    public const string _costMapClosedStr = "closed";
    public const string _costMapNoneStr = "none";

    //PlayerActionのゲージ時間用
    public const float _liftUpCount = 0.4f;
    public const float _liftDownCount = 0.1f;
    public const float _rankUpCount = 1.4f;
    public const float _repairCount = 0.4f;
    public const float _launchCount = 1.0f;

    //HPゲージのオフセット（対象の少し下にセット）
    public const float _hpBarPosOffsetY = 0.4f;
    public const float _hpBarPosOffsetZ = 0.4f;

    //リフトアップ時のY座標
    public const float _liftUpPosY = 2f;
    
    //Action無効時のButton透明度
    public const float _disableButtonAlpha = 0.25f;
    //Button押下時の縮小率
    public const float _buttonShrinkRate = 0.8f;
    //Button押下時または押下終了時の拡大縮小にかかる時間
    public const float _buttonscalingTime = 0.25f;

    //エリア区画用エフェクトのstartLifetimeの最大値
    public const float _fieldWallMaxStartLifeTime = 2f;

    //エリア区画用エフェクトのstartLifetimeの最小値
    public const float _fieldWallMinStartLifeTime = 0.5f;

    //エリア区画用エフェクトのrateOverTimeの最大値
    public const float _fieldWallMaxRateOverTime = 5f;

    //エリア区画用エフェクトのrateOverTimeの最小値
    public const float _fieldWallMinRateOverTime = 1f;
}
