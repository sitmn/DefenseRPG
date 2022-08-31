public class CrystalStatus : StatusBase
{
    //コンストラクタで新しいステータスをセット
    public CrystalStatus(CrystalParam _crystalParam){
        this._level = 0;
        //攻撃用ステータス
        this._attackStatus = _crystalParam._attackStatus;
        //クリスタルパラメータのセット
        this._crystalParam = _crystalParam;
        //効果間隔用のカウント
        this._attackCount = 0;
    }

    //クリスタルステータス
    public CrystalParam _crystalParam;
    //効果間隔用のカウント
    public int _attackCount;

    //クリスタルランクのセット
    public void SetCrystalRank(){
        _level ++;
    }

    //効果カウントのカウントと初期化
    public bool CountAttack(){
        _attackCount++;
        bool _attackLaunch = false;
        if(_attackCount >= _crystalParam._attackMaxCount[_level]){
            _attackCount = 0;
            _attackLaunch = true;
        }
        
        return _attackLaunch;        
    }
}
