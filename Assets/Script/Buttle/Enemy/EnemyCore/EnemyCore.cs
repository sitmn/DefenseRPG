using UnityEngine;

public class EnemyCore : EnemyCoreBase
{
    //エネミーの行動
    public override void EnemyAction(){
        //エネミーの回転
        _enemyMove.DoRotate();
        
        if(CanAttack()){ //移動先にクリスタルがある場合攻撃
            Attack();
            //攻撃カウントが0の時（攻撃したとき、再度経路探索実施）
            if(_enemyStatus._attackCount == 0) _enemyMove.SetTrackPos(_enemyPos.Value, _enemyStatus._searchDestination);
        }else{
            //移動先にクリスタルがない場合。エネミーの移動
            _enemyMove.Move(_enemyPos.Value, _enemyStatus.GetMoveSpeed);
        }
        //マス中心を通過したら移動用の座標を変更
        if(_enemyMove.IsPassPosition(_enemyStatus.GetMoveSpeed)){
            //位置更新（移動用位置と移動経路）
            _enemyMove.UpdatePosition();
            //移動用位置取得
            SetEnemyPos();
        }else{
            //判定用位置取得
            SetJudgePos();
        }
    }

    //移動先にクリスタルがあり、攻撃可能か？
    protected override bool CanAttack(){
        return AStarMap.astarMas[_enemyMove.TrackPos[0].x,_enemyMove.TrackPos[0].y]._crystalCore != null;
    }

    //敵が水晶を攻撃
    protected override void Attack(){
        if(_enemyStatus.CountAttack()) _attack.Attack(_enemyPos.Value, new Vector2Int((int)_enemyTr.forward.x, (int)_enemyTr.forward.z), _enemyStatus._attackStatus);
    }
}
