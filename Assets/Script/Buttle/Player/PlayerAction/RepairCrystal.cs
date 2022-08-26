using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RepairCrystal : MonoBehaviour, IPlayerAction
{
    private PlayerInput _playerInput;
    private Transform _playerTr;
    public bool ActiveFlag => _activeFlag;
    private bool _activeFlag;
    public bool ActionFlag => _actionFlag;
    private bool _actionFlag;

    private int _repairCount;
    private int _repairMaxCount;
    private int _repairPoint;

    public void AwakeManager(){
        _playerInput = this.gameObject.GetComponent<PlayerInput>();
        _playerTr = this.gameObject.GetComponent<Transform>();

        SetParam();
    }

    //初期パラメータのセット
    private void SetParam(){
        _activeFlag = false;
        _actionFlag = false;

        _repairCount = 0;
        _repairMaxCount = 30;
        _repairPoint = 1;
    }

    //入力中、徐々に正面のクリスタルを修復
    public void UpdateManager()
    {
        if(!_actionFlag) return;

        if(RepairCount()) RepairCrystalAction();
    }

    //回復アクションの有効化
    public void InputEnable(){
        _playerInput.actions["Repair"].started += OnInputStart;
        //_playerInput.actions["Repair"].performed += OnInputCompleted;
        _playerInput.actions["Repair"].canceled += OnInputCanceled;

        _activeFlag = true;
    }
    //回復アクションの無効化
    public void InputDisable(){
        _playerInput.actions["Repair"].started -= OnInputStart;
        //_playerInput.actions["Repair"].performed -= OnInputCompleted;
        _playerInput.actions["Repair"].canceled -= OnInputCanceled;

        _activeFlag = false;
        _actionFlag = false;
    }

    //アクションが実行可能な状態か
    public bool CanAction(){
        //リフト中のクリスタルがなく、正面に黒以外のクリスタルがあるか
        return PlayerCore.GetLiftCrystalTr() == null && ExistCrystal() && !ExistBlackCrystal();
    }

    //正面にクリスタルがあるか
    private bool ExistCrystal(){
        Vector2Int _fowardDir = new Vector2Int((int)_playerTr.forward.x, (int)_playerTr.forward.z);
        List<CrystalCore> _crystalCoreList = TargetCore.GetFowardCore<CrystalCore>(AStarMap._playerPos, _fowardDir, 1);
        return _crystalCoreList.Count != 0 && _crystalCoreList[0] != null;
    }

    //正面に黒クリスタルがあるか
    private bool ExistBlackCrystal(){
        Vector2Int _fowardDir = new Vector2Int((int)_playerTr.forward.x, (int)_playerTr.forward.z);
        List<CrystalCore> _crystalCoreList = TargetCore.GetFowardCore<CrystalCore>(AStarMap._playerPos, _fowardDir, 1);
        return _crystalCoreList.Count != 0 && _crystalCoreList[0]._crystalStatus._moveCost == 100;
    }

    //回復用カウント,Maxカウントまで長押しすれば回復
    private bool RepairCount(){
        bool _repairCountFlag = false;
        _repairCount++;

        if(_repairCount >= _repairMaxCount){
            _repairCount = 0;
            _repairCountFlag = true;
        }

        return _repairCountFlag;
    }

    //正面の水晶を回復
    private void RepairCrystalAction(){
        //判定座標
        Vector2Int _judgePos = new Vector2Int(AStarMap._playerPos.x + (int)_playerTr.forward.x, AStarMap._playerPos.y + (int)_playerTr.forward.z);
        //正面に黒以外の水晶があれば回復
        if(AStarMap.astarMas[_judgePos.x, _judgePos.y]._crystalCore != null){
            AStarMap.astarMas[_judgePos.x, _judgePos.y]._crystalCore.Hp += _repairPoint;
        }
    }

    //回復モーションスタート、回復フラグTrue、その間移動不可
    private void OnInputStart(InputAction.CallbackContext context){
        _actionFlag = true;
    }
    //回復モーション終了、回復フラグFalse
    private void OnInputCanceled(InputAction.CallbackContext context){
        _actionFlag = false;
    }
}
