using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class EnemyController : MonoBehaviour, IEnemyController
{
    private Transform _enemyTr;
    public IReactiveProperty<Vector2Int> EnemyPos => _enemyPos;
    private ReactiveProperty<Vector2Int> _enemyPos = new ReactiveProperty<Vector2Int>();
    //プレイヤー追跡用リスト
    public List<Vector2Int> TrackPos => _trackPos;
    private List<Vector2Int> _trackPos = new List<Vector2Int>();
    private IAStar _astar;
    //private CharacterController _characterController;
    [SerializeField]
    private float _moveSpeed;


    [SerializeField]
    private float _searchDestination;


    void Awake(){
        _astar = this.gameObject.GetComponent<AStar>();
        _enemyTr = this.gameObject.GetComponent<Transform>();
        _enemyPos.Value = AStarMap.CastMapPos(_enemyTr.position);
    }

    void Start(){
        TrackStreamSet();
    }

    void Update(){
    }

    private void TrackStreamSet(){
        _enemyPos.Subscribe((x) => {
            //今のマップから削除
            //次のマップへ格納
            AStarMap.astarMas[_enemyPos.Value.x,_enemyPos.Value.y].obj = this;
            //経路探索
            EnemyTrackSet(x);
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


    public void Move(Vector2Int _moveDir){
        //移動
        _enemyTr.position += new Vector3((float)_moveDir.x,0, (float)_moveDir.y) * Time.deltaTime * _moveSpeed;
        _enemyPos.Value = AStarMap.CastMapPos(_enemyTr.position);
    }




}
