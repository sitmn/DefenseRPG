using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UseCrystal : IPlayerAction
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
    //起動するクリスタルのパラメータ
    private CrystalParam _useCrystalParam;
    //装備しているクリスタルの番号
    private int _useCrystalNo;
    //アクションコスト
    private ActionCost _actionCost;

    public UseCrystal(CrystalParam _crystalParam, int _useCrystalNo){
        _playerInput = MapManager._playerCore.gameObject.GetComponent<PlayerInput>();
        _playerTr = MapManager._playerCore.gameObject.GetComponent<Transform>();
        _actionCost = MapManager._playerCore.gameObject.GetComponent<ActionCost>();
        this._useCrystalParam = _crystalParam;
        this._useCrystalNo = _useCrystalNo;
        _activeFlag = false;
        _actionFlag = false;
    }

    //クラスの初期化
    public void AwakeManager(PlayerParam _playerParam){
        
    }

    //本クラスはUpdate処理なし
    public void UpdateManager(){

    }

    //アクションの入力を有効に切り替え
    public void InputEnable(){
        string _useCrystalActionNo = "LaunchCrystal" + _useCrystalNo;
        //InputSystemのコールバックをセット
        _playerInput.actions[_useCrystalActionNo].started += OnInputStart;
        _playerInput.actions[_useCrystalActionNo].performed += OnInputComplete;
        _playerInput.actions[_useCrystalActionNo].canceled += OnInputEnd;
        _activeFlag = true;
    }

    //アクションの入力を無効に切り替え
    public void InputDisable(){
        string _useCrystalActionNo = "LaunchCrystal" + _useCrystalNo;
        //InputSystemのコールバックをセット
        _playerInput.actions[_useCrystalActionNo].started -= OnInputStart;
        _playerInput.actions[_useCrystalActionNo].performed -= OnInputComplete;
        _playerInput.actions[_useCrystalActionNo].canceled -= OnInputEnd;
        _activeFlag = false;
    }

    //アクションが実行可能な状態か
    public bool CanAction(){
        //正面に黒クリスタルがあるか
        return BlackCrystalCheck();
    }
    //アクションコストが足りているか
    public bool EnoughActionCost(){
        return _actionCost.EnoughCrystalCost(_useCrystalParam._launchCost);
    }
    //コスト不足時処理
    public void ShortageActionCost(){
        //コスト不足UI表示
        return;
    }

    //正面に黒クリスタルがあるか
    private bool BlackCrystalCheck(){
        Vector2Int _fowardDir = new Vector2Int((int)_playerTr.forward.x, (int)_playerTr.forward.z);
        List<CrystalCore> _crystalCoreList = TargetCore.GetFowardCore<CrystalCore>(MapManager.GetPlayerPos(), _fowardDir, 1);
        return _crystalCoreList.Count != 0 && _crystalCoreList[0]._crystalStatus._name == "BlackCrystal";
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
        //float _colorNo = context.ReadValue<float>();
        Vector2Int _pos = new Vector2Int(MapManager.GetPlayerPos().x + (int)_playerTr.forward.x, MapManager.GetPlayerPos().y + (int)_playerTr.forward.z);
        CrystalCore _crystalCore = MapManager.GetMap(_pos)._crystalCore;
        //コールバック値に対応するプレイヤー装備クリスタルを正面のクリスタルへ格納
        //☆正面のクリスタルに、色毎にステータスをセットし、オブジェクトの色をMaterialで変える　⇨ ScriptableObjectを使用しているが、間にPlayerStatusを挟んで、装備状況に応じて内容を変更させる予定
        _crystalCore.SetCrystalStatus(_useCrystalParam);
        //マップ状況の更新
        _crystalCore.SetOffMap();
        _crystalCore.SetOnMap();
        //クリスタルコストを消費
        _actionCost.ConsumeCrystalCost(_useCrystalParam._launchCost);
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
