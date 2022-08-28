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
    //修復にかかるコスト
    private int _repairActionCost;

    public void AwakeManager(PlayerParam _playerParam){
        _playerInput = this.gameObject.GetComponent<PlayerInput>();
        _playerTr = this.gameObject.GetComponent<Transform>();

        SetParam(_playerParam);
    }

    //初期パラメータのセット
    private void SetParam(PlayerParam _playerParam){
        _activeFlag = false;
        _actionFlag = false;

        _repairCount = 0;
        _repairMaxCount = _playerParam._repairMaxCount;
        _repairPoint = _playerParam._repairPoint;
        _repairActionCost = _playerParam._repairActionCost;
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
        List<CrystalCore> _crystalCoreList = TargetCore.GetFowardCore<CrystalCore>(MapManager.GetPlayerPos(), _fowardDir, 1);
        return _crystalCoreList.Count != 0 && _crystalCoreList[0] != null;
    }

    //正面に黒クリスタルがあるか
    private bool ExistBlackCrystal(){
        Vector2Int _fowardDir = new Vector2Int((int)_playerTr.forward.x, (int)_playerTr.forward.z);
        List<CrystalCore> _crystalCoreList = TargetCore.GetFowardCore<CrystalCore>(MapManager.GetPlayerPos(), _fowardDir, 1);
        return _crystalCoreList.Count != 0 && _crystalCoreList[0]._crystalStatus._name == "BlackCrystal";
    }

    //アクションコストが足りているか
    public bool EnoughActionCost(ActionCost _actionCost){
        return _actionCost.EnoughCrystalCost(_repairActionCost);
    }
    //コスト不足時処理
    public void ShortageActionCost(){
        //コスト不足UI表示
        return;
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
        Vector2Int _judgePos = new Vector2Int(MapManager.GetPlayerPos().x + (int)_playerTr.forward.x, MapManager.GetPlayerPos().y + (int)_playerTr.forward.z);
        //正面に黒以外の水晶があれば回復
        if(MapManager.GetMap(_judgePos)._crystalCore != null){
            MapManager.GetMap(_judgePos)._crystalCore.Hp += _repairPoint;
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
