using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UseCrystal : MonoBehaviour
{
    private PlayerInput _playerInput;

    void Awake(){
        _playerInput = this.gameObject.GetComponent<PlayerInput>();
    }

    public void LaunchEnable(){
        //InputSystemのコールバックをセット
        _playerInput.actions["Launch"].started += OnLaunchStart;
        _playerInput.actions["Launch"].performed += OnLaunchComplete;
        _playerInput.actions["Launch"].canceled += OnLaunchEnd;
    }

    public void LaunchDisable(){
        //InputSystemのコールバックをセット
        _playerInput.actions["Launch"].started -= OnLaunchStart;
        _playerInput.actions["Launch"].performed -= OnLaunchComplete;
        _playerInput.actions["Launch"].canceled -= OnLaunchEnd;
    }

    //InputSystem 正面に黒クリスタルがある時のみ実行
    //クリスタル起動開始
    private void OnLaunchStart(InputAction.CallbackContext context){
        //起動モーション開始
        Debug.Log(context.ReadValue<float>()+"!!!");
        //起動時間UI表示
        
        //起動中フラグ（移動不可）
    }

    //クリスタル起動完了(長押し)
    private void OnLaunchComplete(InputAction.CallbackContext context){
        //コールバック値に対応するプレイヤー装備クリスタルを正面のクリスタルへ格納
Debug.Log(context.ReadValue<float>()+"$$$");
        //正面クリスタルの色変え

        //起動時間UI非表示

        //起動モーション終了、起動中フラグ取り消し
    }

    //クリスタル起動キャンセル
    private void OnLaunchEnd(InputAction.CallbackContext context){
        //起動モーション終了、起動中フラグ取り消し

        //起動時間UI非表示
    }

}
