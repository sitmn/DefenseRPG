using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarMap : MonoBehaviour
{
    //今の場所（判定用）
    public static Vector2Int _playerPos;
    public static PlayerController _playerController;
    public static AStarMas[,] astarMas;
    [SerializeField]
    private int max_pos_x = 15;
    [SerializeField]
    private int max_pos_z = 20;

    public static int max_pos_x_static;
    public static int max_pos_z_static;

    void Awake(){
        //ステージの最大座標をセット
        SetMaxPos();
        //クラスの初期化
        astarMas = new AStarMas[max_pos_x_static, max_pos_z_static];
        
        for(int i = 0;i < astarMas.GetLength(0); i++){
            for(int j = 0;j < astarMas.GetLength(1); j++){
                astarMas[i,j] = new AStarMas();
                astarMas[i,j].obj = new List<AStageObject>();
                astarMas[i,j].moveCost = 1;
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

    //ランダムに位置を取得
    public static Vector2Int GetRandomPos(){
        Vector2Int randomPos = new Vector2Int(Random.Range(0,astarMas.GetLength(0)),Random.Range(0,astarMas.GetLength(1)));
        return  randomPos;
    }

    //周囲で近いエネミーの探索
    public static List<AStageObject> AroundSearch(Vector2Int _centerPos, int _range){
        List<AStageObject> _enemyControllerList = new List<AStageObject>();

        for(int i = 1;i < _range + 1; i++){
            //中心座標からi離れた座標を１次関数的に調べる
            for(int j = 0; j < i; j++){
                if(_centerPos.x - j >= 0 && _centerPos.x - j < max_pos_x_static && _centerPos.y + i - j >= 0 && _centerPos.y + i - j < max_pos_z_static){
                    if(AStarMap.astarMas[_centerPos.x - j, _centerPos.y + i - j].obj.Count > 0 && AStarMap.astarMas[_centerPos.x - j, _centerPos.y + i - j].obj[0].GetType().Name == "EnemyController"){
                        _enemyControllerList = AStarMap.astarMas[_centerPos.x - j, _centerPos.y + i - j].obj;
                    }
                }
            }
            for(int j = 0; j < i; j++){
                if(_centerPos.x - i + j >= 0 && _centerPos.x - i + j < max_pos_x_static && _centerPos.y - j >= 0 && _centerPos.y - j < max_pos_z_static){
                    if(AStarMap.astarMas[_centerPos.x - i + j, _centerPos.y - j].obj.Count > 0 && AStarMap.astarMas[_centerPos.x - i + j, _centerPos.y - j].obj[0].GetType().Name == "EnemyController"){
                        _enemyControllerList = AStarMap.astarMas[_centerPos.x - i + j, _centerPos.y - j].obj;
                    }
                }
            }
            for(int j = 0; j < i; j++){
                if(_centerPos.x + j >= 0 && _centerPos.x + j < max_pos_x_static && _centerPos.y - i + j >= 0 && _centerPos.y - i + j < max_pos_z_static){
                    if(AStarMap.astarMas[_centerPos.x + j, _centerPos.y - i + j].obj.Count > 0 && AStarMap.astarMas[_centerPos.x + j, _centerPos.y - i + j].obj[0].GetType().Name == "EnemyController"){
                        _enemyControllerList = AStarMap.astarMas[_centerPos.x + j, _centerPos.y - i + j].obj;
                    }
                }
            }
            for(int j = 0; j < i; j++){
                if(_centerPos.x + i - j >= 0 && _centerPos.x + i - j < max_pos_x_static && _centerPos.y + j >= 0 && _centerPos.y + j < max_pos_z_static){
                    if(AStarMap.astarMas[_centerPos.x + i - j, _centerPos.y + j].obj.Count > 0 && AStarMap.astarMas[_centerPos.x + i - j, _centerPos.y + j].obj[0].GetType().Name == "EnemyController"){
                        _enemyControllerList = AStarMap.astarMas[_centerPos.x + i - j, _centerPos.y + j].obj;
                    }
                }
            }
            if(_enemyControllerList.Count > 0) break;
        }

        return _enemyControllerList;
    }

    //周囲のエネミー全員の探索
    public static List<AStageObject> AroundSearchAll(Vector2Int _centerPos, int _range){
        List<AStageObject> _enemyControllerList = new List<AStageObject>();

        for(int i = _centerPos.x -_range + 1;i < _centerPos.x + _range - 1; i++){
            for(int j = _centerPos.y -_range + 1;j < _centerPos.y + _range - 1; j++){
                if(i >= 0 && i < max_pos_x_static && j >= 0 &&  j < max_pos_z_static){
                    if(AStarMap.astarMas[i, j].obj.Count > 0 && AStarMap.astarMas[i,j].obj[0].GetType().Name == "EnemyController"){
                        _enemyControllerList.AddRange(AStarMap.astarMas[i, j].obj);
                    }
                }
            }
        }

        return _enemyControllerList;
    }
}

//AStar利用用構造体
public class AStarMas{
    //移動コスト 0は行き止まり
    public int moveCost;
    //マスに何があるか（エネミー、水晶）
    public List<AStageObject> obj;
}
