using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading;

public class EnemyController : AStageObject
{
    /***ステータス***/
    //Hpと最大HpはIStageObjectが保持
    //攻撃間隔用カウント
    private int _attackMaxCount;
    private int _attackCount;
    //攻撃力
    private int _attack;
    //索敵範囲
    private float _searchDestination;
    //移動速度
    private float _moveSpeedOrigin;
    //ステータス変動用
    private float _moveSpeed;
    public float MoveSpeed
    {
        get{
            return _moveSpeed;
        }
        set{
            value = (value < 0)? 0 : value;
            _moveSpeed = value;
        }
    }
    private EnemyParamAsset _enemyParamData;
    

    private Transform _enemyTr;
    //移動判断用の位置、マスの中心に入ったら場所が変わる
    public IReactiveProperty<Vector2Int> EnemyPos => _enemyPos;
    private ReactiveProperty<Vector2Int> _enemyPos = new ReactiveProperty<Vector2Int>();
    //どのマスにいるかの判断用位置、マス内に入ったら場所が変わる
    public IReactiveProperty<Vector2Int> JudgePos => _judgePos;
    private ReactiveProperty<Vector2Int> _judgePos = new ReactiveProperty<Vector2Int>();
    //移動経路用リスト
    public List<Vector2Int> TrackPos => _trackPos;
    private List<Vector2Int> _trackPos = new List<Vector2Int>();
    //移動経路探索リスト（ステージ移動後すぐに移動）
    public bool _trackChangeFlag;
    private IAStar _astar;
    
    //スピード上昇回復用非同期トークン
    private CancellationTokenSource _cancellationTokenSourceBuff = new CancellationTokenSource();
    //スピード減少回復用非同期トークン
    private CancellationTokenSource _cancellationTokenSourceDebuff = new CancellationTokenSource();
    //スピードデバフエフェクト
    private GameObject _speedDebuff;
    //スピードバフエフェクト
    private GameObject _speedBuff;

    void Awake(){
        _enemyParamData = Resources.Load("Data/EnemyParamData") as EnemyParamAsset;

        _astar = this.gameObject.GetComponent<AStar>();
        _enemyTr = this.gameObject.GetComponent<Transform>();
        _enemyPos.Value = AStarMap.CastMapPos(_enemyTr.position);
        _judgePos.Value = AStarMap.CastMapPos(_enemyTr.position);
        _speedDebuff = transform.GetChild(0).gameObject;
        _speedBuff = transform.GetChild(1).gameObject;
    }

    void Start(){
        TrackStreamSet();
        JudgeStreamSet();
        SetParam();
        SetHPStream();
    }


    //値の初期化
    private void SetParam(){
        _attackMaxCount = _enemyParamData.EnemyParamList[0]._attackMaxCount;
        _attack = _enemyParamData.EnemyParamList[0]._attack;
        _searchDestination = _enemyParamData.EnemyParamList[0]._searchDestination;
        _maxHp = _enemyParamData.EnemyParamList[0]._maxHp;
        _moveSpeedOrigin = _enemyParamData.EnemyParamList[0]._moveSpeed;

        Hp = _maxHp;
        _moveSpeed = _moveSpeedOrigin;

        _trackChangeFlag = false;
    }

    private void SetHPStream(){
        _hp.Subscribe((x) => {
            if(x <= 0) ObjectDestroy();
        }).AddTo(this);
    }

    public void SetOnAStarMap(Vector2Int _pos){
        AStarMap.astarMas[StageMove.UndoElementStageMove(_pos.x),_pos.y].obj.Add(this);
    }
    public void SetOffAStarMap(Vector2Int _pos){
        AStarMap.astarMas[StageMove.UndoElementStageMove(_pos.x), _pos.y].obj.Remove(this);
    }

    //エネミーを削除
    public override void ObjectDestroy(){
        SetOffAStarMap(_judgePos.Value);
        EnemyListController.DeleteEnemyInList(this);
        Destroy(this.gameObject);
    }

    //経路探索用ストリーム
    private void TrackStreamSet(){
        //１マス移動後、移動経路を再検索
        _enemyPos.Subscribe((x) => {
            //目的地に着いたら,またはステージ移動時,またはプレイヤー発見時に経路探索
            if(_trackPos.Count == 0 || _trackChangeFlag || EnemySearch.Search(_enemyPos.Value, _searchDestination)) EnemyTrackSet(x);
        }).AddTo(this);
    }
    //判定座標用ストリーム
    private void JudgeStreamSet(){
        _judgePos.Pairwise()
        .Subscribe((x) => {
            //今のマップへ格納
            SetOnAStarMap(x.Current);
            //前のマップから削除
            SetOffAStarMap(x.Previous);
        }).AddTo(this);
    }
 
