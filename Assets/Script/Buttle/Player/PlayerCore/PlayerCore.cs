using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    public PlayerMove _playerMove;
    public PlayerStatus _playerStatus;
    public CrystalStatus[] EqueipmentCrystal{get;set;}
    [SerializeField]
    private CrystalStatus[] _equipmentCrystal = new CrystalStatus[3];

    private IPlayerAction[] _playerActionArr = new IPlayerAction[4];
    private Transform _playerTr;
    [System.NonSerialized]
    public GameObject _speedBuff;
    [System.NonSerialized]
    public GameObject _speedDebuff;
    //リフト中のクリスタルオブジェクト
    public static CrystalCore _liftCrystalCore;
    public static CrystalCore GetLiftCrystalCore(){
        return _liftCrystalCore;
    }
    
    public static Transform _liftCrystalTr;
    public static Transform GetLiftCrystalTr(){
        return _liftCrystalTr;
    }
    //リフト中のクリスタル情報をセット
    public static void SetLiftCrystal(CrystalCore _crystalCore, Transform _crystalTr){
        _liftCrystalCore = _crystalCore;
        _liftCrystalTr = _crystalTr;
    }
    public static void SetOffLiftCrystal(){
        _liftCrystalCore = null;
        _liftCrystalTr = null;
    }
    
    //クラスの初期化
    public void AwakeManager(PlayerParam _playerParam){
        InitializeComponent();
        _playerStatus = new PlayerStatus(_playerParam);
        AStarMap.SetPlayerCore(this);
    }

    //コンポーネントの初期化
    private void InitializeComponent(){
        //バフオブジェクトの取得
        _speedBuff = transform.GetChild(0).gameObject;
        _speedDebuff = transform.GetChild(1).gameObject;

        _playerMove = this.gameObject.GetComponent<PlayerMove>();
        _playerActionArr[0] = this.gameObject.GetComponent<UseCrystal>();
        _playerActionArr[1] = this.gameObject.GetComponent<LiftUpCrystal>();
        _playerActionArr[2] = this.gameObject.GetComponent<LiftDownCrystal>();
        _playerActionArr[3] = this.gameObject.GetComponent<RepairCrystal>();
        _playerTr = this. gameObject.GetComponent<Transform>();
    }

    //Update処理
    public void UpdateManager()
    {
        if(GameManager.GetIsGameOver()) return;
        //有効になっているプレイヤーアクションが実行できない状態の場合、無効化
        foreach(var _playerAction in _playerActionArr){
            if(_playerAction.ActiveFlag && !_playerAction.CanAction()){
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
            foreach(var _playerAction in _playerActionArr){
            if(_playerAction.ActionFlag) return;
            }
            //行動していないとき、アクション可能であれば入力を有効化
            foreach(var _playerAction in _playerActionArr){
                if(!_playerAction.ActiveFlag && _playerAction.CanAction()){
                    _playerAction.InputEnable();
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
        foreach(var _playerAction in _playerActionArr){
            if(_playerAction.ActiveFlag) _playerAction.InputDisable();
        }
    }
}
