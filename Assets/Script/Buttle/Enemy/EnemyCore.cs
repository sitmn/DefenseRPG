using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading;

public class EnemyCore : AEnemyCore
{
    public EnemyStatus _enemyStatus;
    public EnemyMove _enemyMove;
    private Transform _enemyTr;
    //移動判断用の位置、マスの中心に入ったら場所が変わる
    //public IReactiveProperty<Vector2Int> EnemyPos => _enemyPos;
    //private ReactiveProperty<Vector2Int> _enemyPos = new ReactiveProperty<Vector2Int>();
    //どのマスにいるかの判断用位置、マス内に入ったら場所が変わる
    public IReactiveProperty<Vector2Int> JudgePos => _judgePos;
    private ReactiveProperty<Vector2Int> _judgePos = new ReactiveProperty<Vector2Int>();
    //移動判断用の位置、マスの中心に入ったら場所が変わる
    public IReactiveProperty<Vector2Int> EnemyPos => _enemyPos;
    private ReactiveProperty<Vector2Int> _enemyPos = new ReactiveProperty<Vector2Int>();
    //移動経路用リスト
    //public List<Vector2Int> TrackPos => _trackPos;
    //private List<Vector2Int> _trackPos = new List<Vector2Int>();
    //移動経路探索リスト（ステージ移動後すぐに移動）
    //public bool _trackChangeFlag;
    //private IAStar _astar;
    
    //スピード上昇回復用非同期トークン
    private CancellationTokenSource _cancellationTokenSourceBuff = new CancellationTokenSource();
    //スピード減少回復用非同期トークン
    private CancellationTokenSource _cancellationTokenSourceDebuff = new CancellationTokenSource();
    //スピードデバフエフェクト
    private GameObject _speedDebuff;
    //スピードバフエフェクト
    private GameObject _speedBuff;
    //攻撃用インターフェース
    private IAttack _attack;

    public void InitializeCore(EnemyParam _enemyParamData){
        SetComponent();
        SetParam(_enemyParamData);
        CreateJudgePosStream();
        CreateEnemyPosStream();
        CreateHPStream();
    }

    private void SetComponent(){
        _enemyTr = this.gameObject.GetComponent<Transform>();
        _enemyPos.Value = new Vector2Int(StageMove.UndoElementStageMove(AStarMap.CastMapPos(_enemyTr.position).x),AStarMap.CastMapPos(_enemyTr.position).y);
        _enemyMove = this.gameObject.GetComponent<EnemyMove>();
        _judgePos.Value = new Vector2Int(StageMove.UndoElementStageMove(AStarMap.CastMapPos(_enemyTr.position).x),AStarMap.CastMapPos(_enemyTr.position).y);
        Debug.Log(_enemyMove + "AAA");
    }

    //値の初期化
    private void SetParam(EnemyParam _enemyParamData){
        //バフオブジェクトの取得
        _speedDebuff = transform.GetChild(0).gameObject;
        _speedBuff = transform.GetChild(1).gameObject;
        //攻撃方法のセット
        _attack = new EnemyAttack();
        //エネミーステータスのセット
        _enemyStatus = new EnemyStatus(_enemyParamData);
        _maxHp = _enemyParamData._maxHp;

        Hp = _maxHp;
    }

    //Map上でクラス管理（Mapへ追加したり、取り除いたり）
    public void SetOnAStarMap(Vector2Int _pos){
        AStarMap.astarMas[_pos.x,_pos.y]._enemyCoreList.Add(this);
    }
    public void SetOffAStarMap(Vector2Int _pos){
        AStarMap.astarMas[_pos.x, _pos.y]._enemyCoreList.Remove(this);
    }

    //エネミーを削除
    public override void ObjectDestroy(){
        SetOffAStarMap(_judgePos.Value);
        EnemyListCore.RemoveEnemyCoreInList(this);
        Destroy(this.gameObject);
    }

    //Hp用ストリーム
    private void CreateHPStream(){
        _hp.Subscribe((x) => {
            if(x <= 0) ObjectDestroy();
        }).AddTo(this);
    }

    //判定座標用ストリーム生成
    private void CreateJudgePosStream(){
        _judgePos.Pairwise()
        .Subscribe((x) => {
            //今のマップへ格納
            SetOnAStarMap(x.Current);
            //前のマップから削除
            SetOffAStarMap(x.Previous);
        }).AddTo(this);
    }

