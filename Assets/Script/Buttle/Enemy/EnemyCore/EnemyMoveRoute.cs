using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


public class EnemyMoveRoute: MonoBehaviour
{
    //AStar経路探索に使用するための構造体
    struct CostMap
    {
        //座標
        public Vector2Int _pos;
        //計算状態
        public string _status;
        //推定コスト
        public int _estimateCost;
        //実コスト
        public int _realCost;
    }

    private CostMap[,] _costMap;

    /*//テスト用
    [SerializeField]
    private Vector2Int CurrentPos;

    [SerializeField]
    private Vector2Int DestinationPos;
    //テスト用
    void Start(){
        List<Vector2Int> a = AstarMain(CurrentPos,DestinationPos);
        foreach(var b in a){
            //Debug.Log(b+"NNNNNNN");
        }
        
    }*/

    //コストマップ初期化
    private void InitializeCostMap(Vector2Int _currentPos, Vector2Int _destination){
        //コスト格納配列
        _costMap = new CostMap[AStarMap.astarMas.GetLength(0),AStarMap.astarMas.GetLength(1)];

        for(int i = 0 ; i < _costMap.GetLength(0) ; i++){
            for(int j = 0 ; j < _costMap.GetLength(1); j++){
                if(AStarMap.astarMas[i,j]._moveCost != 0) 
                _costMap[i,j] = new CostMap();
                _costMap[i,j]._pos = new Vector2Int(i,j);
                _costMap[i,j]._estimateCost = CalculateEstimateCost(_costMap[i,j]._pos, _destination);
                _costMap[i,j]._status = "none";
                _costMap[i,j]._realCost = 999;
            }
        }

        //初期地点実コスト
        _costMap[_currentPos.x, _currentPos.y]._realCost = 0;
    }

    //追跡先を取得
    public List<Vector2Int> GetTrackPos(Vector2Int _currentPos, int _searchDestination){
        //ステージ移動を考慮した座標を導出
        //_enemyPos = new Vector2Int(StageMove.UndoElementStageMove(_enemyPos.x), _enemyPos.y);
        //Vector2Int _playerPos = new Vector2Int(StageMove.UndoElementStageMove(AStarMap._playerPos.x),AStarMap._playerPos.y);
        
        List<Vector2Int> _trackPos;
        //索敵範囲にプレイヤーがいれば経路探索
        if(IsSearchPlayer(_currentPos, _searchDestination)){
            _trackPos = GenerateTrackRoute(_currentPos, AStarMap.GetPlayerPos());
        }else{
            //適当な位置を指定 ⇨ 軽量化するには、　　既に指定している場合、次の配列要素へ（経路探索はキャッシュを使用）
            _trackPos = GenerateTrackRoute(_currentPos, AStarMap.GetRandomPos(_currentPos));
        }

        //フラグクリア
        //_trackChangeFlag = false;
        

        return _trackPos;
    }

    //現在位置と目的地から移動経路を生成
    public List<Vector2Int> GenerateTrackRoute(Vector2Int _currentPos, Vector2Int _destination){
        List<Vector2Int> _trackRoute = new List<Vector2Int>();
        //目的地に着いている場合、ランダムな隣のマスを1マス移動経路にする
        if(_currentPos == _destination){
            _trackRoute.Add(GetOneTrack(_destination));
        }else{
            //コストマップの初期化
            InitializeCostMap(_currentPos, _destination);
            
            CostMap _minCost = _costMap[_currentPos.x, _currentPos.y];
            for(int i = 0; i < AStarMap.astarMas.GetLength(0) * AStarMap.astarMas.GetLength(1); i++){
                //次のOpen先を導出
                CalculateRealCost(_minCost._pos, _minCost._realCost);
                _minCost = FindMinCost();
                //目的地の隣までOpenしたらループを抜ける
                if(_minCost._pos == _destination) break;
            }
            //経路の座標を格納
            _trackRoute = DecideTrackRoute(_minCost,_currentPos);
        }

        return _trackRoute;
    } 

    //目的地までの推定コスト計算
    private int CalculateEstimateCost(Vector2Int _pos, Vector2Int _destination){
        int _cost = Mathf.Abs(_pos.x - (int)_destination.x) + Mathf.Abs(_pos.y - _destination.y) + AStarMap.astarMas[_pos.x,_pos.y]._moveCost - 1;
        
        return _cost;
    }

    //実コスト計算
    private void CalculateRealCost(Vector2Int _openPos, int _cost){
        //隣接マスの実コストを計算し、オープンする
        for(int i = _openPos.x - 1 ; i < _openPos.x + 2; i++){
            for(int j = _openPos.y - 1; j < _openPos.y + 2; j++){
                if(i >= 0 && j >= 0 && ((i == _openPos.x && j != _openPos.y)||(i != _openPos.x && j == _openPos.y))
                && (i < AStarMap.astarMas.GetLength(0) && j < AStarMap.astarMas.GetLength(1))){
                    if(_costMap[i,j]._status == "none" && AStarMap.astarMas[i,j]._moveCost != 0){
                        _costMap[i,j]._realCost = _cost + AStarMap.astarMas[i,j]._moveCost;
                        _costMap[i,j]._status = "closed";
                        //Debug.Log(costMap[i,j].realCost + "_"+i+"x_"+j+"y"+"YYYYY");
                    }
                }
            }
        }

        //マス計算終了フラグ
        _costMap[_openPos.x,_openPos.y]._status = "open";
    }

