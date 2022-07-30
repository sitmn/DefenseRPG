using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public class PlayerMove : MonoBehaviour, IPlayerMove
{
    //private CharacterController _characterController;

    private Transform _playerTr;
    private IPlayerMotion _playerMotion;
    private PlayerInput _playerInput;

    [SerializeField]
    private float _moveSpeedOrigin = 5;
    public static float _moveSpeed;
    //移動方向（移動用）
    private Vector2Int _moveDir;
    //直前の移動方向（移動用）
    private Vector2Int _movePreviousDir;
    //今の場所（移動用）
    private Vector2Int _playerPos;
    //次の移動場所（移動用）
    private Vector2Int _nextPlayerPos;
    //スピードバフ用非同期トークン
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    //スピードバフエフェクト
    private GameObject _speedBuff;

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

        _moveSpeed = _moveSpeedOrigin;

        _speedBuff = transform.GetChild(0).gameObject;

        
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
    public void Move(){
            Vector3 player_move = new Vector3(_nextPlayerPos.x - _playerPos.x, 0, _nextPlayerPos.y - _playerPos.y);

            _playerTr.position += player_move * _moveSpeed * Time.deltaTime;
            //マス中心まで移動した時、位置確定
            if(Mathf.Abs(_playerTr.position.x - _nextPlayerPos.x + _playerTr.position.z - _nextPlayerPos.y) < _moveSpeed * Time.deltaTime + 0.1f){
                _playerTr.position = new Vector3(_nextPlayerPos.x, 0 , _nextPlayerPos.y);
                _playerPos = AStarMap.CastMapPos(_playerTr.position);
            }

            AStarMap._playerPos = AStarMap.CastMapPos(_playerTr.position);
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

    //スピードを上昇させる
    public void SpeedUp(float _upRate, int _upTime){
        _moveSpeed = _moveSpeedOrigin * (1 + _upRate);
        _speedBuff.SetActive(true);

        _cancellationTokenSource.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        UndoSpeed(_upTime, _cancellationTokenSource.Token);
    }

    //スピードの上昇を元に戻す
    private async UniTask UndoSpeed(int _upTime, CancellationToken cancellationToken = default(CancellationToken)){
        await UniTask.Delay(TimeSpan.FromSeconds(_upTime), cancellationToken: _cancellationTokenSource.Token);
        _moveSpeed = _moveSpeedOrigin;
        _speedBuff.SetActive(false);
    }

    //次の目的地を設定（移動用）
    public void NextMovePos(){
        if(MoveCheck(_moveDir)) _nextPlayerPos = _playerPos + _moveDir;
        if(_moveDir != Vector2Int.zero)RotatePlayer(_moveDir);
    }

    //移動中か判定（移動中は他の行動不可）
    public bool MoveFlag(){
        return _nextPlayerPos != _playerPos;
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
        _movePreviousDir = _moveDir;
    }
    //移動解除コールバック用
    private void MoveStop(InputAction.CallbackContext context){
        _playerMotion.MoveMotionCancel();
        _moveDir = Vector2Int.zero;
        _movePreviousDir = Vector2Int.zero;
    }
}
