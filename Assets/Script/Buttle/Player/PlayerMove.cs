using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    //private CharacterController _characterController;

    private Transform _playerTr;
    private IPlayerMotion _playerMotion;
    private PlayerInput _playerInput;

    [SerializeField]
    private float _moveSpeed = 5;
    //移動方向（移動用）
    private Vector2Int _moveDir;
    //直前の移動方向（移動用）
    private Vector2Int _movePreviousDir;
    //今の場所（移動用）
    private Vector2Int _playerPos;
    //次の移動場所（移動用）
    private Vector2Int _nextPlayerPos;
    

    // Start is called before the first frame update
    void Awake()
    {
        SetComponent();
    }

    private void SetComponent(){
        _playerMotion = this.gameObject.GetComponent<IPlayerMotion>();
        _playerInput = this.gameObject.GetComponent<PlayerInput>();
        //_characterController = this.gameObject.GetComponent<CharacterController>();
        _playerTr = this.gameObject.GetComponent<Transform>();

        //AStarMapの自キャラポジション格納（判定用）
        AStarMap._playerPos = AStarMap.CastMapPos(_playerTr.position);
        //自キャラポジション格納（移動用）
        _playerPos = AStarMap._playerPos;
        //次の移動場所（移動用）
        _nextPlayerPos = _playerPos;

        
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

    //隣のマス中心まで移動
    private void MovePlayer(){
        //if(MoveCheck(_moveDir)){
            Vector3 player_move = new Vector3(_nextPlayerPos.x - _playerPos.x, 0, _nextPlayerPos.y - _playerPos.y);

            //_characterController.Move(player_move * _moveSpeed * Time.deltaTime);
            _playerTr.position += player_move * _moveSpeed * Time.deltaTime;
            //マス中心まで移動した時、位置確定
            if(Mathf.Abs(_playerTr.position.x - _nextPlayerPos.x + _playerTr.position.z - _nextPlayerPos.y) < _moveSpeed * Time.deltaTime + 0.1f){
                _playerTr.position = new Vector3(_nextPlayerPos.x, 0 , _nextPlayerPos.y);
                _playerPos = AStarMap.CastMapPos(_playerTr.position);
            }

            AStarMap._playerPos = AStarMap.CastMapPos(_playerTr.position);
        //}
        return;
    }

    //プレイヤー向き変更
    private void RotatePlayer(Vector2Int _moveDir){
        Quaternion _rotateDir = Quaternion.LookRotation(new Vector3(_moveDir.x, 0, _moveDir.y));
        _playerTr.rotation = _rotateDir;
    }

    //移動可能か：敵、水晶、エリア外が移動方向に存在しないか
    private bool MoveCheck(Vector2Int _moveDir){
        return AStarMap.astarMas[AStarMap._playerPos.x + (int)_moveDir.x, AStarMap._playerPos.y + (int)_moveDir.y].obj.Count == 0;//this.pos + _moveDir のAStarMap.Objが存在しないか
    }

    void Update(){
        //次の場所まで移動
        if(_nextPlayerPos != _playerPos){
            MovePlayer();
        }//次の場所を格納
        else{
            NextMovePos(_moveDir);
        }
    }

    //次の目的地を設定（移動用）
    private void NextMovePos(Vector2Int _moveDir){
        if(MoveCheck(_moveDir)) _nextPlayerPos = _playerPos + _moveDir;
    }


    //移動コールバック用
    private void MoveStart(InputAction.CallbackContext context){
        _playerMotion.MoveMotion();
    }

    //移動コールバック用
    private void OnMove(InputAction.CallbackContext context){

        _moveDir = new Vector2Int ((int)context.ReadValue<Vector2>().x, (int)context.ReadValue<Vector2>().y);
        
        //移動キー同時押し
        if(_moveDir.x != 0 && _moveDir.y != 0){
            if(_movePreviousDir == Vector2Int.zero){
                _moveDir.y = 0;
            }else{
                _moveDir -= _movePreviousDir;
            }
        }
        RotatePlayer(_moveDir);
        _movePreviousDir = _moveDir;
    }
    //移動解除コールバック用
    private void MoveStop(InputAction.CallbackContext context){
        Debug.Log("AAA");
        _playerMotion.MoveMotionCancel();
        _moveDir = Vector2Int.zero;
        _movePreviousDir = Vector2Int.zero;
    }
}
