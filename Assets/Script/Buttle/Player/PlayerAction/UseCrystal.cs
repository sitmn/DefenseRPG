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
    public void AwakeManager(PlayerParam _playerParam){
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
        string a = "nch";
        //InputSystemのコールバックをセット
        _playerInput.actions["Lau"+ a].started += OnInputStart;
        _playerInput.actions["Lau"+ a].performed += OnInputComplete;
        _playerInput.actions["Lau"+ a].canceled += OnInputEnd;
        _activeFlag = true;
    }

    //アクションの入力を無効に切り替え
    public void InputDisable(){
        string a = "nch";
        //InputSystemのコールバックをセット
        _playerInput.actions["Lau"+ a].started -= OnInputStart;
        _playerInput.actions["Lau"+ a].performed -= OnInputComplete;
        _playerInput.actions["Lau"+ a].canceled -= OnInputEnd;
        _activeFlag = false;
    }

    //アクションが実行可能な状態か
    public bool CanAction(){
        //正面に黒クリスタルがあるか
        return BlackCrystalCheck();
    }
    //アクションコストが足りているか
    public bool EnoughActionCost(ActionCost _actionCost){
        Vector2Int _fowardDir = new Vector2Int((int)_playerTr.forward.x, (int)_playerTr.forward.z);
        List<CrystalCore> _crystalCoreList = TargetCore.GetFowardCore<CrystalCore>(MapManager.GetPlayerPos(), _fowardDir, 1);
        if(_crystalCoreList.Count != 0) return _actionCost.EnoughCrystalCost(_crystalCoreList[0]._crystalStatus._launchCost);
        return false;
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
        float _colorNo = context.ReadValue<float>();
        Vector2Int _pos = new Vector2Int(MapManager.GetPlayerPos().x + (int)_playerTr.forward.x, MapManager.GetPlayerPos().y + (int)_playerTr.forward.z);
        CrystalCore _crystalCore = MapManager.GetMap(_pos)._crystalCore as CrystalCore;
        //コールバック値に対応するプレイヤー装備クリスタルを正面のクリスタルへ格納
        //☆正面のクリスタルに、色毎にステータスをセットし、オブジェクトの色をMaterialで変える　⇨ ScriptableObjectを使用しているが、間にPlayerStatusを挟んで、装備状況に応じて内容を変更させる予定
        _crystalCore.SetCrystalStatus(_crystalParamData.CrystalParamList[(int)_colorNo]);
        //マップ状況の更新
        _crystalCore.SetOffMap();
        _crystalCore.SetOnMap();
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
