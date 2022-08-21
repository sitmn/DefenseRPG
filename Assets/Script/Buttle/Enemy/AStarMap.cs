using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarMap : MonoBehaviour
{
    //今の場所（判定用）
    public static Vector2Int _playerPos;
    public static PlayerCore _playerCore;
    public static AStarMas[,] astarMas;
    [SerializeField]
    private int max_pos_x = 15;
    [SerializeField]
    private int max_pos_z = 20;

    public static int max_pos_x_static;
    public static int max_pos_z_static;

    public void AwakeManager(){
        //ステージの最大座標をセット
        SetMaxPos();
        //クラスの初期化
        astarMas = new AStarMas[max_pos_x_static, max_pos_z_static];
        
        for(int i = 0;i < astarMas.GetLength(0); i++){
            for(int j = 0;j < astarMas.GetLength(1); j++){
                astarMas[i,j] = new AStarMas();
                astarMas[i,j]._enemyCoreList = new List<AEnemyCore>();
                astarMas[i,j]._moveCost = 1;
            }
        }
    }

    //ステージの最大座標をセット
    private void SetMaxPos(){
        max_pos_x_static = max_pos_x;
        max_pos_z_static = max_pos_z;
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
            randomPos = new Vector2Int(Random.Range(1,astarMas.GetLength(0)),Random.Range(0,astarMas.GetLength(1)));
        }
        
        return  randomPos;
    }

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
                if(!AStarMap.OutOfReferenceCheck(_judgePos)) _coreList.AddRange(GetCore<T>(_judgePos));
            }
            for(int j = 0; j < i; j++){
                _judgePos = new Vector2Int(_centerPos.x - i + j, _centerPos.y - j);
                //ステージリストの範囲外でないなら、Coreを取得
                if(!AStarMap.OutOfReferenceCheck(_judgePos)) _coreList.AddRange(GetCore<T>(_judgePos));
            }
            for(int j = 0; j < i; j++){
                _judgePos = new Vector2Int(_centerPos.x + j, _centerPos.y - i + j);
                //ステージリストの範囲外でないなら、Coreを取得
                if(!AStarMap.OutOfReferenceCheck(_judgePos)) _coreList.AddRange(GetCore<T>(_judgePos));
            }
            for(int j = 0; j < i; j++){
                _judgePos = new Vector2Int(_centerPos.x + i - j, _centerPos.y + j);
                //ステージリストの範囲外でないなら、Coreを取得
                if(!AStarMap.OutOfReferenceCheck(_judgePos)) _coreList.AddRange(GetCore<T>(_judgePos));
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
                if(!AStarMap.OutOfReferenceCheck(_judgePos)) _coreList.AddRange(GetCore<T>(_judgePos));
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
            if(!AStarMap.OutOfReferenceCheck(_judgePos)) _coreList.AddRange(GetCore<T>(_judgePos));
        }

        return _coreList;
    }

    //対象マスのCoreを取得
    private static List<T> GetCore<T>(Vector2Int _judgePos){
        List<T> _coreList = new List<T>();

        //対象がエネミーの場合,エネミーを取得
        if(typeof(T) == typeof(AEnemyCore)){
            if(AStarMap.astarMas[_judgePos.x , _judgePos.y]._enemyCoreList.Count > 0/* && AStarMap.astarMas[StageMove.UndoElementStageMove(_judgePos.x) , _judgePos.y].obj[0].GetType().Name == "EnemyController"*/){
                for(int k = 0 ; k < AStarMap.astarMas[_judgePos.x , _judgePos.y]._enemyCoreList.Count ; k++){
                    var _enemyCore = (T)(object)AStarMap.astarMas[_judgePos.x , _judgePos.y]._enemyCoreList[k];
                    _coreList.Add(_enemyCore);
                }
            }
        }//対象がクリスタルの場合,クリスタルを取得
        else if(typeof(T) == typeof(ACrystalCore)){
            if(AStarMap.astarMas[_judgePos.x , _judgePos.y]._crystalCore != null/* && AStarMap.astarMas[StageMove.UndoElementStageMove(_judgePos.x) , _judgePos.y].obj[0].GetType().Name == "EnemyController"*/){
                var _crystalCore = (T)(object)AStarMap.astarMas[_judgePos.x , _judgePos.y]._crystalCore;
                _coreList.Add(_crystalCore);
            }
        }//対象がプレイヤーの場合,プレイヤーを取得
        else if(typeof(T) == typeof(PlayerCore)){
            if(AStarMap._playerPos == _judgePos/* && AStarMap.astarMas[StageMove.UndoElementStageMove(_judgePos.x) , _judgePos.y].obj[0].GetType().Name == "EnemyController"*/){
                var _playerCoreVar = (T)(object)_playerCore;
                _coreList.Add(_playerCoreVar);
            }
        }

        return _coreList;
    }

    //ステージ範囲外でないかの判定
    public static bool OutOfReferenceCheck(Vector2Int _judgePos){
        //x軸方向の判定（ステージの移動も加味）
        bool _judgeFlag_x = _judgePos.x < 0 || _judgePos.x >= AStarMap.max_pos_x_static;
        //z軸方向の判定
        bool _judgeFlag_z = _judgePos.y < 0 || _judgePos.y >= AStarMap.max_pos_z_static;

        return _judgeFlag_x || _judgeFlag_z;
    }
}

//AStar利用用構造体
public class AStarMas{
    //移動コスト 0は行き止まり
    public int _moveCost;
    //マスに何があるか（エネミー、水晶）
    public List<AEnemyCore> _enemyCoreList;
    public ACrystalCore _crystalCore;
}
