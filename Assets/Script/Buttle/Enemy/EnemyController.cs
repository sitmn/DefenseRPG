using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading;

public class EnemyController : IStageObject
{
    private Transform _enemyTr;
    //移動判断用の位置、マスの中心に入ったら場所が変わる
    public IReactiveProperty<Vector2Int> EnemyPos => _enemyPos;
    private ReactiveProperty<Vector2Int> _enemyPos = new ReactiveProperty<Vector2Int>();
    //どのマスにいるかの判断用位置、マス内に入ったら場所が変わる
    public IReactiveProperty<Vector2Int> JudgePos => _judgePos;
    private ReactiveProperty<Vector2Int> _judgePos = new ReactiveProperty<Vector2Int>();
    //プレイヤー追跡用リスト
    public List<Vector2Int> TrackPos => _trackPos;
    private List<Vector2Int> _trackPos = new List<Vector2Int>();
    private IAStar _astar;
    //private CharacterController _characterController;
    [SerializeField]
    private float _moveSpeedOrigin;
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
    //スピード減少回復用非同期トークン
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    [SerializeField]
    private float _searchDestination;

    [SerializeField]
    private int _hpMax;

    void Awake(){
        _astar = this.gameObject.GetComponent<AStar>();
        _enemyTr = this.gameObject.GetComponent<Transform>();
        _enemyPos.Value = AStarMap.CastMapPos(_enemyTr.position);
        _judgePos.Value = AStarMap.CastMapPos(_enemyTr.position);
    }

    void Start(){
        TrackStreamSet();
        JudgeStreamSet();
        SetParam();
        SetHPStream();
    }

    //値の初期化
    private void SetParam(){
        _hp.Value = _hpMax;
        _moveSpeed = _moveSpeedOrigin;
    }

    private void SetHPStream(){
        _hp.Subscribe((x) => {
            Debug.Log(x);
            if(x <= 0) EnemyDeath();
        }).AddTo(this);
    }
    //エネミーを削除
    private void EnemyDeath(){
        EnemyListController.DeleteEnemyInList(this);
        AStarMap.astarMas[_judgePos.Value.x, _judgePos.Value.y].obj.Remove(this);
        Destroy(this.gameObject);
    }

    //経路探索用ストリーム
    private void TrackStreamSet(){
        //１マス移動後、移動経路を再検索　　　☆今の状態だと、0.5pos毎に移動経路探索になっている
        _enemyPos.Subscribe((x) => {
            //経路探索
            EnemyTrackSet(x);
        }).AddTo(this);
    }
    //判定座標用ストリーム
    private void JudgeStreamSet(){
        _judgePos.Pairwise()
        .Subscribe((x) => {
            //今のマップへ格納
            AStarMap.astarMas[x.Current.x,x.Current.y].obj.Add(this);
            //前のマップから削除
            AStarMap.astarMas[x.Previous.x,x.Previous.y].obj.Remove(this);
        }).AddTo(this);
    }
 
    //追跡先をセット
    public void EnemyTrackSet(Vector2Int _enemyPos){
        //索敵範囲にプレイヤーがいれば経路探索
        if(EnemySearch.Search(_enemyPos, _searchDestination)){
            _trackPos = _astar.AstarMain(_enemyPos, AStarMap._playerPos);
        }else{
            //適当な位置を指定 ⇨ 軽量化するには、　　既に指定している場合、次の配列要素へ（経路探索はキャッシュを使用）
            _trackPos = _astar.AstarMain(_enemyPos, AStarMap.GetRandomPos());
        }
    }

    //スピードを減少させる
    public override void SpeedDown(float _decreaseRate, int _decreaseTime){
        _moveSpeed = _moveSpeedOrigin * _decreaseRate;

        _cancellationTokenSource.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        UndoSpeed(_decreaseTime, _cancellationTokenSource.Token);
    }

    //スピードの減少を元に戻す
    private async UniTask UndoSpeed(int _decreaseTime, CancellationToken cancellationToken = default(CancellationToken)){
        
        Debug.Log("AAA");
        await UniTask.Delay(TimeSpan.FromSeconds(_decreaseTime), cancellationToken: _cancellationTokenSource.Token);
        Debug.Log("BBB");
        _moveSpeed = _moveSpeedOrigin;
    }

    public void Move(Vector2Int _moveDir){
        //移動
        _enemyTr.position += new Vector3((float)_moveDir.x,0, (float)_moveDir.y) * Time.deltaTime * _moveSpeed;

        //AStarのマス中心を通過したら座標を変更（移動用）
        if(Mathf.Abs(_enemyTr.position.x - _trackPos[0].x + _enemyTr.position.z - _trackPos[0].y)  < Time.deltaTime * _moveSpeed + 0.01f){
            _enemyPos.Value = AStarMap.CastMapPos(_enemyTr.position);
            _enemyTr.position = new Vector3(_enemyPos.Value.x , 0.5f , _enemyPos.Value.y);
        }
        //AStarのマス内に踏み入れたら座標を変更（当たり判定用）
        _judgePos.Value = AStarMap.CastMapPos(_enemyTr.position);
    }
}
