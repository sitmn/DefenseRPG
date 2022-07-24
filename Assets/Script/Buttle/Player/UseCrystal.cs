using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UseCrystal : MonoBehaviour, IUserCrystal
{
    private PlayerStatus _playerStatus;
    private PlayerInput _playerInput;
    private Transform _playerTr;
    //水晶起動アクションの有効無効状態
    public bool LaunchActiveFlag => _launchActiveFlag;
    private bool _launchActiveFlag;
    //水晶起動中フラグ
    public bool LaunchActionFlag => _launchActionFlag;
    private bool _launchActionFlag;

    //起動水晶色変え用マテリアル
    private ICrystalStatus[] _setCrystalStatus = new ICrystalStatus[3];

    void Awake(){
        _playerStatus = this.gameObject.GetComponent<PlayerStatus>();
        _playerInput = this.gameObject.GetComponent<PlayerInput>();
        _playerTr = this.gameObject.GetComponent<Transform>();
        _launchActiveFlag = false;
        _launchActionFlag = false;
    }

    //テスト用
    void Start(){
        SetCrystalStatusDeck();
    }

    public void LaunchEnable(){
        //InputSystemのコールバックをセット
        _playerInput.actions["Launch"].started += OnLaunchStart;
        _playerInput.actions["Launch"].performed += OnLaunchComplete;
        _playerInput.actions["Launch"].canceled += OnLaunchEnd;
        _launchActiveFlag = true;
    }

    public void LaunchDisable(){
        //InputSystemのコールバックをセット
        _playerInput.actions["Launch"].started -= OnLaunchStart;
        _playerInput.actions["Launch"].performed -= OnLaunchComplete;
        _playerInput.actions["Launch"].canceled -= OnLaunchEnd;
        _launchActiveFlag = false;
    }

    //プレイヤーが装備しているクリスタルのステータスをセット
    public void SetCrystalStatusDeck(){
        _setCrystalStatus[0] = _playerStatus._crystalStatus[0];
        _setCrystalStatus[0]._material = _playerStatus._material[0];
        _setCrystalStatus[1] = _playerStatus._crystalStatus[1];
        _setCrystalStatus[1]._material = _playerStatus._material[1];
    }

    //前方にクリスタルがあるかを確認
    public bool CrystalCheck(){
        bool _checkCrystal = false;
        if(AStarMap.astarMas[AStarMap._playerPos.x + (int)_playerTr.forward.x, AStarMap._playerPos.y + (int)_playerTr.forward.z].obj.Count == 1){
            _checkCrystal = (AStarMap.astarMas[AStarMap._playerPos.x + (int)_playerTr.forward.x, AStarMap._playerPos.y + (int)_playerTr.forward.z].obj[0].GetType().Name == "CrystalController");
        }
        
        return _checkCrystal;
    }

    //InputSystem 正面に黒クリスタルがある時のみ実行
    //クリスタル起動開始
    private void OnLaunchStart(InputAction.CallbackContext context){
        //起動中フラグ（移動不可）
        _launchActionFlag = true;
        //起動モーション開始

        //起動時間UI表示
        
        Debug.Log("AAA");
    }

    //クリスタル起動完了(長押し)
    private void OnLaunchComplete(InputAction.CallbackContext context){
        Debug.Log("BBB");
        float _colorNo = context.ReadValue<float>();

        ICrystalController _crystalController = AStarMap.astarMas[AStarMap._playerPos.x + (int)_playerTr.forward.x, AStarMap._playerPos.y + (int)_playerTr.forward.z].obj[0] as ICrystalController;
        //コールバック値に対応するプレイヤー装備クリスタルを正面のクリスタルへ格納
        //☆正面のクリスタルに、色ごとのクリスタルステータスを格納し、オブジェクトの色をMaterialで変える
        _crystalController.SetCrystalType(_setCrystalStatus[(int)_colorNo - 1]);
        //正面クリスタルの色変え

        //起動時間UI非表示

        //起動モーション終了、起動中フラグ取り消し
        _launchActionFlag = false;
    }

    //クリスタル起動キャンセル
    private void OnLaunchEnd(InputAction.CallbackContext context){
        Debug.Log("CCC");
        //起動モーション終了、起動中フラグ取り消し
        _launchActionFlag = false;

        //起動時間UI非表示
    }

}
