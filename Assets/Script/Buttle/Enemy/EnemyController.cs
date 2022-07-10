using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, StageObject
{
    private Transform _enemyTr;
    //プレイヤー追跡用リスト
    private List<Vector2Int> _trackPos;
    private IAStar _astar;
    //private CharacterController _characterController;
    [SerializeField]
    private float _moveSpeed;

    private bool _searchFlag;

    [SerializeField]
    private float _searchDestination;


    void Awake(){
        _trackPos = new List<Vector2Int>();

        _astar = this.gameObject.GetComponent<AStar>();
        _enemyTr = this.gameObject.GetComponent<Transform>();
        //_characterController = this.gameObject.GetComponent<CharacterController>();

        _searchFlag = false;
    }

    void Update(){
        if(_searchFlag) EnemyTrackSet();
        _searchFlag = EnemySearch.Search(_enemyTr.position, _searchDestination);
    }
 
    public void EnemyTrackSet(){
        //次の移動先を決定
        Vector2Int _pos = AStarMap.CastMapPos(_enemyTr.position);
        
        AStarMap.astarMas[_pos.x,_pos.y].obj = this;

        _trackPos = _astar.AstarMain(_pos, AStarMap._playerPos);

        //移動
        EnemyMove(_trackPos[0] - _pos);

        

    }


    private void EnemyMove(Vector2Int _moveDir){
        _enemyTr.position += new Vector3((float)_moveDir.x,0, (float)_moveDir.y) * Time.deltaTime * _moveSpeed;
    }




}
