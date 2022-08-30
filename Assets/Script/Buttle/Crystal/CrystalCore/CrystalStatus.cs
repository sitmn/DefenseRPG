public class CrystalStatus
{
    //コンストラクタで新しいステータスをセット
    public CrystalStatus(CrystalParam _crystalParam){
        this._crystalRank = 0;
        //攻撃用ステータス
        this._attackStatus = new AttackStatus(_crystalParam._attack[_crystalRank], _crystalParam._attackRange[_crystalRank], _crystalParam._effectRate[_crystalRank], _crystalParam._effectTime[_crystalRank]);
        //クリスタルパラメータのセット
        this._crystalParam = _crystalParam;
        //効果間隔用のカウント
        this._attackCount = 0;
    }

    //クリスタルステータスデータ
    public CrystalParam _crystalParam;
    //攻撃用ステータス
    public AttackStatus _attackStatus;
    //効果間隔用のカウント
    public int _attackCount;
    //クリスタルのランク
    public int _crystalRank;

    //クリスタルランクのセット
    public void SetCrystalRank(){
        _crystalRank ++;
    }

    //効果カウントのカウントと初期化
    public bool CountAttack(){
        _attackCount++;
        bool _attackLaunch = false;
        if(_attackCount >= _crystalParam._attackMaxCount[_crystalRank]){
            _attackCount = 0;
            _attackLaunch = true;
        }
        
        return _attackLaunch;        
    }
}
