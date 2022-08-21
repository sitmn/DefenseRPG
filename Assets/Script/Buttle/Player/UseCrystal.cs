using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UseCrystal : MonoBehaviour, IUserCrystal
{
    private PlayerStatus _playerStatus;
    private PlayerInput _playerInput;
    private Transform _playerTr;
    //水晶起動アクションの有効無効状態
    public bool LaunchActiveFlag => _launchActiveFlag;
    private bool _launchActiveFlag;
    //水晶起動中フラグ
    public bool LaunchActionFlag => _launchActionFlag;
    private bool _launchActionFlag;

    
    //起動水晶色変え用マテリアル
    private CrystalStatus[] _setCrystalStatus = new CrystalStatus[3];
    private CrystalParamAsset _crystalParamData;


    void Awake(){
        _crystalParamData = Resources.Load("Data/CrystalParamData") as CrystalParamAsset;

        _playerStatus = this.gameObject.GetComponent<PlayerStatus>();
        _playerInput = this.gameObject.GetComponent<PlayerInput>();
        _playerTr = this.gameObject.GetComponent<Transform>();
        _launchActiveFlag = false;
        _launchActionFlag = false;
    }


    public void LaunchEnable(){
        //InputSystemのコールバックをセット
        _playerInput.actions["Launch"].started += OnLaunchStart;
        _playerInput.actions["Launch"].performed += OnLaunchComplete;
        _playerInput.actions["Launch"].canceled += OnLaunchEnd;
        _launchActiveFlag = true;
    }

    public void LaunchDisable(){
        //InputSystemのコールバックをセット
        _playerInput.actions["Launch"].started -= OnLaunchStart;
        _playerInput.actions["Launch"].performed -= OnLaunchComplete;
        _playerInput.actions["Launch"].canceled -= OnLaunchEnd;
        _launchActiveFlag = false;
    }

    //前方にクリスタルがあるかを確認
    public bool CrystalCheck(){
        bool _checkCrystal = false;
        //判定座標
        Vector2Int _judgePos = new Vector2Int(AStarMap._playerPos.x + (int)_playerTr.forward.x,AStarMap._playerPos.y + (int)_playerTr.forward.z);
        //判定座標がステージリスト範囲外ではないか、正面にクリスタルがあるか
        if(!AStarMap.OutOfReferenceCheck(_judgePos) && AStarMap.astarMas[_judgePos.x, _judgePos.y]._crystalCore != null){
            _checkCrystal = true;
        }
        
        return _checkCrystal;
    }

    //前方に黒クリスタルがあるかを確認
    public bool BlackCrystalCheck(){
        bool _checkBlackCrystal = false;
        Vector2Int _judgePos = new Vector2Int(AStarMap._playerPos.x + (int)_playerTr.forward.x,AStarMap._playerPos.y + (int)_playerTr.forward.z);
        if(!AStarMap.OutOfReferenceCheck(_judgePos) && AStarMap.astarMas[_judgePos.x, _judgePos.y]._crystalCore != null){
            //黒水晶のみ移動コストが100
            _checkBlackCrystal = (AStarMap.astarMas[_judgePos.x, _judgePos.y]._crystalCore as CrystalCore)._crystalStatus._moveCost == 100;
        }

        return _checkBlackCrystal;
    }

    //InputSystem 正面に黒クリスタルがある時のみ実行
    //クリスタル起動開始
    private void OnLaunchStart(InputAction.CallbackContext context){
        //起動中フラグ（移動不可）
        _launchActionFlag = true;
        //起動モーション開始

        //起動時間UI表示
        
    }

    //クリスタル起動完了(長押し)
    private void OnLaunchComplete(InputAction.CallbackContext context){
        float _colorNo = context.ReadValue<float>();

        ACrystalCore _crystalCore = AStarMap.astarMas[AStarMap._playerPos.x + (int)_playerTr.forward.x, AStarMap._playerPos.y + (int)_playerTr.forward.z]._crystalCore as ACrystalCore;
        //コールバック値に対応するプレイヤー装備クリスタルを正面のクリスタルへ格納
        //☆正面のクリスタルに、色毎にステータスをセットし、オブジェクトの色をMaterialで変える　⇨ ScriptableObjectを使用しているが、間にPlayerStatusを挟んで、装備状況に応じて内容を変更させる予定
        _crystalCore.SetCrystalStatus(_crystalParamData.CrystalParamList[(int)_colorNo]);
        //起動時間UI非表示

        //起動モーション終了、起動中フラグ取り消し
        _launchActionFlag = false;
    }

    //クリスタル起動キャンセル
    private void OnLaunchEnd(InputAction.CallbackContext context){
        //起動モーション終了、起動中フラグ取り消し
        _launchActionFlag = false;

        //起動時間UI非表示
    }

}
