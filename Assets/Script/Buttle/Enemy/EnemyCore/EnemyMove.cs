using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class EnemyMove : MonoBehaviour
{
    //移動経路用リスト
    public List<Vector2Int> TrackPos => _trackPos;
    private List<Vector2Int> _trackPos = new List<Vector2Int>();
    
    //移動経路探索リスト（ステージ移動後すぐに移動）
    //public bool _trackChangeFlag;
    //移動中フラグ
    public bool isMove;
    //エネミー位置情報
    private Transform _enemyTr;
    //移動スピード
    //private float _moveSpeed;
    //索敵範囲
    //private int _searchDestination;
    //移動経路探索クラス
    public EnemyMoveRoute _enemyMoveRoute;

    //コンポーネントの初期取得
    public void InitializeComponent(){
        _enemyTr = this.gameObject.GetComponent<Transform>();
        _enemyMoveRoute = this.gameObject.GetComponent<EnemyMoveRoute>();
    }
    
    public void SetTrackPos(Vector2Int _currentPos,int _searchDestination){
        if(!IsUpdateTrack(_currentPos, _searchDestination)) return;

        _trackPos = _enemyMoveRoute.GetTrackPos(_currentPos, _searchDestination);
    }

    //エネミーの回転
    public void DoRotate(){
        _enemyTr.LookAt(new Vector3(TrackPos[0].x, 0.5f, TrackPos[0].y));
    }

    //エネミーの移動
    public void Move(Vector2Int _currentPos, float _moveSpeed){
        //移動
        //ステージ最後列削除時、元最後列と今の最後列で移動中のエネミーの移動時、移動用の位置がステージ外になってしまうための例外処理
        // if(_currentPos.x < 0){
        //     DoMove(TrackPos[0] - new Vector2Int(TrackPos[0].x - 1,TrackPos[0].y), _moveSpeed);
        // }else{
            DoMove(TrackPos[0] - _currentPos, _moveSpeed);
        //}
    }

    //オブジェクトの移動
    public void DoMove(Vector2Int _moveDir, float _moveSpeed){
        //位置の移動
        _enemyTr.position += new Vector3((float)_moveDir.x,0, (float)_moveDir.y) * Time.deltaTime * _moveSpeed;
    }

    //座標中心を通過した時、座標中心へ移動
    public void UpdatePosition(){
        //位置をマス中心に
        _enemyTr.position = new Vector3(AStarMap.CastMapPos(_enemyTr.position).x , 0.5f , AStarMap.CastMapPos(_enemyTr.position).y);
        //直近の移動経路を削除し、次の移動経路に変える
        _trackPos.RemoveAt(0);
    }

    //判定座標取得
    public Vector2Int GetCurrentPosition(){
        if(StageMove.UndoElementStageMove(AStarMap.CastMapPos(_enemyTr.position).x) < 0) Debug.Log(_enemyTr.position + "CCC" + StageMove._moveRowCount);
        return new Vector2Int(StageMove.UndoElementStageMove(AStarMap.CastMapPos(_enemyTr.position).x),AStarMap.CastMapPos(_enemyTr.position).y);
    }
    // //移動座標取得
    // public Vector2Int GetCurrentPosition(){
    //     //移動用の座標の変更
    //     return new Vector2Int(StageMove.UndoElementStageMove(AStarMap.CastMapPos(_enemyTr.position).x),StageMove.UndoElementStageMove(AStarMap.CastMapPos(_enemyTr.position).y));
    // }

    //中心座標を通過したか
    public bool IsPassPosition(float _moveSpeed){
        return Mathf.Abs(_enemyTr.position.x - StageMove._moveRowCount - _trackPos[0].x + _enemyTr.position.z - _trackPos[0].y) < Time.deltaTime * _moveSpeed + 0.01f;
    }

    //移動経路を更新するか
    private bool IsUpdateTrack(Vector2Int _currentPos, int _searchDestination){
        bool _isUpdate = true;

        //目的地にいるか
        if(_trackPos.Count == 0) return _isUpdate;
        //プレイヤーを索敵しているか
        if(_enemyMoveRoute.IsSearchPlayer(_currentPos, _searchDestination)) return _isUpdate;
        //ステージ移動が原因で
        if(_trackPos[0].x < 0) return _isUpdate;

        //どの条件も満たさない場合、falseを返す
        _isUpdate = false;
        return _isUpdate;
    }
}