    //移動経路更新用ストリーム生成
    private void CreateEnemyPosStream(){
        //１マス移動後、移動経路を再検索
        _enemyPos.Subscribe((x) => {
        //移動経路の更新
        _enemyMove.SetTrackPos(x, _enemyStatus._searchDestination);
         
        }).AddTo(this);
    }
    
    //スピードを上昇させる
    public override void SpeedUp(float _upRate, int _upTime){
        _enemyStatus._moveSpeed = _enemyStatus._moveSpeedOrigin * (1 + _upRate);
        _speedBuff.SetActive(true);

        _cancellationTokenSourceBuff.Cancel();
        _cancellationTokenSourceBuff = new CancellationTokenSource();
        UndoBuffSpeed(_upTime, _speedBuff, _cancellationTokenSourceBuff.Token);
    }

    //スピードを減少させる
    public override void SpeedDown(float _decreaseRate, int _decreaseTime){
        _enemyStatus._moveSpeed = _enemyStatus._moveSpeedOrigin * _decreaseRate;
        _speedDebuff.SetActive(true);

        _cancellationTokenSourceDebuff.Cancel();
        _cancellationTokenSourceDebuff = new CancellationTokenSource();
        UndoDebuffSpeed(_decreaseTime, _speedDebuff, _cancellationTokenSourceDebuff.Token);
    }

    //スピードを元に戻す
    private async UniTask UndoBuffSpeed(int _decreaseTime, GameObject _buffObj, CancellationToken cancellationToken = default(CancellationToken)){
        await UniTask.Delay(TimeSpan.FromSeconds(_decreaseTime), cancellationToken: _cancellationTokenSourceBuff.Token);
        _enemyStatus._moveSpeed = _enemyStatus._moveSpeedOrigin;
        _buffObj.SetActive(false);
    }
    //スピードを元に戻す
    private async UniTask UndoDebuffSpeed(int _decreaseTime, GameObject _buffObj, CancellationToken cancellationToken = default(CancellationToken)){
        await UniTask.Delay(TimeSpan.FromSeconds(_decreaseTime), cancellationToken: _cancellationTokenSourceDebuff.Token);
        _enemyStatus._moveSpeed = _enemyStatus._moveSpeedOrigin;
        _buffObj.SetActive(false);
    }

    //エネミーの行動
    public void EnemyAction(){
        //エネミーの回転
        Debug.Log(_enemyMove + "BBB");
        _enemyMove.DoRotate();
        
        if(CanAttack()){ //移動先にクリスタルがある場合攻撃
            Attack();
            //攻撃カウントが0の時（攻撃したとき、再度経路探索実施）
            if(_enemyStatus._attackCount == 0) _enemyMove.SetTrackPos(_enemyPos.Value, _enemyStatus._searchDestination);
        }else{
            //移動先にクリスタルがない場合。エネミーの移動
            _enemyMove.Move(_enemyPos.Value, _enemyStatus._moveSpeed);
        }
        //マス中心を通過したら移動用の座標を変更
        if(_enemyMove.IsPassPosition(_enemyStatus._moveSpeed)){
            //位置更新（移動用位置と移動経路）
            _enemyMove.UpdatePosition();
            //移動用位置取得
            SetEnemyPos();
        }else{
            //判定用位置取得
            SetJudgePos();
        }
    // }
    }

    //移動先にクリスタルがあり、攻撃可能か？
    public bool CanAttack(){
        return AStarMap.astarMas[_enemyMove.TrackPos[0].x,_enemyMove.TrackPos[0].y]._crystalCore != null;
    }

    //敵が水晶を攻撃,_attackPosは既にステージ列移動が考慮された座標のためStageMove.UndoElementStageMoveは不要
    private void Attack(){
        if(_enemyStatus.CountAttack()) _attack.DoAttack(_enemyPos.Value, new Vector2Int((int)_enemyTr.forward.x, (int)_enemyTr.forward.z), _enemyStatus._attackStatus);
    }

    //移動用座標をセット
    public void SetEnemyPos(){
        _enemyPos.Value = _enemyMove.GetCurrentPosition();
    }
    //判定用座標をセット
    public void SetJudgePos(){
        _judgePos.Value = _enemyMove.GetCurrentPosition();
    }

    void OnDestroy(){
        _cancellationTokenSourceBuff.Cancel();
        _cancellationTokenSourceDebuff.Cancel();
    }
}
