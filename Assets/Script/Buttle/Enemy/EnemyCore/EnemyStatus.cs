using UnityEngine;
using System.Threading;

public class EnemyStatus : StatusBase
{
    //エネミーパラメータ
    public EnemyParam _enemyParam;
    public int _attackCount;
    public float GetMoveSpeed
    {
        get{
            return _enemyParam._moveSpeed[_level] * (1 + _moveSpeedUp - _moveSpeedDown);
        }
    }
    //バフ用変数
    public float _moveSpeedUp;
    //デバフ用変数
    public float _moveSpeedDown;
    public CancellationTokenSource _cancelSpeedBuffToken = new CancellationTokenSource();
    public CancellationTokenSource _cancelSpeedDebuffToken = new CancellationTokenSource();

    public EnemyStatus(EnemyParam _enemyParam){
        //レベルセット
        this._level = 0;
        //エネミーパラメータのセット
        this._enemyParam = _enemyParam;
        //Debug.Log(_enemyParam._attackStatus._attack[0] + "AAA");
        //攻撃用ステータス
        this._attackStatus = _enemyParam._attackStatus;
        this._attackCount = 0;
    }

    //効果カウントのカウントと初期化
    public bool CountAttack(){
        _attackCount++;
        bool _attackLaunch = false;
        if(_attackCount >= _enemyParam._attackMaxCount[_level]){
            _attackCount = 0;
            _attackLaunch = true;
        }
        
        return _attackLaunch;        
    }
}
