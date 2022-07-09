using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private CharacterController _characterController;

    private Transform _playerTr;

    private IPlayerMotion _playerMotion;

    private PlayerInput _playerInput;

    [SerializeField]
    private float _moveSpeed = 5;

    [SerializeField]
    private IStagePos stagePos;

    private Vector2 _moveDir;

    // Start is called before the first frame update
    void Awake()
    {
        SetComponent();
    }

    private void SetComponent(){
        _playerMotion = this.gameObject.GetComponent<IPlayerMotion>();
        _playerInput = this.gameObject.GetComponent<PlayerInput>();
        _characterController = this.gameObject.GetComponent<CharacterController>();
        _playerTr = this.gameObject.GetComponent<Transform>();
    }

    //InputSystemのアクションにコールバックをセット
    void OnEnable(){
        _playerInput.actions["Move"].started += MoveStart;
        _playerInput.actions["Move"].performed += OnMove;
        _playerInput.actions["Move"].canceled += MoveStop;
    }

    void OnDisable(){
        _playerInput.actions["Move"].started -= MoveStart;
        _playerInput.actions["Move"].performed -= OnMove;
        _playerInput.actions["Move"].canceled -= MoveStop;
    }

    //プレイヤー移動
    private void MovePlayer(Vector2 _moveDir){
        if(MoveCheck(_moveDir)){
            Vector3 player_move = new Vector3(_moveDir.x, 0, _moveDir.y);

            _characterController.Move(player_move * _moveSpeed * Time.deltaTime);
        }
        return;
    }

    //プレイヤー向き変更
    private void RotatePlayer(Vector2 _moveDir){
        Quaternion _rotateDir = Quaternion.LookRotation(new Vector3(_moveDir.x,0,_moveDir.y));
        _playerTr.rotation = Quaternion.Slerp(_playerTr.rotation, _rotateDir, 0.3f);
    }

    //移動可能か：敵、水晶、エリア外が移動方向に存在しないか
    private bool MoveCheck(Vector2 _moveDir){
        
        return true; //this.pos + _moveDir のstagePos.stageObjListが存在しないか
    }

    void Update(){
        if(_moveDir != Vector2.zero){
            MovePlayer(_moveDir);
            RotatePlayer(_moveDir);
        }
        
    }


    //移動コールバック用
    private void MoveStart(InputAction.CallbackContext context){
        _playerMotion.MoveMotion();
    }

    //移動コールバック用
    private void OnMove(InputAction.CallbackContext context){
        _moveDir = context.ReadValue<Vector2>();
    }
    //移動解除コールバック用
    private void MoveStop(InputAction.CallbackContext context){
        Debug.Log("FFF");

        _playerMotion.MoveMotionCancel();
        _moveDir = context.ReadValue<Vector2>();
    }
}
