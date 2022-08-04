using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LiftCrystal : MonoBehaviour, ILiftCrystal
{
    private PlayerStatus _playerStatus;
    private PlayerInput _playerInput;
    private Transform _playerTr;
    //水晶リフトアップアクションの有効無効状態
    public bool LiftUpActiveFlag => _liftUpActiveFlag;
    private bool _liftUpActiveFlag;
    //水晶リフトアップ中フラグ
    public bool LiftUpActionFlag => _liftUpActionFlag;
    private bool _liftUpActionFlag;

    //水晶リフトダウンアクションの有効無効状態
    public bool LiftDownActiveFlag => _liftDownActiveFlag;
    private bool _liftDownActiveFlag;
    //水晶リフトダウンフラグ
    public bool LiftDownActionFlag => _liftDownActionFlag;
    private bool _liftDownActionFlag;

    //リフト中の水晶オブジェクト
    public static CrystalController _crystalController;
    public Transform CrystalTr => _crystalTr;
    private Transform _crystalTr;
    
    /*//起動水晶色変え用マテリアル
    private ACrystalStatus[] _setCrystalStatus = new ACrystalStatus[3];
    private CrystalParamAsset _crystalParamData;*/


    void Awake(){
        //_crystalParamData = Resources.Load("Data/CrystalParamData") as CrystalParamAsset;

        _playerStatus = this.gameObject.GetComponent<PlayerStatus>();
        _playerInput = this.gameObject.GetComponent<PlayerInput>();
        _playerTr = this.gameObject.GetComponent<Transform>();
        _liftUpActiveFlag = false;
        _liftUpActionFlag = false;
        _liftDownActiveFlag = false;
        _liftDownActionFlag = false;
    }

    public void UpdateManager(){
        if(_crystalTr == null) return;
        _crystalTr.position = _playerTr.position + new Vector3(0, 2, 0);
    }


    public void LiftUpEnable(){
        //InputSystemのコールバックをセット
        _playerInput.actions["LiftUp"].started += OnLiftUpStart;
        _playerInput.actions["LiftUp"].performed += OnLiftUpComplete;
        _playerInput.actions["LiftUp"].canceled += OnLiftUpEnd;
        _liftUpActiveFlag = true;
    }

    public void LiftUpDisable(){
        //InputSystemのコールバックをセット
        _playerInput.actions["LiftUp"].started -= OnLiftUpStart;
        _playerInput.actions["LiftUp"].performed -= OnLiftUpComplete;
        _playerInput.actions["LiftUp"].canceled -= OnLiftUpEnd;
        _liftUpActiveFlag = false;
    }

    public void LiftDownEnable(){
        //InputSystemのコールバックをセット
        _playerInput.actions["LiftDown"].started += OnLiftDownStart;
        _playerInput.actions["LiftDown"].performed += OnLiftDownComplete;
        _playerInput.actions["LiftDown"].canceled += OnLiftDownEnd;
        _liftDownActiveFlag = true;
    }

    public void LiftDownDisable(){
        //InputSystemのコールバックをセット
        _playerInput.actions["LiftDown"].started -= OnLiftDownStart;
        _playerInput.actions["LiftDown"].performed -= OnLiftDownComplete;
        _playerInput.actions["LiftDown"].canceled -= OnLiftDownEnd;
        _liftDownActiveFlag = false;
    }

    //前方に敵またはクリスタルがあるか、かつ、前方隣1マスに敵がいるかを確認
    public bool StageObjCheck(){
        bool _checkCrystal = false;
        Vector2Int _judgePos = new Vector2Int(AStarMap._playerPos.x + (int)_playerTr.forward.x, AStarMap._playerPos.y + (int)_playerTr.forward.z);
        if(!AStarMap.OutOfReferenceCheck(_judgePos) && (AStarMap.astarMas[StageMove.UndoElementStageMove(_judgePos.x), _judgePos.y].obj.Count > 0 ||
        AStarMap.AroundSearch(new Vector2Int(AStarMap._playerPos.x + (int)_playerTr.forward.x, AStarMap._playerPos.y + (int)_playerTr.forward.z), 1).Count > 0)){
            _checkCrystal = true;
        }
        
        return _checkCrystal;
    }

    //前方に黒クリスタルがあるかを確認
    public bool BlackCrystalCheck(){
        bool _checkBlackCrystal = false;
        Vector2Int _judgePos = new Vector2Int(AStarMap._playerPos.x + (int)_playerTr.forward.x, AStarMap._playerPos.y + (int)_playerTr.forward.z);
        if(!AStarMap.OutOfReferenceCheck(_judgePos) && (AStarMap.astarMas[StageMove.UndoElementStageMove(_judgePos.x), _judgePos.y].obj.Count == 1)){
            _checkBlackCrystal = (AStarMap.astarMas[StageMove.UndoElementStageMove(_judgePos.x), _judgePos.y].obj[0] as CrystalController)._crystalStatus.GetType().Name == "BlackCrystalStatus";
        }

        return _checkBlackCrystal;
    }

    //InputSystem 正面に黒以外のクリスタルがある時のみ実行
    //クリスタルリフトアップ開始
    private void OnLiftUpStart(InputAction.CallbackContext context){
        //リフトアップ中フラグ（移動不可）
        _liftUpActionFlag = true;
        //リフトアップモーション開始

        //起動時間UI表示
        
    }

    //クリスタルリフトアップ完了(長押し)
    private void OnLiftUpComplete(InputAction.CallbackContext context){
        //Lift中Objを格納
        Vector2Int _judgePos = new Vector2Int(AStarMap._playerPos.x + (int)_playerTr.forward.x, AStarMap._playerPos.y + (int)_playerTr.forward.z);
        _crystalTr = AStarMap.astarMas[StageMove.UndoElementStageMove(_judgePos.x), _judgePos.y].obj[0].gameObject.GetComponent<Transform>();
        _crystalController = AStarMap.astarMas[StageMove.UndoElementStageMove(_judgePos.x), _judgePos.y].obj[0] as CrystalController;
        //マップ情報から水晶を削除
        _crystalController.SetOffAStarMap();
        //オブジェクトを頭上へ移動
        _crystalTr.position = _playerTr.position + new Vector3(0, 2, 0);
        //起動時間UI非表示

        //リフトアップモーション終了、リフトアップ中フラグ取り消し
        _liftUpActionFlag = false;
    }

    //クリスタルリフトアップキャンセル
    private void OnLiftUpEnd(InputAction.CallbackContext context){
        //リフトアップモーション終了、リフトアップ中フラグ取り消し
        _liftUpActionFlag = false;

        //起動時間UI非表示
    }



    //InputSystem 正面に黒以外のクリスタルがある時のみ実行
    //クリスタルリフトダウン開始
    private void OnLiftDownStart(InputAction.CallbackContext context){
        //リフトダウン中フラグ（移動不可）
        _liftDownActionFlag = true;
        //リフトダウンモーション開始

        //起動時間UI表示
        
    }

    //クリスタルリフトダウン完了(長押し)
    private void OnLiftDownComplete(InputAction.CallbackContext context){
        //オブジェクトをマスへ配置
        _crystalTr.position = new Vector3(AStarMap._playerPos.x + (int)_playerTr.forward.x, 0.5f, AStarMap._playerPos.y + (int)_playerTr.forward.z);
        //プレイヤーの次の移動先が重複している場合、移動をキャンセル
        //マップ情報に水晶を追加
        _crystalController.SetOnAStarMap();
        //リフト中情報をnullに
        _crystalTr = null;
        _crystalController = null;

        //格納したマスが移動先になっているエネミーがいれば再度経路探索

        //起動時間UI非表示

        //リフトダウンモーション終了、リフトダウン中フラグ取り消し
        _liftDownActionFlag = false;
    }

    //クリスタルリフトダウンキャンセル
    private void OnLiftDownEnd(InputAction.CallbackContext context){
        //リフトダウンモーション終了、リフトダウン中フラグ取り消し
        _liftDownActionFlag = false;

        //起動時間UI非表示
    }
}
