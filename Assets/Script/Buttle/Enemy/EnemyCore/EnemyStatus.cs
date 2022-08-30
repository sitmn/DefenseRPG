using UnityEngine;
using System.Threading;

public class EnemyStatus
{
    //エネミーパラメータ
    public EnemyParam _enemyParam;
    //攻撃用ステータス
    public AttackStatus _attackStatus;
    // /***ステータス***/
    // //Hpと最大HpはEnemyCoreBaseが保持
    // //名前
    // public string _name;
    // //攻撃間隔用カウント
    // public int _attackMaxCount;
    public int _attackCount;
    // //攻撃力
    // public int _attack;
    // //攻撃範囲
    // public int _attackRange;
    // //効果倍率
    // public float _effectRate;
    // //効果時間
    // public int _effectTime;
    // //索敵範囲
    // public int _searchDestination;
    // //ステータス変動用
    // private float _moveSpeed;
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
        // /***ステータス***/
        // //Hpと最大HpはAEnemyCoreが保持
        // //エネミー名
        // this._name = _enemyParam._enemyName;
        // //攻撃間隔用カウント
        // this._attackMaxCount = _enemyParam._attackMaxCount;
        // //攻撃力
        // this._attack = _enemyParam._attack;
        // //攻撃範囲
        // this._attackRange = _enemyParam._attackRange;
        // //効果倍率
        // this._effectRate = _enemyParam._effectRate;
        // //効果時間
        // this._effectTime = _enemyParam._effectTime;
        // //索敵範囲
        // this._searchDestination = _enemyParam._searchDestination;
        // //移動速度
        // this._moveSpeed = _enemyParam._moveSpeed;
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
