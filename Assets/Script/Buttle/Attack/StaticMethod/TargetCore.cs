using System.Collections.Generic;
using UnityEngine;

//対象決めクラス
public class TargetCore
{
    //周囲で近いCoreの探索（単体ヒット）
    public static List<T> GetAroundCore<T>(Vector2Int _centerPos, Vector2Int _forwardDir, int _range){
        List<T> _coreList = new List<T>();
        
        for(int i = 1;i < _range + 1; i++){
            //判定座標
            Vector2Int _judgePos;
            //中心座標からi離れた座標を１次関数的に調べる
            for(int j = 0; j < i; j++){
                _judgePos = new Vector2Int(_centerPos.x - j, _centerPos.y + i - j);
                //ステージリストの範囲外でないなら、Coreを取得
                if(!MapManager.IsOutOfReference(_judgePos)) _coreList.AddRange(GetCore<T>(_judgePos));
            }
            for(int j = 0; j < i; j++){
                _judgePos = new Vector2Int(_centerPos.x - i + j, _centerPos.y - j);
                //ステージリストの範囲外でないなら、Coreを取得
                if(!MapManager.IsOutOfReference(_judgePos)) _coreList.AddRange(GetCore<T>(_judgePos));
            }
            for(int j = 0; j < i; j++){
                _judgePos = new Vector2Int(_centerPos.x + j, _centerPos.y - i + j);
                //ステージリストの範囲外でないなら、Coreを取得
                if(!MapManager.IsOutOfReference(_judgePos)) _coreList.AddRange(GetCore<T>(_judgePos));
            }
            for(int j = 0; j < i; j++){
                _judgePos = new Vector2Int(_centerPos.x + i - j, _centerPos.y + j);
                //ステージリストの範囲外でないなら、Coreを取得
                if(!MapManager.IsOutOfReference(_judgePos)) _coreList.AddRange(GetCore<T>(_judgePos));
            }
            if(_coreList.Count > 0) break;
        }

        return _coreList;
    }

    //周囲のCoreの探索(複数ヒットかつ四角範囲)
    public static List<T> GetAroundCoreAll<T>(Vector2Int _centerPos, Vector2Int _forwardDir, int _range){
        List<T> _coreList = new List<T>();
        Vector2Int _judgePos;

        for(int i = _centerPos.x -_range;i <= _centerPos.x + _range; i++){
            for(int j = _centerPos.y -_range;j <= _centerPos.y + _range; j++){
                _judgePos = new Vector2Int(i, j);
                //ステージリストの範囲外でないなら、Coreを取得
                if(!MapManager.IsOutOfReference(_judgePos)) _coreList.AddRange(GetCore<T>(_judgePos));
            }
        }

        return _coreList;
    }

    //正面のエネミーの探索（複数ヒット）
    public static List<T> GetFowardCore<T>(Vector2Int _centerPos, Vector2Int _forwardDir, int _range){
        List<T> _coreList = new List<T>();

        for(int i = 1;i < _range + 1; i++){
            Vector2Int _judgePos = new Vector2Int(_centerPos.x + i * _forwardDir.x, _centerPos.y + i * _forwardDir.y);
            //ステージリストの範囲外でないなら、Coreを取得
            if(!MapManager.IsOutOfReference(_judgePos)) _coreList.AddRange(GetCore<T>(_judgePos));
        }

        return _coreList;
    }

    //対象マスのCoreを取得
    private static List<T> GetCore<T>(Vector2Int _judgePos){
        List<T> _coreList = new List<T>();

        //対象がエネミーの場合,エネミーを取得
        if(typeof(T) == typeof(EnemyCoreBase)){
            if(MapManager.GetMap(_judgePos)._enemyCoreList.Count > 0/* && MapManager._map[StageMove.UndoElementStageMove(_judgePos.x) , _judgePos.y].obj[0].GetType().Name == "EnemyController"*/){
                for(int k = 0 ; k < MapManager.GetMap(_judgePos)._enemyCoreList.Count ; k++){
                    var _enemyCore = (T)(object)MapManager.GetMap(_judgePos)._enemyCoreList[k];
                    _coreList.Add(_enemyCore);
                }
            }
        }//対象がクリスタルの場合,クリスタルを取得
        else if(typeof(T) == typeof(CrystalCore)){
            if(MapManager.GetMap(_judgePos)._crystalCore != null/* && MapManager._map[StageMove.UndoElementStageMove(_judgePos.x) , _judgePos.y].obj[0].GetType().Name == "EnemyController"*/){
                var _crystalCore = (T)(object)MapManager.GetMap(_judgePos)._crystalCore;
                _coreList.Add(_crystalCore);
            }
        }//対象がプレイヤーの場合,プレイヤーを取得
        else if(typeof(T) == typeof(PlayerCore)){
            if(MapManager.GetPlayerPos() == _judgePos/* && MapManager._map[StageMove.UndoElementStageMove(_judgePos.x) , _judgePos.y].obj[0].GetType().Name == "EnemyController"*/){
                var _playerCoreVar = (T)(object)MapManager.GetPlayerCore();
                _coreList.Add(_playerCoreVar);
            }
        }

        return _coreList;
    }
}
