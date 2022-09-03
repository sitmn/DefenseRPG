using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    //移動経路用リスト
    public List<Vector2Int> TrackPos => _trackPos;
    private List<Vector2Int> _trackPos = new List<Vector2Int>();
    //エネミー位置情報
    private Transform _enemyTr;

    //コンポーネントの初期取得
    public void InitializeComponent(){
        _enemyTr = this.gameObject.GetComponent<Transform>();
    }
    
    //移動経路をセット
    public void SetTrackPos(Vector2Int _currentPos){
        if(!IsUpdateTrack(_currentPos)) return;

        _trackPos = EnemyMoveRoute.GetTrackPos(_currentPos);
    }

    //エネミーの回転
    public void DoRotate(){
        if(TrackPos.Count != 0)
        _enemyTr.LookAt(new Vector3(TrackPos[0].x, 0.5f, TrackPos[0].y));
    }

    //エネミーの移動
    public void Move(Vector2Int _currentPos, float _moveSpeed){
        //移動を実行
        DoMove(TrackPos[0] - _currentPos, _moveSpeed);
    }

    //オブジェクトの移動
    private void DoMove(Vector2Int _moveDir, float _moveSpeed){
        //位置の移動
        _enemyTr.position += new Vector3((float)_moveDir.x,0, (float)_moveDir.y) * Time.deltaTime * _moveSpeed;
    }

    //座標中心を通過した時、座標中心へ移動して位置を更新（移動経路を次のものに変更）
    public void UpdatePosition(){
        //位置をマス中心に
        _enemyTr.position = new Vector3(MapManager.CastMapPos(_enemyTr.position).x , 0.5f , MapManager.CastMapPos(_enemyTr.position).y);
        //直近の移動経路を削除し、次の移動経路に変える
        _trackPos.RemoveAt(0);
    }

    //判定座標取得
    public Vector2Int GetCurrentPosition(){
        return new Vector2Int(StageMove.UndoElementStageMove(MapManager.CastMapPos(_enemyTr.position).x),MapManager.CastMapPos(_enemyTr.position).y);
    }

    //中心座標を通過したか
    public bool IsPassPosition(float _moveSpeed){
        return Mathf.Abs(_enemyTr.position.x - StageMove._moveRowCount - _trackPos[0].x + _enemyTr.position.z - _trackPos[0].y) < Time.deltaTime * _moveSpeed + 0.01f;
    }

    //移動経路を更新するか
    private bool IsUpdateTrack(Vector2Int _currentPos){
        bool _isUpdate = true;

        //目的地にいるか
        if(_trackPos.Count == 0){
            return _isUpdate;
        }
        //プレイヤーが輸送クリスタルをリフトアップ中かつエネミーがマス中心か
        if(MapManager.IsShippingCrystalLiftUp() && _enemyTr.position.x % 1 == 0 && _enemyTr.position.z % 1 == 0){
            return _isUpdate;
        } 
        //ステージ移動が原因で
        if(_trackPos[0].x < 0){
            return _isUpdate;
        }

        //どの条件も満たさない場合、falseを返す
        _isUpdate = false;
        return _isUpdate;
    }
}