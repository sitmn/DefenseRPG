using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarMap : MonoBehaviour
{
    public static Vector2Int _playerPos;
    public static AStarMas[,] astarMas;
    [SerializeField]
    private int max_pos_x = 15;
    [SerializeField]
    private int max_pos_z = 20;

    [SerializeField]
    private int enemy_not_move_y = 10;
    [SerializeField]
    private int enemy_move_x = 5;

    void Awake(){
        //クラスの初期化
        astarMas = new AStarMas[max_pos_x, max_pos_z];
        
        for(int i = 0;i < astarMas.GetLength(0); i++){
            for(int j = 0;j < astarMas.GetLength(1); j++){
                astarMas[i,j] = new AStarMas();
                if(i != enemy_move_x && j == enemy_not_move_y){
                    astarMas[i,j].moveCost = 0;
                }else{
                    astarMas[i,j].moveCost = 1;
                }
            }
        }
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

    /*AStarMapの座標にオブジェクトを格納
    public static void SetAStarMap(StageObject stageObject, Vector2Int setPos){
        astarMas[setPos.x,setPos.y].obj = stageObject;
    }*/
}

//AStar利用用構造体
public class AStarMas{
    //移動コスト
    public int moveCost;
    //マスに何があるか（プレイヤー、エネミー、水晶）
    public EnemyController obj;
}
