using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LiftDownCrystal : MonoBehaviour, IPlayerAction
{
    private PlayerInput _playerInput;
    private Transform _playerTr;
    //クリスタルリフトダウンアクションの有効無効状態
    public bool IsActive => _isActive;
    private bool _isActive;
    //クリスタルリフトダウン中フラグ
    public bool IsAction => _isAction;
    private bool _isAction;
    

    //クラスの初期化
    public void AwakeManager(PlayerParam _playerParam){
        _playerInput = this.gameObject.GetComponent<PlayerInput>();
        _playerTr = this.gameObject.GetComponent<Transform>();
        _isActive = false;
        _isAction = false;
    }

    //リフトアップ中のクリスタルをプレイヤー移動に合わせて移動
    public void UpdateManager(){
        if(PlayerCore.GetLiftCrystalTr() == null) return;
        PlayerCore.GetLiftCrystalTr().position = _playerTr.position + new Vector3(0, 2, 0);
    }

    //リフトアップアクション入力の有効化
    public void InputEnable(){
        //InputSystemのコールバックをセット
        _playerInput.actions["LiftDown"].started += OnInputStart;
        _playerInput.actions["LiftDown"].performed += OnInputComplete;
        _playerInput.actions["LiftDown"].canceled += OnInputEnd;
        _isActive = true;
    }

    //リフトアップアクション入力の無効化
    public void InputDisable(){
        //InputSystemのコールバックをセット
        _playerInput.actions["LiftDown"].started -= OnInputStart;
        _playerInput.actions["LiftDown"].performed -= OnInputComplete;
        _playerInput.actions["LiftDown"].canceled -= OnInputEnd;
        _isActive = false;
    }

    //アクションが実行可能な状態か
    public bool CanAction(){
        ////リフト中のクリスタルがある、かつ、正面にクリスタルがない、かつ、周囲に敵がいないとき
        return PlayerCore.GetLiftCrystalTr() != null && !ExistCrystal() && !ExistAroundEnemy();
    }

    //正面にクリスタルがあるか
    private bool ExistCrystal(){
        Vector2Int _fowardDir = new Vector2Int((int)_playerTr.forward.x, (int)_playerTr.forward.z);
        List<CrystalCore> _crystalCoreList = TargetCore.GetFowardCore<CrystalCore>(MapManager.GetPlayerPos(), _fowardDir, 1);
        return MapManager.IsOutOfReference(MapManager.GetPlayerPos() + _fowardDir) || (_crystalCoreList.Count != 0 && _crystalCoreList[0] != null);
    }

    //正面のマスの周囲に敵がいるか
    private bool ExistAroundEnemy(){
        Vector2Int _fowardDir = new Vector2Int((int)_playerTr.forward.x, (int)_playerTr.forward.z);
        List<EnemyCoreBase> _crystalCoreList = TargetCore.GetAroundCore<EnemyCoreBase>(MapManager.GetPlayerPos() + _fowardDir, _fowardDir, 1);
        return _crystalCoreList.Count != 0;
    }

    //アクションコストが足りているか
    public bool EnoughActionCost(){
        return true;
    }
    public void ShortageActionCost(){
        return;
    }

    //InputSystem 正面に黒以外のクリスタルがある時のみ実行
    //クリスタルリフトダウン開始
    private void OnInputStart(InputAction.CallbackContext context){
        //リフトダウン中フラグ（移動不可）
        _isAction = true;
        //リフトダウンモーション開始

        //起動時間UI表示
        
    }

    //クリスタルリフトダウン完了(長押し)
    private void OnInputComplete(InputAction.CallbackContext context){
        //オブジェクトをマスへ配置
        PlayerCore.GetLiftCrystalTr().position = new Vector3(_playerTr.position.x + (int)_playerTr.forward.x, 0.5f, _playerTr.position.z + (int)_playerTr.forward.z);
        //プレイヤーの次の移動先が重複している場合、移動をキャンセル
        //マップ情報に水晶を追加
        PlayerCore.GetLiftCrystalCore().SetOnMap();
        //リフト中情報をnullに
        PlayerCore.SetOffLiftCrystal();

        //格納したマスが移動先になっているエネミーがいれば再度経路探索

        //起動時間UI非表示

        //リフトダウンモーション終了、リフトダウン中フラグ取り消し
        _isAction = false;
    }

    //クリスタルリフトダウンキャンセル
    private void OnInputEnd(InputAction.CallbackContext context){
        //リフトダウンモーション終了、リフトダウン中フラグ取り消し
        _isAction = false;

        //起動時間UI非表示
    }
}
