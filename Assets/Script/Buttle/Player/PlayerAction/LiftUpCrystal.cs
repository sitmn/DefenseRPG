using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LiftUpCrystal : MonoBehaviour, IPlayerAction
{
    //リフトアップボタン
    [SerializeField]
    private GameObject _liftUpButton;
    //リフトダウンボタン
    [SerializeField]
    private GameObject _liftDownButton;
    private PlayerInput _playerInput;
    private Transform _playerTr;
    //クリスタルリフトアップアクションの有効無効状態
    public bool IsActive => _isActive;
    private bool _isActive;
    //クリスタルリフトアップ中フラグ
    public bool IsAction => _isAction;
    private bool _isAction;
    private UIManager _UIManager;
    
    //クラスの初期化
    public void AwakeManager(PlayerParam _playerParam, UIManager _UIManager){
        _playerInput = this.gameObject.GetComponent<PlayerInput>();
        _playerTr = this.gameObject.GetComponent<Transform>();
        _isActive = false;
        _isAction = false;
        this._UIManager = _UIManager;
    }

    //リフトアップ中のクリスタルをプレイヤー移動に合わせて移動
    public void UpdateManager(){
        if(PlayerCore.GetLiftCrystalTr() == null) return;
        PlayerCore.GetLiftCrystalTr().position = _playerTr.position + new Vector3(0, 2, 0);
    }

    //リフトアップアクション入力の有効化
    public void InputEnable(){
        //InputSystemのコールバックをセット
        _playerInput.actions[ConstManager._liftUpInput].started += OnInputStart;
        _playerInput.actions[ConstManager._liftUpInput].performed += OnInputComplete;
        _playerInput.actions[ConstManager._liftUpInput].canceled += OnInputEnd;
        _isActive = true;
        //リフトアップボタンを非透明に
        _UIManager._liftUpButton.SetOpacityButton();
    }

    //リフトアップアクション入力の無効化
    public void InputDisable(){
        //InputSystemのコールバックをセット
        _playerInput.actions[ConstManager._liftUpInput].started -= OnInputStart;
        _playerInput.actions[ConstManager._liftUpInput].performed -= OnInputComplete;
        _playerInput.actions[ConstManager._liftUpInput].canceled -= OnInputEnd;
        _isActive = false;
        //リフトアップボタンを透明に
        _UIManager._liftUpButton.SetTransparentButton();
    }

    //アクションが実行可能な状態か
    public bool CanAction(){
        //リフト中のクリスタルがなく、正面に黒以外のクリスタルがあるか
        return PlayerCore.GetLiftCrystalTr() == null && ExistCrystal();
    }

    //正面にクリスタルがあるか
    private bool ExistCrystal(){
        Vector2Int _fowardDir = new Vector2Int((int)_playerTr.forward.x, (int)_playerTr.forward.z);
        List<CrystalCoreBase> _crystalCoreList = TargetCore.GetFowardCore<CrystalCoreBase>(MapManager.GetPlayerPos(), _fowardDir, 1); 
        return _crystalCoreList.Count != 0 && _crystalCoreList[0] != null;
    }

    //アクションコストが足りているか
    public bool EnoughActionCost(){
        return true;
    }
    public void ShortageActionCost(){
        return;
    }

    //リフトアップ、ダウンボタンの切り替え
    private void ChangeLiftButton(){
        _liftUpButton.SetActive(false);
        _liftDownButton.SetActive(true);
    }

    //InputSystem 正面に黒以外のクリスタルがある時のみ実行
    //クリスタルリフトアップ開始
    private void OnInputStart(InputAction.CallbackContext context){
        //リフトアップ中フラグ（移動不可）
        _isAction = true;
        //リフトアップモーション開始

        //起動時間UI表示
        _UIManager._actionGauge.SetTween(ConstManager._liftUpCount);
    }

    //クリスタルリフトアップ完了(長押し)
    private void OnInputComplete(InputAction.CallbackContext context){
        Vector2Int _judgePos = new Vector2Int(MapManager.GetPlayerPos().x + (int)_playerTr.forward.x, MapManager.GetPlayerPos().y + (int)_playerTr.forward.z);
        CrystalCoreBase _crystalCore = MapManager.GetMap(_judgePos)._crystalCore as CrystalCoreBase;
        Transform _crystalTr = MapManager.GetMap(_judgePos)._crystalCore.gameObject.GetComponent<Transform>();
        //Lift中Objを格納
        PlayerCore.SetLiftCrystal(_crystalCore, _crystalTr);
        
        //マップ情報から水晶を削除
        _crystalCore.SetOffMap();
        //オブジェクトを頭上へ移動
        _crystalTr.position = _playerTr.position + new Vector3(0, 2, 0);
        //起動時間UI非表示

        //リフトアップモーション終了、リフトアップ中フラグ取り消し
        _isAction = false;
        //リフトアップボタンとリフトダウンボタンの有効無効切り替え
        ChangeLiftButton();
    }

    //クリスタルリフトアップキャンセル
    private void OnInputEnd(InputAction.CallbackContext context){
        //リフトアップモーション終了、リフトアップ中フラグ取り消し
        _isAction = false;

        //起動時間UI非表示
        _UIManager._actionGauge.CancelTween();
    }
}
