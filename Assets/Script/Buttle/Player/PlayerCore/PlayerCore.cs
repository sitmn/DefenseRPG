using UnityEngine;
using System.Collections.Generic;

public class PlayerCore : MonoBehaviour
{
    [System.NonSerialized]
    public PlayerMove _playerMove;
    [System.NonSerialized]
    public PlayerStatus _playerStatus;
    //プレイヤーアクション配列
    private List<IPlayerAction> _playerActionList = new List<IPlayerAction>();
    [System.NonSerialized]
    public GameObject _speedBuff;
    [System.NonSerialized]
    public GameObject _speedDebuff;
    //リフト中のクリスタルオブジェクト
    public static CrystalCoreBase _liftCrystalCore;
    public static CrystalCoreBase GetLiftCrystalCore(){
        return _liftCrystalCore;
    }
    
    public static Transform _liftCrystalTr;
    public static Transform GetLiftCrystalTr(){
        return _liftCrystalTr;
    }
    //リフト中のクリスタル情報をセット
    public static void SetLiftCrystal(CrystalCoreBase _crystalCore, Transform _crystalTr){
        _liftCrystalCore = _crystalCore;
        _liftCrystalTr = _crystalTr;
    }
    public static void SetOffLiftCrystal(){
        _liftCrystalCore = null;
        _liftCrystalTr = null;
    }
    
    //クラスの初期化
    public void AwakeManager(PlayerParam _playerParam, CrystalParamAsset _crystalParamData){
        _playerStatus = new PlayerStatus(_playerParam);
        MapManager.SetPlayerCore(this);
        InitializeComponent(_crystalParamData);
    }

    //コンポーネントの初期化
    private void InitializeComponent(CrystalParamAsset _crystalParamData){
        //バフオブジェクトの取得
        _speedBuff = transform.GetChild(0).gameObject;
        _speedDebuff = transform.GetChild(1).gameObject;

        _playerMove = this.gameObject.GetComponent<PlayerMove>();
        //UseCrystalをインスタンス化し、各クリスタルステータスをセット（0の黒クリスタルは含めないため1からスタート）
        for(int i = 1; i < _crystalParamData.CrystalParamList.Count; i++){
            _playerActionList.Add(new UseCrystal(_crystalParamData.CrystalParamList[i], i));
        }
        //UseCrystal以外のPlayerActionをリストへ追加
        _playerActionList.Add(this.gameObject.GetComponent<LiftUpCrystal>());
        _playerActionList.Add(this.gameObject.GetComponent<LiftDownCrystal>());
        _playerActionList.Add(this.gameObject.GetComponent<RepairCrystal>());
        _playerActionList.Add(this.gameObject.GetComponent<CrystalRankUp>());
    }

    //Update処理
    public void UpdateManager()
    {
        // foreach(var _playerAction in _playerActionList){
        //     Debug.Log(_playerAction.IsAction + "_"+ _playerAction.IsActive);
        // }
        
        if(GameManager.GetIsGameOver()) return;
        //有効になっているプレイヤーアクションが実行できない状態の場合、無効化
        foreach(var _playerAction in _playerActionList){
            if(_playerAction.IsActive && (!_playerAction.CanAction() || !_playerAction.EnoughActionCost())){
                _playerAction.InputDisable();
            }
        }
        
        //水晶起動、修理、持ち上げアクション中か？
        //移動中か？
        if(_playerMove.IsMove()){
            _playerMove.Move(_playerStatus.GetMoveSpeed);
            return;
        }else{
            //アクション中なら以降の処理をスキップ
            foreach(var _playerAction in _playerActionList){
                if(_playerAction.IsAction) return;
            }
            //行動していないとき、アクション可能であれば入力を有効化
            foreach(var _playerAction in _playerActionList){
                if(!_playerAction.IsActive && _playerAction.CanAction()){
                    if(_playerAction.EnoughActionCost()){
                        _playerAction.InputEnable();
                    }else{
                        _playerAction.ShortageActionCost();
                    }
                    
                }
            }
            //移動場所を設定
            _playerMove.SetNextMovePos();
            //プレイヤーの向き変更
            _playerMove.RotatePlayer();
            //移動先を確定させた場合、アクションを無効化
            if(_playerMove.IsMove()){
                InputInvalid();
            }
        }
    }

    //プレイヤーアクションを無効化
    public void InputInvalid(){
        foreach(var _playerAction in _playerActionList){
            if(_playerAction.IsActive) _playerAction.InputDisable();
        }
    }
}
