using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class CrystalRankUp : MonoBehaviour, IPlayerAction
{
    //クリスタルの最大レベル
    [SerializeField]
    private int _max_level;
    private PlayerInput _playerInput;
    private Transform _playerTr;
    //クリスタル修復アクション有効フラグ
    public bool IsActive => _isActive;
    private bool _isActive;
    //クリスタル修復アクション中フラグ
    public bool IsAction => _isAction;
    private bool _isAction;
    //アクションコスト
    private ActionCost _actionCost;

    public void AwakeManager(PlayerParam _playerParam){
        SetComponent();
        SetParam();
    }
    //コンポーネントをセット
    private void SetComponent(){
        _playerInput = this.gameObject.GetComponent<PlayerInput>();
        _playerTr = this.gameObject.GetComponent<Transform>();
        _actionCost = MapManager._playerCore.gameObject.GetComponent<ActionCost>();
    }

    //初期パラメータのセット
    private void SetParam(){
        _isActive = false;
        _isAction = false;
    }

    public void UpdateManager(){

    }

    //回復アクションの有効化
    public void InputEnable(){
        _playerInput.actions["RankUp"].started += OnInputStart;
        _playerInput.actions["RankUp"].performed += OnInputCompleted;
        _playerInput.actions["RankUp"].canceled += OnInputCanceled;

    Debug.Log("EEE");
        _isActive = true;
    }
    //回復アクションの無効化
    public void InputDisable(){
        _playerInput.actions["RankUp"].started -= OnInputStart;
        _playerInput.actions["RankUp"].performed -= OnInputCompleted;
        _playerInput.actions["RankUp"].canceled -= OnInputCanceled;

        _isActive = false;
        _isAction = false;
    }

    //アクションが実行可能な状態か
    public bool CanAction(){
        //リフト中のクリスタルがなく、正面に黒以外のクリスタルがあるか
        return PlayerCore.GetLiftCrystalTr() == null && ExistCrystal() && !ExistBlackCrystal();
    }

    //正面にクリスタルがあるか
    private bool ExistCrystal(){
        Vector2Int _fowardDir = new Vector2Int((int)_playerTr.forward.x, (int)_playerTr.forward.z);
        List<CrystalCore> _crystalCoreList = TargetCore.GetFowardCore<CrystalCore>(MapManager.GetPlayerPos(), _fowardDir, 1);
        return _crystalCoreList.Count != 0 && _crystalCoreList[0] != null;
    }

    //正面に黒クリスタルがあるか
    private bool ExistBlackCrystal(){
        Vector2Int _fowardDir = new Vector2Int((int)_playerTr.forward.x, (int)_playerTr.forward.z);
        List<CrystalCore> _crystalCoreList = TargetCore.GetFowardCore<CrystalCore>(MapManager.GetPlayerPos(), _fowardDir, 1);
        return _crystalCoreList.Count != 0 && _crystalCoreList[0]._crystalStatus._crystalParam._crystalCoreName == "BlackCrystal";
    }

    //アクションコストが足りているか
    public bool EnoughActionCost(){
        bool _isEnouughCost = false;
        //プレイヤー正面のクリスタルのステータスを取得
        CrystalStatus _crystalStatus = GetFowardCrystalStatus();

        if(_crystalStatus != null && _crystalStatus._level < _max_level - 1){
            //RankUpのコストが足りているか
            _isEnouughCost = _actionCost.EnoughCrystalCost(_crystalStatus._crystalParam._cost[_crystalStatus._level + 1]);
        }
        return _isEnouughCost;
    }

    //プレイヤー正面のクリスタルのステータスを取得
    private CrystalStatus GetFowardCrystalStatus(){
        CrystalStatus _crystalStatus = null;
        //正面のクリスタルに対して
        Vector2Int _crystalPos = MapManager.CastMapPos(_playerTr.position) + new Vector2Int((int)_playerTr.forward.x, (int)_playerTr.forward.z); 
        if(MapManager.GetMap(_crystalPos)._crystalCore != null) _crystalStatus = MapManager.GetMap(_crystalPos)._crystalCore._crystalStatus;

        return _crystalStatus;
    }

    //コスト不足時処理
    public void ShortageActionCost(){
        //コスト不足UI表示
        return;
    }

    //正面のクリスタルをランクアップ
    private void RankUpCrystal(CrystalStatus _crystalStatus){
        //Rank変数の上限アップ
        _crystalStatus._level ++;
    }

    //クリスタルrankUp開始
    private void OnInputStart(InputAction.CallbackContext context){
        //起動中フラグ（移動不可）
        _isAction = true;
        //起動モーション開始

        //起動時間UI表示
        
    }

    //クリスタルRankUp完了(長押し)
    private void OnInputCompleted(InputAction.CallbackContext context){
        CrystalStatus _crystalStatus = GetFowardCrystalStatus();
        //コールバック値に対応するプレイヤー装備クリスタルを正面のクリスタルへ格納
        //☆正面のクリスタルに、色毎にステータスをセットし、オブジェクトの色をMaterialで変える　⇨ ScriptableObjectを使用しているが、間にPlayerStatusを挟んで、装備状況に応じて内容を変更させる予定
        //_crystalCore.SetCrystalStatus(_useCrystalParam);
        //正面クリスタルのRankUp
        RankUpCrystal(_crystalStatus);
        //クリスタルコストを消費
        _actionCost.ConsumeCrystalCost(_crystalStatus._crystalParam._cost[_crystalStatus._level]);
        //起動時間UI非表示

        //起動モーション終了、起動中フラグ取り消し
        _isAction = false;
    }

    //クリスタルRankUpキャンセル
    private void OnInputCanceled(InputAction.CallbackContext context){
        //起動モーション終了、起動中フラグ取り消し
        _isAction = false;

        //起動時間UI非表示
    }
}