    //追跡先をセット
    public void EnemyTrackSet(Vector2Int _enemyPos){
        //ステージ移動を考慮した座標を導出
        _enemyPos = new Vector2Int(StageMove.UndoElementStageMove(_enemyPos.x), _enemyPos.y);
        Vector2Int _playerPos = new Vector2Int(StageMove.UndoElementStageMove(AStarMap._playerPos.x),AStarMap._playerPos.y);
        //索敵範囲にプレイヤーがいれば経路探索
        if(EnemySearch.Search(_enemyPos, _searchDestination)){
            _trackPos = _astar.AstarMain(_enemyPos, _playerPos);
        }else{
            //適当な位置を指定 ⇨ 軽量化するには、　　既に指定している場合、次の配列要素へ（経路探索はキャッシュを使用）
            _trackPos = _astar.AstarMain(_enemyPos, AStarMap.GetRandomPos(_enemyPos));
        }

        

        //フラグクリア
        _trackChangeFlag = false;
    }

    //スピードを上昇させる
    public override void SpeedUp(float _upRate, int _upTime){
        _moveSpeed = _moveSpeedOrigin * (1 + _upRate);
        _speedBuff.SetActive(true);

        _cancellationTokenSourceBuff.Cancel();
        _cancellationTokenSourceBuff = new CancellationTokenSource();
        UndoBuffSpeed(_upTime, _speedBuff, _cancellationTokenSourceBuff.Token);
    }

    //スピードを減少させる
    public override void SpeedDown(float _decreaseRate, int _decreaseTime){
        _moveSpeed = _moveSpeedOrigin * _decreaseRate;
        _speedDebuff.SetActive(true);

        _cancellationTokenSourceDebuff.Cancel();
        _cancellationTokenSourceDebuff = new CancellationTokenSource();
        UndoDebuffSpeed(_decreaseTime, _speedDebuff, _cancellationTokenSourceDebuff.Token);
    }

    //スピードを元に戻す
    private async UniTask UndoBuffSpeed(int _decreaseTime, GameObject _buffObj, CancellationToken cancellationToken = default(CancellationToken)){
        await UniTask.Delay(TimeSpan.FromSeconds(_decreaseTime), cancellationToken: _cancellationTokenSourceBuff.Token);
        _moveSpeed = _moveSpeedOrigin;
        _buffObj.SetActive(false);
    }
    //スピードを元に戻す
    private async UniTask UndoDebuffSpeed(int _decreaseTime, GameObject _buffObj, CancellationToken cancellationToken = default(CancellationToken)){
        await UniTask.Delay(TimeSpan.FromSeconds(_decreaseTime), cancellationToken: _cancellationTokenSourceDebuff.Token);
        _moveSpeed = _moveSpeedOrigin;
        _buffObj.SetActive(false);
    }

    public void Move(Vector2Int _moveDir){
        //移動
        _enemyTr.position += new Vector3((float)_moveDir.x,0, (float)_moveDir.y) * Time.deltaTime * _moveSpeed;

        //AStarのマス中心を通過したら座標を変更（移動用） _enemyTr.position.xにはステージ列移動が考慮されていないのでStageMoveのUndoElementStageMoveが必要（型が異なるため、__moveRowCountを直接引く）
        if(Mathf.Abs(_enemyTr.position.x - StageMove._moveRowCount - _trackPos[0].x + _enemyTr.position.z - _trackPos[0].y) < Time.deltaTime * _moveSpeed + 0.01f){
            _trackPos.RemoveAt(0);
            _enemyPos.Value = AStarMap.CastMapPos(_enemyTr.position);
            _enemyTr.position = new Vector3(_enemyPos.Value.x , 0.5f , _enemyPos.Value.y);
        }
        //AStarのマス内に踏み入れたら座標を変更（当たり判定用）
        _judgePos.Value = AStarMap.CastMapPos(_enemyTr.position);
    }

    //敵が水晶を攻撃,_attackPosは既にステージ列移動が考慮された座標のためStageMove.UndoElementStageMoveは不要
    public void Attack(Vector2Int _attackPos){
        if(AttackCount()) AStarMap.astarMas[_attackPos.x, _attackPos.y].obj[0].Hp -= _attack;
    }

    //攻撃間隔用のカウント
    private bool AttackCount(){
        bool _attackFlag = false;
        _attackCount ++;

        //_attackMaxCount分経過したら攻撃
        if(_attackCount >= _attackMaxCount){
            _attackCount = 0;
            _attackFlag = true;

            //攻撃時、経路探索し直し
            EnemyTrackSet(_enemyPos.Value);
        }

        return _attackFlag;
    }

    void OnDestroy(){
        _cancellationTokenSourceBuff.Cancel();
        _cancellationTokenSourceDebuff.Cancel();
    }
}