    //実コスト+推定コストが最小なものを取得
    private CostMap FindMinCost(){
        int _min = 999;
        CostMap _minCostMap = new CostMap();

        //for分をランダムで回すためのリスト
        List<int> _randomList_i = new List<int>();
        List<int> _randomList_j = new List<int>();
        for(int i = 0; i < _costMap.GetLength(0); i++){
            _randomList_i.Add(i);
        }
        for(int j = 0; j < _costMap.GetLength(1); j++){
            _randomList_j.Add(j);
        }
        _randomList_i = _randomList_i.OrderBy(_ => Guid.NewGuid()).ToList();
        _randomList_j = _randomList_j.OrderBy(_ => Guid.NewGuid()).ToList();

        //オープンされていないマスで、最小の実コスト＋推定コストをランダムに選択
        for(int i = 0 ; i < _costMap.GetLength(0) ; i ++){
                for(int j = 0 ; j < _costMap.GetLength(1) ; j ++){
                    if(_costMap[_randomList_i[i],_randomList_j[j]]._realCost != 999 && _costMap[_randomList_i[i],_randomList_j[j]]._status != "open"){
                        //最小コスト更新
                        if(_min > _costMap[_randomList_i[i],_randomList_j[j]]._realCost + _costMap[_randomList_i[i],_randomList_j[j]]._estimateCost){
                            _min = _costMap[_randomList_i[i],_randomList_j[j]]._realCost + _costMap[_randomList_i[i],_randomList_j[j]]._estimateCost;
                            
                            _minCostMap = _costMap[_randomList_i[i],_randomList_j[j]];
                        }
                    }
                }
            }

        return _minCostMap;
    }

    //移動ルートを目的地マスに隣接したマスから取得
    private List<Vector2Int> DecideTrackRoute(CostMap _lastCost, Vector2Int _currentPos){
        var _trackRoute = new List<Vector2Int>();

        //目的地を格納
        _trackRoute.Add(_lastCost._pos);
        //実コストが0になるまで隣接するマスを格納していく
        CostMap _trackPos = _lastCost;
        List<CostMap> _keepTrackPosList;

        int _count = 0;
        //目的地から現在位置まで逆算して、隣マスをListへ入れていく　☆目的地に辿り着けない場合、フリーズする
        while(_trackPos._pos != _currentPos){
            _keepTrackPosList = new List<CostMap>();
            for(int i = _trackPos._pos.x - 1; i < _trackPos._pos.x + 2; i++){
                for(int j = _trackPos._pos.y - 1; j < _trackPos._pos.y + 2; j++){
                    if(i >= 0 && j >= 0 && ((i == _trackPos._pos.x && j != _trackPos._pos.y)||(i != _trackPos._pos.x && j == _trackPos._pos.y))
                    && (i < AStarMap.astarMas.GetLength(0) && j < AStarMap.astarMas.GetLength(1))){
                        //移動経路実コストと移動コストが一致するもの（openしてきた経路）を移動経路候補として格納
                        if(_costMap[i,j]._realCost + AStarMap.astarMas[_trackPos._pos.x,_trackPos._pos.y]._moveCost == _trackPos._realCost){
                            _keepTrackPosList.Add(_costMap[i,j]);
                        }
                    }
                }
            }

            //移動経路の候補からランダムで移動経路を入れる
            _trackPos = _keepTrackPosList[UnityEngine.Random.Range(0, _keepTrackPosList.Count - 1)];
            _trackRoute.Insert(0,_trackPos._pos);
            
            _count ++;
            if(_count > 1000){
                //Debug.Log(AStarMap.astarMas[AStarMap._playerPos.x + 1,AStarMap._playerPos.y].moveCost + "YYYY");
                //Debug.Log(costMap[AStarMap._playerPos.x + 1,AStarMap._playerPos.y].status + "Status");
                //Debug.Log(trackPos.pos + "&" + currentPos + "AAAA");
                break;
            }
            
        }
        //現在位置の座標を取り除く
        _trackRoute.RemoveAt(0);

        return _trackRoute;
    }


    //プレイヤーの位置についたとき、ランダムに近くの１マスへ移動
    private Vector2Int GetOneTrack(Vector2Int _destination){
        List<Vector2Int> _trackPosList = new List<Vector2Int>();
        if(_destination.x != 0) _trackPosList.Add(new Vector2Int(_destination.x - 1, _destination.y));
        if(_destination.x != AStarMap.max_pos_x_static - 1) _trackPosList.Add(new Vector2Int(_destination.x + 1, _destination.y));
        if(_destination.y != 0) _trackPosList.Add(new Vector2Int(_destination.x, _destination.y - 1));
        if(_destination.y != AStarMap.max_pos_z_static - 1) _trackPosList.Add(new Vector2Int(_destination.x, _destination.y + 1));

        return _trackPosList[UnityEngine.Random.Range(0, _trackPosList.Count - 1)];
    }


    public bool IsSearchPlayer(Vector2Int _currentPos, int _searchDestination){
        bool _isSearch = false;

        Vector2Int _destination = _currentPos - new Vector2Int(StageMove.UndoElementStageMove(AStarMap.GetPlayerPos().x), AStarMap.GetPlayerPos().y);
        if((Mathf.Abs(_destination.x) + Mathf.Abs(_destination.y)) < _searchDestination){
            _isSearch = true;
        }
        return _isSearch;
    }
}
