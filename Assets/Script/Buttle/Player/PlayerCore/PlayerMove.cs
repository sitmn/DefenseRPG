using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private Transform _playerTr;
    private IPlayerMotion _playerMotion;
    private PlayerInput _playerInput;
    //移動方向（移動用）
    private Vector2Int _moveDir;
    //直前の移動方向（移動用）
    private Vector2Int _movePreviousDir;
    //今の場所（移動用）
    private Vector2Int _playerPos;
    //次の移動場所（移動用）
    private Vector2Int _nextPlayerPos;

    //クラスの初期化
    public void AwakeManager()
    {
        SetComponent();
    }

    //コンポーネントのセット
    private void SetComponent(){
        _playerMotion = this.gameObject.GetComponent<IPlayerMotion>();
        _playerInput = this.gameObject.GetComponent<PlayerInput>();
        _playerTr = this.gameObject.GetComponent<Transform>();

        //MapManagerの自キャラポジション格納（判定用）
        MapManager.SetPlayerPos(MapManager.CastMapPos(_playerTr.position));
        //自キャラポジション格納（移動用）
        _playerPos = MapManager.GetPlayerPos();
        //次の移動場所（移動用）
        _nextPlayerPos = _playerPos;
    }

    //InputSystemのアクションにコールバックをセット
    void OnEnable(){
        _playerInput.actions["Move"].started += OnMoveStart;
        _playerInput.actions["Move"].performed += OnMoveComplete;
        _playerInput.actions["Move"].canceled += OnMoveStop;
    }

    void OnDisable(){
        _playerInput.actions["Move"].started -= OnMoveStart;
        _playerInput.actions["Move"].performed -= OnMoveComplete;
        _playerInput.actions["Move"].canceled -= OnMoveStop;
    }

    //隣のマス中心まで移動
    public void Move(float _moveSpeed){
        //移動場所と現在地から移動方向を決定
        Vector3 player_move = new Vector3(_nextPlayerPos.x - _playerPos.x, 0, _nextPlayerPos.y - _playerPos.y);
        //位置変更
        _playerTr.position += player_move * _moveSpeed * Time.deltaTime;
        //マス中心まで移動した時、位置確定
        if(Mathf.Abs(_playerTr.position.x - _nextPlayerPos.x + _playerTr.position.z - _nextPlayerPos.y) < _moveSpeed * Time.deltaTime + 0.1f){
            _playerTr.position = new Vector3(_nextPlayerPos.x, 0 , _nextPlayerPos.y);
            _playerPos = MapManager.CastMapPos(_playerTr.position);
        }

        MapManager.SetPlayerPos(new Vector2Int(StageMove.UndoElementStageMove(MapManager.CastMapPos(_playerTr.position).x),MapManager.CastMapPos(_playerTr.position).y));
        return;
    }

    //プレイヤー向き変更
    public void RotatePlayer(){
        if(_moveDir == Vector2Int.zero) return;
        Quaternion _rotateDir = Quaternion.LookRotation(new Vector3(_moveDir.x, 0, _moveDir.y));
        _playerTr.rotation = _rotateDir;
    }

    //移動可能か
    private bool CanMove(Vector2Int _moveDir){
        bool _isMoveCheck = false;
        /*以下の条件を一つでも満たさなければ移動不可*/
        //移動後のワールド座標
        Vector2Int _movePos = MapManager.GetPlayerPos() + _moveDir;
        //ステージ範囲外判定
        if(MapManager.IsOutOfReference(_movePos)) return _isMoveCheck;
        //移動先にエネミーが存在しないか
        if(!(MapManager.GetMap(_movePos)._enemyCoreList.Count == 0)) return _isMoveCheck;
        //移動先にクリスタルが存在しないか
        if(!(MapManager.GetMap(_movePos)._crystalCore == null)) return _isMoveCheck;
        //ステージ移動直前に最後列に移動していないか
        if((MapManager.GetPlayerPos().x == 1 && _moveDir.x == -1 && StageMove._stageMoveCount > StageMove._stageMoveMaxCount * 9 / 10)) return _isMoveCheck;

        _isMoveCheck = true;
        return _isMoveCheck; 
    }

    //次の目的地を設定（移動用）
    public void SetNextMovePos(){
        if(CanMove(_moveDir)) _nextPlayerPos = _playerPos + _moveDir;
    }

    //移動中か判定（移動中は他の行動不可）
    public bool IsMove(){
        return _nextPlayerPos != _playerPos;
    }

    //移動コールバック用
    private void OnMoveStart(InputAction.CallbackContext context){
        _playerMotion.MoveMotion();
    }

    //移動コールバック用
    private void OnMoveComplete(InputAction.CallbackContext context){

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
    private void OnMoveStop(InputAction.CallbackContext context){
        _playerMotion.MoveMotionCancel();
        _moveDir = Vector2Int.zero;
        _movePreviousDir = Vector2Int.zero;
    }
}
