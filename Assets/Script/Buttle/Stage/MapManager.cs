using System.Collections.Generic;
using UnityEngine;

//座標管理クラス
public static class MapManager
{
    //プレイヤー情報
    public static Vector2Int _playerPos;
    public static Vector2Int GetPlayerPos(){ return _playerPos;}
    public static void SetPlayerPos(Vector2Int _pos){  _playerPos = _pos;}
    public static PlayerCore _playerCore;
    public static PlayerCore GetPlayerCore(){ return _playerCore;}
    public static void SetPlayerCore(PlayerCore _core){ _playerCore = _core;}
    //輸送クリスタル情報
    public static Vector2Int _shippingCrystalPos;
    public static Vector2Int GetShippingCrystalPos(){ return _shippingCrystalPos;}
    public static void SetShippingCrystalPos(Vector2Int _pos){  _shippingCrystalPos = _pos;}
    public static Map[,] _map;
    public static Map GetMap(Vector2Int _pos){
        return _map[_pos.x, _pos.y];
    }

    public static int max_pos_x;
    public static int max_pos_z;

    public static void AwakeManager(SystemParam _systemParam){
        //ステージの最大座標をセット
        SetParam(_systemParam);
        //クラスの初期化
        _map = new Map[max_pos_x, max_pos_z];
        
        for(int i = 0;i < _map.GetLength(0); i++){
            for(int j = 0;j < _map.GetLength(1); j++){
                _map[i,j] = new Map();
                _map[i,j]._enemyCoreList = new List<EnemyCoreBase>();
                _map[i,j]._moveCost = 1;
            }
        }
    }

    //ステージの最大座標をセット
    public static void SetParam(SystemParam _systemParam){
        max_pos_x = _systemParam.max_pos_x;
        max_pos_z = _systemParam.max_pos_z;
    }

    //オブジェクトのポジションをAStarMapの座標に変換
    public static Vector2Int CastMapPos(Vector3 pos){
        Vector2Int mapPos = new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z));

        return mapPos;
    }

    //ランダムに位置を取得 ステージ最後列（x軸方向）は目的地には含まない（AStarで移動経路にはなる）
    public static Vector2Int GetRandomPos(Vector2Int _pos){
        Vector2Int randomPos = _pos;
        while(randomPos == _pos){
            randomPos = new Vector2Int(Random.Range(1,_map.GetLength(0)),Random.Range(0,_map.GetLength(1)));
        }
        
        return  randomPos;
    }

    //ステージ範囲外かの判定
    public static bool IsOutOfReference(Vector2Int _judgePos){
        //x軸方向の判定（ステージの移動も加味）
        bool _judgeFlag_x = _judgePos.x < 0 || _judgePos.x >= max_pos_x;
        //z軸方向の判定
        bool _judgeFlag_z = _judgePos.y < 0 || _judgePos.y >= max_pos_z;

        return _judgeFlag_x || _judgeFlag_z;
    }

    //プレイヤーが輸送クリスタルをリフト中か
    public static bool IsShippingCrystalLiftUp(){
        bool _isShippingLift = false;
        //クリスタルをリフト中でない
        if(PlayerCore.GetLiftCrystalCore() == null) return _isShippingLift;
        //リフト中のクリスタルが輸送クリスタルか
        return PlayerCore.GetLiftCrystalCore().GetType().Name == ConstManager._shippingCrystalName;
    }
}

//AStar利用用構造体
public class Map{
    //移動コスト 0は行き止まり
    public int _moveCost;
    //マスに何があるか（エネミー、水晶）
    public List<EnemyCoreBase> _enemyCoreList;
    public CrystalCoreBase _crystalCore;
}
