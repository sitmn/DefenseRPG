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

    void Update(){
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

    //前方に敵またはクリスタルがあるかを確認
    public bool StageObjCheck(){
        bool _checkCrystal = AStarMap.astarMas[AStarMap._playerPos.x + (int)_playerTr.forward.x, AStarMap._playerPos.y + (int)_playerTr.forward.z].obj.Count > 0;
        
        return _checkCrystal;
    }

    //前方に黒クリスタルがあるかを確認
    public bool BlackCrystalCheck(){
        bool _checkBlackCrystal = false;
        if(AStarMap.astarMas[AStarMap._playerPos.x + (int)_playerTr.forward.x, AStarMap._playerPos.y + (int)_playerTr.forward.z].obj.Count == 1){
            _checkBlackCrystal = (AStarMap.astarMas[AStarMap._playerPos.x + (int)_playerTr.forward.x, AStarMap._playerPos.y + (int)_playerTr.forward.z].obj[0] as CrystalController)._crystalStatus.GetType().Name == "BlackCrystalStatus";
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
        
        Debug.Log("AAA2");
    }

    //クリスタルリフトアップ完了(長押し)
    private void OnLiftUpComplete(InputAction.CallbackContext context){
        Debug.Log("BBB2");

        
        //Lift中Objを格納
        _crystalTr = AStarMap.astarMas[AStarMap._playerPos.x + (int)_playerTr.forward.x, AStarMap._playerPos.y + (int)_playerTr.forward.z].obj[0].gameObject.GetComponent<Transform>();
        _crystalController = AStarMap.astarMas[AStarMap._playerPos.x + (int)_playerTr.forward.x, AStarMap._playerPos.y + (int)_playerTr.forward.z].obj[0] as CrystalController;
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
        Debug.Log("CCC");
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
        
        Debug.Log("AAA2");
    }

    //クリスタルリフトダウン完了(長押し)
    private void OnLiftDownComplete(InputAction.CallbackContext context){
        Debug.Log("BBB2");

        //オブジェクトをマスへ配置
        _crystalTr.position = new Vector3(AStarMap._playerPos.x + (int)_playerTr.forward.x, 0.5f, AStarMap._playerPos.y + (int)_playerTr.forward.z);
        //プレイヤーの次の移動先が重複している場合、移動をキャンセル
        //マップ情報に水晶を追加
        _crystalController.SetOnAStarMap();
        //リフト中情報をnullに
        _crystalTr = null;
        //格納したマスが移動先になっているエネミーがいれば再度経路探索

        //起動時間UI非表示

        //リフトダウンモーション終了、リフトダウン中フラグ取り消し
        _liftDownActionFlag = false;
    }

    //クリスタルリフトダウンキャンセル
    private void OnLiftDownEnd(InputAction.CallbackContext context){
        Debug.Log("CCC");
        //リフトダウンモーション終了、リフトダウン中フラグ取り消し
        _liftDownActionFlag = false;

        //起動時間UI非表示
    }
}
