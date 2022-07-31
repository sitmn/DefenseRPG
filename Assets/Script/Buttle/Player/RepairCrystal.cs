using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RepairCrystal : MonoBehaviour, IRepairCrystal
{
    private PlayerInput _playerInput;
    private Transform _playerTr;
    public bool RepairActiveFlag => _repairActiveFlag;
    private bool _repairActiveFlag;
    public bool RepairActionFlag => _repairActionFlag;
    private bool _repairActionFlag;

    private int _repairCount;
    private int _repairMaxCount;

    private int _repairPoint;

    void Awake(){
        _playerInput = this.gameObject.GetComponent<PlayerInput>();
        _playerTr = this.gameObject.GetComponent<Transform>();

        SetParam();
    }

    //初期パラメータのセット
    private void SetParam(){
        _repairActiveFlag = false;
        _repairActionFlag = false;

        _repairCount = 0;
        _repairMaxCount = 30;
        _repairPoint = 1;
    }

    //回復アクションの有効化
    public void RepairEnable(){
        _playerInput.actions["Repair"].started += OnRepairStart;
        //_playerInput.actions["Repair"].performed += OnRepairCompleted;
        _playerInput.actions["Repair"].canceled += OnRepairCanceled;

        _repairActiveFlag = true;
    }
    //回復アクションの無効化
    public void RepairDisable(){
        _playerInput.actions["Repair"].started -= OnRepairStart;
        //_playerInput.actions["Repair"].performed -= OnRepairCompleted;
        _playerInput.actions["Repair"].canceled -= OnRepairCanceled;

        _repairActiveFlag = false;
        _repairActionFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!_repairActionFlag) return;

        if(RepairCount()) RepairCrystalAction();
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
        if(AStarMap.astarMas[AStarMap._playerPos.x + (int)_playerTr.forward.x, AStarMap._playerPos.y + (int)_playerTr.forward.z].obj.Count == 1){
            if(AStarMap.astarMas[AStarMap._playerPos.x + (int)_playerTr.forward.x, AStarMap._playerPos.y + (int)_playerTr.forward.z].obj[0].GetType().Name == "CrystalController"){
                AStarMap.astarMas[AStarMap._playerPos.x + (int)_playerTr.forward.x, AStarMap._playerPos.y + (int)_playerTr.forward.z].obj[0].Hp += _repairPoint;
                Debug.Log(AStarMap.astarMas[AStarMap._playerPos.x + (int)_playerTr.forward.x, AStarMap._playerPos.y + (int)_playerTr.forward.z].obj[0].Hp + "HHH");
            }
        }
    }

    //回復モーションスタート、回復フラグTrue、その間移動不可
    private void OnRepairStart(InputAction.CallbackContext context){
        _repairActionFlag = true;
    }
    // //長押し時、正面の水晶を徐々に回復
    // private void OnRepairCompleted(InputAction.CallbackContext context){
    //     Debug.Log("BBB");
    // }
    //回復モーション終了、回復フラグFalse
    private void OnRepairCanceled(InputAction.CallbackContext context){
        _repairActionFlag = false;
    }
}
