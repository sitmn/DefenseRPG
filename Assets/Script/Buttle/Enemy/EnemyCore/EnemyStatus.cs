using UnityEngine;
using System.Threading;

public class EnemyStatus
{
    //エネミーパラメータ
    public EnemyParam _enemyParam;
    //攻撃用ステータス
    public AttackStatus _attackStatus;
    public int _attackCount;
    public float GetMoveSpeed
    {
        get{
            return _enemyParam._moveSpeed * (1 + _moveSpeedUp - _moveSpeedDown);
        }
    }
    //バフ用変数
    public float _moveSpeedUp;
    //デバフ用変数
    public float _moveSpeedDown;
    public CancellationTokenSource _cancelSpeedBuffToken = new CancellationTokenSource();
    public CancellationTokenSource _cancelSpeedDebuffToken = new CancellationTokenSource();

    public EnemyStatus(EnemyParam _enemyParam){
        //エネミーパラメータのセット
        this._enemyParam = _enemyParam;
        //攻撃用ステータス
        this._attackStatus = new AttackStatus(_enemyParam._attack, _enemyParam._attackRange, _enemyParam._effectRate, _enemyParam._effectTime);
        this._attackCount = 0;
    }

    //効果カウントのカウントと初期化
    public bool CountAttack(){
        _attackCount++;
        bool _attackLaunch = false;
        if(_attackCount >= _enemyParam._attackMaxCount){
            _attackCount = 0;
            _attackLaunch = true;
        }
        
        return _attackLaunch;        
    }
}
