public class CrystalStatus
{
    //コンストラクタで新しいステータスをセット
    public CrystalStatus(CrystalParam _crystalParam){
        //攻撃用ステータス
        this._attackStatus = new AttackStatus(_crystalParam._attack, _crystalParam._attackRange, _crystalParam._effectRate, _crystalParam._effectTime);
        //クリスタル名
        this._name = _crystalParam._crystalCoreName;
        //水晶の攻撃間隔
        this._attackMaxCount = _crystalParam._attackMaxCount;
        //効果間隔用のカウント
        this._attackCount = 0;
        //攻撃力
        this._attack = _crystalParam._attack;
        //攻撃範囲
        this._attackRange = _crystalParam._attackRange;
        //特殊効果倍率
        this._effectRate = _crystalParam._effectRate;
        //特殊効果時間
        this._effectTime = _crystalParam._effectTime;
        //水晶の移動コスト(エネミーの移動経路探索用)
        this._moveCost = _crystalParam._moveCost;
        this._attackCount = 0;
    }

    //攻撃用ステータス
    public AttackStatus _attackStatus;
    //名前
    public string _name;
    //水晶の攻撃間隔
    public int _attackMaxCount;
    //効果間隔用のカウント
    public int _attackCount;
    //攻撃力
    public int _attack;
    //攻撃範囲
    public int _attackRange;
    //特殊効果倍率
    public float _effectRate;
    //特殊効果時間
    public int _effectTime;
    //水晶の移動コスト(エネミーの移動経路探索用)
    public int _moveCost;

    //効果カウントのカウントと初期化
    public bool CountAttack(){
        _attackCount++;
        bool _attackLaunch = false;
        if(_attackCount >= _attackMaxCount){
            _attackCount = 0;
            _attackLaunch = true;
        }
        
        return _attackLaunch;        
    }
}
