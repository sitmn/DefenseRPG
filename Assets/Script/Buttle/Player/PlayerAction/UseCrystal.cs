using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UseCrystal : MonoBehaviour, IPlayerAction
{
    private PlayerInput _playerInput;
    private Transform _playerTr;
    //クリスタル起動アクションの有効無効状態
    public bool IsActive => _isActive;
    private bool _isActive;
    //クリスタル起動中フラグ
    public bool IsAction => _isAction;
    private bool _isAction;
    //プレイヤーモーション実行クラス
    private PlayerMotion _playerMotion;
    //起動クリスタル色変え用マテリアル
    private CrystalStatus[] _setCrystalStatus = new CrystalStatus[3];
    //起動するクリスタルのパラメータ
    //private CrystalParam _useCrystalParam;
    private CrystalParamAsset _useCrystalParamAsset;
    //アクションコスト
    private ActionCost _actionCost;
    private UIManager _UIManager;

    //クラスの初期化
    public void AwakeManager(PlayerParam _playerParam, CrystalParamAsset _crystalParamAsset, UIManager _UIManager){
        InitializeComponent();
        InitializeParam(_crystalParamAsset, _UIManager);
    }

    private void InitializeComponent(){
        _playerInput = MapManager._playerCore.gameObject.GetComponent<PlayerInput>();
        _playerTr = MapManager._playerCore.gameObject.GetComponent<Transform>();
        _actionCost = MapManager._playerCore.gameObject.GetComponent<ActionCost>();
        _playerMotion = this.GetComponent<PlayerMotion>();
    }

    private void InitializeParam(CrystalParamAsset _crystalParamAsset, UIManager _UIManager){
        this._useCrystalParamAsset = _crystalParamAsset;
        _isActive = false;
        _isAction = false;
        this._UIManager = _UIManager;
    }

    //本クラスはUpdate処理なし
    public void UpdateManager(){

    }

    //アクションの入力を有効に切り替え
    public void InputEnable(){
        //InputSystemのコールバックをセット
        _playerInput.actions[ConstManager._launchInput].started += OnInputStart;
        _playerInput.actions[ConstManager._launchInput].performed += OnInputComplete;
        _playerInput.actions[ConstManager._launchInput].canceled += OnInputEnd;
        _isActive = true;
        //起動ボタンを非透明に
        _UIManager._launchButton.SetOpacityButton();
    }

    //アクションの入力を無効に切り替え
    public void InputDisable(){
        //InputSystemのコールバックをセット
        _playerInput.actions[ConstManager._launchInput].started -= OnInputStart;
        _playerInput.actions[ConstManager._launchInput].performed -= OnInputComplete;
        _playerInput.actions[ConstManager._launchInput].canceled -= OnInputEnd;
        _isActive = false;
        //起動ボタンを透明に
        _UIManager._launchButton.SetTransparentButton();
    }

    //アクションが実行可能な状態か
    public bool CanAction(){
        //正面に黒クリスタルがあるか
        return BlackCrystalCheck();
    }
    //アクションコストが足りているか
    public bool EnoughActionCost(){
        return _actionCost.EnoughCrystalCost(_useCrystalParamAsset.CrystalParamList[_UIManager._selectLaunchButtonList._selectButtonNo.Value]._cost[0]);
    }
    //コスト不足時処理
    public void ShortageActionCost(){
        //コスト不足UI表示
        return;
    }

    //正面に黒クリスタルがあるか
    private bool BlackCrystalCheck(){
        Vector2Int _fowardDir = new Vector2Int((int)_playerTr.forward.x, (int)_playerTr.forward.z);
        List<CrystalCoreBase> _crystalCoreList = TargetCore.GetFowardCore<CrystalCoreBase>(MapManager.GetPlayerPos(), _fowardDir, 1);
        return _crystalCoreList.Count != 0 && _crystalCoreList[0]._crystalStatus._crystalParam._crystalCoreName == "BlackCrystal";
    }

    /// <summary>
    /// モーション開始メソッド
    /// </summary>
    /// <param name="_playerMotion">プレイヤーモーションのコンポーネント</param>
    private void StartMotion(){
        _playerMotion.StartLaunchMotion();
    }

    /// <summary>
    /// モーション終了メソッド
    /// </summary>
    /// <param name="_playerMotion">プレイヤーモーションのコンポーネント</param>
    private void EndMotion(){
        _playerMotion.EndLaunchMotion();
    }

    //クリスタル起動開始
    private void OnInputStart(InputAction.CallbackContext context){
        //起動中フラグ（移動不可）
        _isAction = true;
        //起動モーション開始
        StartMotion();
        //起動時間UI表示
        _UIManager._actionGauge.SetTween(ConstManager._rankUpCount);
    }

    //クリスタル起動完了(長押し)
    private void OnInputComplete(InputAction.CallbackContext context){
        //float _colorNo = context.ReadValue<float>();
        Vector2Int _pos = new Vector2Int(MapManager.GetPlayerPos().x + (int)_playerTr.forward.x, MapManager.GetPlayerPos().y + (int)_playerTr.forward.z);
        CrystalCoreBase _crystalCore = MapManager.GetMap(_pos)._crystalCore;
        //コールバック値に対応するプレイヤー装備クリスタルを正面のクリスタルへ格納
        //☆正面のクリスタルに、色毎にステータスをセットし、オブジェクトの色をMaterialで変える　⇨ ScriptableObjectを使用しているが、間にPlayerStatusを挟んで、装備状況に応じて内容を変更させる予定
        _crystalCore.SetCrystalStatus(_useCrystalParamAsset.CrystalParamList[_UIManager._selectLaunchButtonList._selectButtonNo.Value]);
        //マップ状況の更新
        _crystalCore.SetOffMap();
        _crystalCore.SetOnMap();
        //クリスタルコストを消費
        _actionCost.ConsumeCrystalCost(_useCrystalParamAsset.CrystalParamList[_UIManager._selectLaunchButtonList._selectButtonNo.Value]._cost[0]);
        //起動時間UI非表示

        //起動モーション終了、起動中フラグ取り消し
        EndMotion();
        _isAction = false;
    }

    //クリスタル起動キャンセル
    private void OnInputEnd(InputAction.CallbackContext context){
        //起動モーション終了、起動中フラグ取り消し
        EndMotion();
        _isAction = false;

        //起動時間UI非表示
        _UIManager._actionGauge.CancelTween();
    }

}
