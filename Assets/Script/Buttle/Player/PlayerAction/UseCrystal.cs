using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UseCrystal : MonoBehaviour, IPlayerAction
{
    private PlayerInput _playerInput;
    private Transform _playerTr;
    //水晶起動アクションの有効無効状態
    public bool ActiveFlag => _activeFlag;
    private bool _activeFlag;
    //水晶起動中フラグ
    public bool ActionFlag => _actionFlag;
    private bool _actionFlag;
    //起動水晶色変え用マテリアル
    private CrystalStatus[] _setCrystalStatus = new CrystalStatus[3];
    private CrystalParamAsset _crystalParamData;

    //クラスの初期化
    public void AwakeManager(){
        _crystalParamData = Resources.Load("Data/CrystalParamData") as CrystalParamAsset;

        _playerInput = this.gameObject.GetComponent<PlayerInput>();
        _playerTr = this.gameObject.GetComponent<Transform>();
        _activeFlag = false;
        _actionFlag = false;
    }

    //本クラスはUpdate処理なし
    public void UpdateManager(){

    }

    //アクションの入力を有効に切り替え
    public void InputEnable(){
        //InputSystemのコールバックをセット
        _playerInput.actions["Launch"].started += OnInputStart;
        _playerInput.actions["Launch"].performed += OnInputComplete;
        _playerInput.actions["Launch"].canceled += OnInputEnd;
        _activeFlag = true;
    }

    //アクションの入力を無効に切り替え
    public void InputDisable(){
        //InputSystemのコールバックをセット
        _playerInput.actions["Launch"].started -= OnInputStart;
        _playerInput.actions["Launch"].performed -= OnInputComplete;
        _playerInput.actions["Launch"].canceled -= OnInputEnd;
        _activeFlag = false;
    }

    //アクションが実行可能な状態か
    public bool CanAction(){
        //正面に黒クリスタルがあるか
        return BlackCrystalCheck();
    }

    //正面に黒クリスタルがあるか
    private bool BlackCrystalCheck(){
        Vector2Int _fowardDir = new Vector2Int((int)_playerTr.forward.x, (int)_playerTr.forward.z);
        List<CrystalCore> _crystalCoreList = TargetCore.GetFowardCore<CrystalCore>(AStarMap.GetPlayerPos(), _fowardDir, 1);
        return _crystalCoreList.Count != 0 && _crystalCoreList[0]._crystalStatus._moveCost == 100;
    }

    //クリスタル起動開始
    private void OnInputStart(InputAction.CallbackContext context){
        //起動中フラグ（移動不可）
        _actionFlag = true;
        //起動モーション開始

        //起動時間UI表示
        
    }

    //クリスタル起動完了(長押し)
    private void OnInputComplete(InputAction.CallbackContext context){
        float _colorNo = context.ReadValue<float>();

        CrystalCore _crystalCore = AStarMap.astarMas[AStarMap.GetPlayerPos().x + (int)_playerTr.forward.x, AStarMap.GetPlayerPos().y + (int)_playerTr.forward.z]._crystalCore as CrystalCore;
        //コールバック値に対応するプレイヤー装備クリスタルを正面のクリスタルへ格納
        //☆正面のクリスタルに、色毎にステータスをセットし、オブジェクトの色をMaterialで変える　⇨ ScriptableObjectを使用しているが、間にPlayerStatusを挟んで、装備状況に応じて内容を変更させる予定
        _crystalCore.SetCrystalStatus(_crystalParamData.CrystalParamList[(int)_colorNo]);
        //起動時間UI非表示

        //起動モーション終了、起動中フラグ取り消し
        _actionFlag = false;
    }

    //クリスタル起動キャンセル
    private void OnInputEnd(InputAction.CallbackContext context){
        //起動モーション終了、起動中フラグ取り消し
        _actionFlag = false;

        //起動時間UI非表示
    }

}
