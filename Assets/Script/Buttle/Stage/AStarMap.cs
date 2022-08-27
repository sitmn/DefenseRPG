using System.Collections.Generic;
using UnityEngine;

public class AStarMap : MonoBehaviour
{
    //今の場所（判定用）
    public static Vector2Int _playerPos;
    public static Vector2Int GetPlayerPos(){ return _playerPos;}
    public static void SetPlayerPos(Vector2Int _pos){  _playerPos = _pos;}
    public static PlayerCore _playerCore;
    public static PlayerCore GetPlayerCore(){ return _playerCore;}
    public static void SetPlayerCore(PlayerCore _core){ _playerCore = _core;}
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
                astarMas[i,j]._enemyCoreList = new List<EnemyCoreBase>();
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

    //ステージ範囲外かの判定
    public static bool IsOutOfReference(Vector2Int _judgePos){
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
    public List<EnemyCoreBase> _enemyCoreList;
    public CrystalCore _crystalCore;
}
