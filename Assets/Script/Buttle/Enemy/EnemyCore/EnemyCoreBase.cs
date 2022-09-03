using UnityEngine;
using UniRx;

public abstract class EnemyCoreBase:MonoBehaviour
{
    private ReactiveProperty<int> _hp;
    public int Hp{
        get{
            return _hp.Value;
        }
        set{
            if(value > _enemyStatus._enemyParam._maxHp[_enemyStatus._level]){
                _hp.Value = _enemyStatus._enemyParam._maxHp[_enemyStatus._level];
            }else{
                _hp.Value = value;
            }
        }
    }

    [System.NonSerialized]
    public EnemyStatus _enemyStatus;
    //スピードデバフエフェクト
    [System.NonSerialized]
    public GameObject _speedDebuff;
    //スピードバフエフェクト
    [System.NonSerialized]
    public GameObject _speedBuff;
    //移動用クラス
    [System.NonSerialized]
    public EnemyMove _enemyMove;
    protected Transform _enemyTr;
    //どのマスにいるかの判断用位置、マス内に入ったら場所が変わる
    public IReactiveProperty<Vector2Int> JudgePos => _judgePos;
    protected ReactiveProperty<Vector2Int> _judgePos = new ReactiveProperty<Vector2Int>();
    //移動判断用の位置、マスの中心に入ったら場所が変わる
    public IReactiveProperty<Vector2Int> EnemyPos => _enemyPos;
    protected ReactiveProperty<Vector2Int> _enemyPos = new ReactiveProperty<Vector2Int>();
    //攻撃用インターフェース
    protected AttackBase _attack;

    //クラスの初期化
    public void InitializeCore(EnemyParam _enemyParamData){
        //コンポーネントの初期化
        SetComponent();
        //初期パラメータセット
        SetParam(_enemyParamData);
        //enemyMoveクラスのコンポーネントの初期化
        _enemyMove.InitializeComponent();
        //ストリーム作成
        CreateJudgePosStream();
        CreateEnemyPosStream();
        CreateHPStream();
        //移動経路をセット
        _enemyMove.SetTrackPos(_enemyPos.Value);
    }

    //コンポーネントセット
    private void SetComponent(){
        _enemyTr = this.gameObject.GetComponent<Transform>();
        _enemyPos.Value = new Vector2Int(StageMove.UndoElementStageMove(MapManager.CastMapPos(_enemyTr.position).x),MapManager.CastMapPos(_enemyTr.position).y);
        _enemyMove = this.gameObject.GetComponent<EnemyMove>();
        _judgePos.Value = new Vector2Int(StageMove.UndoElementStageMove(MapManager.CastMapPos(_enemyTr.position).x),MapManager.CastMapPos(_enemyTr.position).y);
    }

    //値の初期化
    private void SetParam(EnemyParam _enemyParamData){
        //エネミーステータスのセット
        _enemyStatus = new EnemyStatus(_enemyParamData);
        //バフオブジェクトの取得
        _speedDebuff = transform.GetChild(0).gameObject;
        _speedBuff = transform.GetChild(1).gameObject;
        //攻撃方法のセット
        _attack = new EnemyAttack();
        //HPをセット
        _hp = new ReactiveProperty<int>(_enemyParamData._maxHp[_enemyStatus._level]);
    }

    //Hp用ストリーム
    private void CreateHPStream(){
        _hp.Subscribe((x) => {
            if(x <= 0) {
                ObjectDestroy();}
        }).AddTo(this);
    }

    //判定座標用ストリーム生成
    private void CreateJudgePosStream(){
        _judgePos.Pairwise()
        .Subscribe((x) => {
            if(!MapManager.IsOutOfReference(x.Current)){
                //今のマップへ格納
                SetOnMap(x.Current);
                //前のマップから削除
                SetOffMap(x.Previous);
            }
        }).AddTo(this);
    }

    //移動経路更新用ストリーム生成
    private void CreateEnemyPosStream(){
        //１マス移動後、移動経路を再検索
        _enemyPos.Subscribe((x) => {
        //移動経路の更新
        if(!MapManager.IsOutOfReference(x)) _enemyMove.SetTrackPos(x);
        
        }).AddTo(this);
    }

    //Map上にエネミー情報作成
    public void SetOnMap(Vector2Int _pos){
        MapManager.GetMap(_pos)._enemyCoreList.Add(this);
    }
    //Map上からエネミー情報削除
    public void SetOffMap(Vector2Int _pos){
        MapManager.GetMap(_pos)._enemyCoreList.Remove(this);
    }

    //エネミーを削除
    public void ObjectDestroy(){
        SetOffMap(_judgePos.Value);
        EnemyListCore.RemoveEnemyCoreInList(this);
        Destroy(this.gameObject);
    }

    //移動用座標をセット(位置がマスの中心の時に呼び出すこと)
    public void SetEnemyPos(){
        _enemyPos.Value = _enemyMove.GetCurrentPosition();
    }
    //判定用座標をセット
    public void SetJudgePos(){
        _judgePos.Value = _enemyMove.GetCurrentPosition();
    }

    //削除時、バフ用の非同期トークン削除
    void OnDestroy(){
        _enemyStatus._cancelSpeedBuffToken.Cancel();
        _enemyStatus._cancelSpeedDebuffToken.Cancel();
    }

    //エネミーの行動
    public abstract void EnemyAction();
    //攻撃判定用
    protected abstract bool CanAttack();
    //攻撃クラスのメソッド呼び出し
    protected abstract void Attack();
}
