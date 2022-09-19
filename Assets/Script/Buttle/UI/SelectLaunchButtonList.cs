using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.InputSystem;

public class SelectLaunchButtonList : MonoBehaviour
{
    private PlayerInput _playerInput;
    //選択中のボタンNo
    public ReactiveProperty<int> _selectButtonNo;
    [SerializeField]
    private SelectLaunchButton[] _selectLaunchButtonArr = new SelectLaunchButton[5];

    public void AwakeManager(PlayerInput _playerInput){
        this._playerInput = _playerInput;
        _selectButtonNo = new ReactiveProperty<int>(1);
        //各選択ボタンを初期化
        for(int i = 0; i < _selectLaunchButtonArr.Length ;i++){
            _selectLaunchButtonArr[i].AwakeManager();
        }
        //選択ボタンのactionMapをすべてセット
        for(int i = 1; i < ConstManager._possettionCrystalAmount + 1 ; i++){
            SetPlayerAction(i);
        }
        CreateSelectButtonStream();
        SetButtonListActive(0);
    }

    //選択ボタンの入力をセット
    private void SetPlayerAction(int _crystalNo){
        string _actionName = ConstManager._selectInput + _crystalNo;
        _playerInput.actions[_actionName].started += OnStartSelect;
        // _playerInput.actions[_actionName].performed += 
        // _playerInput.actions[_actionName].canceled += 
    }

    //選択ボタンのUI反映
    private void CreateSelectButtonStream(){
        _selectButtonNo.Subscribe((x) => {
            SetButtonListActive(x - 1);
        }).AddTo(this);
    }

    //選択ボタンのUI反映。選択したボタンのみ非透明に変更
    private void SetButtonListActive(int _selectButtonNo){
        for(int i = 0; i < _selectLaunchButtonArr.Length; i++){
            if(_selectButtonNo == i) _selectLaunchButtonArr[i].SetOpacityButton();
            else _selectLaunchButtonArr[i].SetTransparentButton();
        }
    }

    //ボタン入力時のコールバック
    private void OnStartSelect(InputAction.CallbackContext _context){
        //選択中のボタンを変更
        _selectButtonNo.Value = (int)_context.ReadValue<float>();
    }
    
}
