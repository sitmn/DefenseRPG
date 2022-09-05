using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


public static class EnemyMoveRoute
{
    //AStar経路探索に使用するための構造体
    public struct CostMap
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

    public static CostMap[,] _costMap;

    public static void AwakeManager(){
        InitializeCostMap();
    }

    //コストマップ初期化
    public static void InitializeCostMap(){
        //コスト格納配列(初回は全値を入れる)
        _costMap = new CostMap[MapManager._map.GetLength(0),MapManager._map.GetLength(1)];
        for(int i = 0 ; i < _costMap.GetLength(0) ; i++){
            for(int j = 0 ; j < _costMap.GetLength(1); j++){
                Vector2Int _pos = new Vector2Int(i, j);
                if(MapManager.GetMap(_pos)._moveCost != 0) 
                _costMap[i,j] = new CostMap();
                _costMap[i,j]._pos = new Vector2Int(i,j);
            }
        }
    }

    //コストマップの値のみ初期化
    public static void ResetCostMap(Vector2Int _currentPos, Vector2Int _destination){
        //コスト格納配列
        for(int i = 0 ; i < _costMap.GetLength(0) ; i++){
            for(int j = 0 ; j < _costMap.GetLength(1); j++){
                Vector2Int _pos = new Vector2Int(i, j);
                if(MapManager.GetMap(_pos)._moveCost != 0) 
                _costMap[i,j]._estimateCost = CalculateEstimateCost(_costMap[i,j]._pos, _destination);
                _costMap[i,j]._status = ConstManager._costMapNoneStr;
                _costMap[i,j]._realCost = 999;
            }
        }

        //初期地点実コスト
        _costMap[_currentPos.x, _currentPos.y]._realCost = 0;
    }

    //移動経路を取得
    public static List<Vector2Int> GetTrackPos(Vector2Int _currentPos){
        List<Vector2Int> _trackPos;
        //プレイヤーが輸送クリスタルをリフト中でないなら、輸送クリスタルを追跡
        if(!MapManager.IsShippingCrystalLiftUp()){
            _trackPos = GenerateTrackRoute(_currentPos, (Vector2Int)MapManager.GetShippingCrystalPos());
        }//プレイヤーが輸送クリスタルをリフト中なら、プレイヤーを追跡
        else{
            _trackPos = GenerateTrackRoute(_currentPos, MapManager.GetPlayerPos());
        }

        return _trackPos;
    }

    //現在位置と目的地から移動経路を生成
    public static List<Vector2Int> GenerateTrackRoute(Vector2Int _currentPos, Vector2Int _destination){
        List<Vector2Int> _trackRoute = new List<Vector2Int>();
        //目的地に着いている場合、ランダムな隣のマスを1マス移動経路にする
        if(_currentPos == _destination){
            _trackRoute.Add(GetOneTrack(_destination));
        }else{
            //コストマップの初期化
            ResetCostMap(_currentPos, _destination);
            
            CostMap _minCost = _costMap[_currentPos.x, _currentPos.y];
            for(int i = 0; i < MapManager._map.GetLength(0) * MapManager._map.GetLength(1); i++){
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
    public static int CalculateEstimateCost(Vector2Int _pos, Vector2Int _destination){
        int _cost = Mathf.Abs(_pos.x - (int)_destination.x) + Mathf.Abs(_pos.y - _destination.y) + MapManager.GetMap(_pos)._moveCost - 1;
        
        return _cost;
    }

    //実コスト計算
    public static void CalculateRealCost(Vector2Int _openPos, int _cost){
        //隣接マス(十字で1マス)の実コストを計算し、オープンする
        for(int i = _openPos.x - 1 ; i < _openPos.x + 2; i++){
            for(int j = _openPos.y - 1; j < _openPos.y + 2; j++){
                Vector2Int _pos = new Vector2Int(i ,j);
                if(i >= 0 && j >= 0 && ((i == _openPos.x && j != _openPos.y)||(i != _openPos.x && j == _openPos.y))
                && (i < MapManager._map.GetLength(0) && j < MapManager._map.GetLength(1))){
                    if(_costMap[i,j]._status == ConstManager._costMapNoneStr && MapManager.GetMap(_pos)._moveCost != 0){
                        _costMap[i,j]._realCost = _cost + MapManager.GetMap(_pos)._moveCost;
                        _costMap[i,j]._status = ConstManager._costMapClosedStr;
                    }
                }
            }
        }

        //マス計算終了フラグ
        _costMap[_openPos.x,_openPos.y]._status = ConstManager._costMapOpenStr;
    }

    //実コスト+推定コストが最小なものを取得
    public static CostMap FindMinCost(){
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
                    if(_costMap[_randomList_i[i],_randomList_j[j]]._realCost != 999 && _costMap[_randomList_i[i],_randomList_j[j]]._status != ConstManager._costMapOpenStr){
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
    public static List<Vector2Int> DecideTrackRoute(CostMap _lastCost, Vector2Int _currentPos){
        var _trackRoute = new List<Vector2Int>();

        //目的地を格納
        _trackRoute.Add(_lastCost._pos);
        //実コストが0になるまで隣接するマスを格納していく
        CostMap _trackPos = _lastCost;
        List<CostMap> _keepTrackPosList;

        //目的地から現在位置まで逆算して、隣マスをListへ入れていく　☆目的地に辿り着けない場合、フリーズする
        while(_trackPos._pos != _currentPos){
            _keepTrackPosList = new List<CostMap>();
            for(int i = _trackPos._pos.x - 1; i < _trackPos._pos.x + 2; i++){
                for(int j = _trackPos._pos.y - 1; j < _trackPos._pos.y + 2; j++){
                    if(i >= 0 && j >= 0 && ((i == _trackPos._pos.x && j != _trackPos._pos.y)||(i != _trackPos._pos.x && j == _trackPos._pos.y))
                    && (i < MapManager._map.GetLength(0) && j < MapManager._map.GetLength(1))){
                        //移動経路実コストと移動コストが一致するもの（openしてきた経路）を移動経路候補として格納
                        if(_costMap[i,j]._realCost + MapManager.GetMap(_trackPos._pos)._moveCost == _trackPos._realCost){
                            _keepTrackPosList.Add(_costMap[i,j]);
                        }
                    }
                }
            }

            //移動経路の候補からランダムで移動経路を入れる
            _trackPos = _keepTrackPosList[UnityEngine.Random.Range(0, _keepTrackPosList.Count - 1)];
            _trackRoute.Insert(0,_trackPos._pos);
        }
        //現在位置の座標を取り除く
        _trackRoute.RemoveAt(0);

        return _trackRoute;
    }

    //プレイヤーの位置についたとき、ランダムに近くの１マスへ移動
    public static Vector2Int GetOneTrack(Vector2Int _destination){
        List<Vector2Int> _trackPosList = new List<Vector2Int>();
        if(_destination.x != 0) _trackPosList.Add(new Vector2Int(_destination.x - 1, _destination.y));
        if(_destination.x != MapManager.max_pos_x - 1) _trackPosList.Add(new Vector2Int(_destination.x + 1, _destination.y));
        if(_destination.y != 0) _trackPosList.Add(new Vector2Int(_destination.x, _destination.y - 1));
        if(_destination.y != MapManager.max_pos_z - 1) _trackPosList.Add(new Vector2Int(_destination.x, _destination.y + 1));

        return _trackPosList[UnityEngine.Random.Range(0, _trackPosList.Count - 1)];
    }
}
