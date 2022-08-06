using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


public class AStar: MonoBehaviour,IAStar
{
    struct AStarCost
    {
        //座標
        public Vector2Int pos;
        //計算状態
        public string status;
        //推定コスト
        public int estimateCost;
        //実コスト
        public int realCost;
    }

    private AStarCost[,] costMap;

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

    //Astarメイン処理,現在位置と目的地から移動経路を算出
    //currentPos及びdestinationはステージ移動を考慮した座標を引数にすること
    //returnはリスト要素を基準とした移動経路
    public List<Vector2Int> AstarMain(Vector2Int currentPos, Vector2Int destination){
        List<Vector2Int> trackRoute = new List<Vector2Int>();
        //目的地に着いている場合、ランダムな隣のマスを移動経路にする
        if(currentPos == destination){
            trackRoute.Add(ArriveDestination(destination));
        }else{

            //AStarコスト格納配列
            costMap = new AStarCost[AStarMap.astarMas.GetLength(0),AStarMap.astarMas.GetLength(1)];

            for(int i = 0 ; i < costMap.GetLength(0) ; i++){
                for(int j = 0 ; j < costMap.GetLength(1); j++){
                    if(AStarMap.astarMas[i,j].moveCost != 0) 
                    costMap[i,j] = new AStarCost();
                    costMap[i,j].estimateCost = AStarEstimateCostCalculate(new Vector2Int(i,j),destination);
                    costMap[i,j].pos = new Vector2Int(i,j);
                    costMap[i,j].status = "none";
                    costMap[i,j].realCost = 999;
                }
            }

            //初期地点実コスト
            costMap[currentPos.x, currentPos.y].realCost = 0;

            AStarCost astarCostMin = costMap[currentPos.x, currentPos.y];
            for(int i = 0; i < AStarMap.astarMas.GetLength(0) * AStarMap.astarMas.GetLength(1); i++){

                //次のOpen先を導出
                AStarRealCostCalculate(astarCostMin.pos, astarCostMin.realCost);
                astarCostMin = MinSumCost();
                //目的地の隣までOpenしたらループを抜ける
                if(astarCostMin.pos == destination) break;
            }
            //経路の座標を格納
            trackRoute = TrackRoute(astarCostMin,currentPos);
        }

        return trackRoute;
    } 

    //目的地までの推定コスト計算
    private int AStarEstimateCostCalculate(Vector2Int pos, Vector2Int destination){
        int cost = Mathf.Abs(pos.x - (int)destination.x) + Mathf.Abs(pos.y - destination.y) + AStarMap.astarMas[pos.x,pos.y].moveCost - 1;
        
        return cost;
    }

    //実コスト計算
    private void AStarRealCostCalculate(Vector2Int openPos, int cost){

        //隣接マスの実コストを計算し、オープンする
        for(int i = openPos.x - 1 ; i < openPos.x + 2; i++){
            for(int j = openPos.y - 1; j < openPos.y + 2; j++){
                if(i >= 0 && j >= 0 && ((i == openPos.x && j != openPos.y)||(i != openPos.x && j == openPos.y))
                && (i < AStarMap.astarMas.GetLength(0) && j < AStarMap.astarMas.GetLength(1))){
                    if(costMap[i,j].status == "none" && AStarMap.astarMas[i,j].moveCost != 0){
                        costMap[i,j].realCost = cost + AStarMap.astarMas[i,j].moveCost;
                        costMap[i,j].status = "closed";
                        //Debug.Log(costMap[i,j].realCost + "_"+i+"x_"+j+"y"+"YYYYY");
                    }
                }
            }
        }

        //マス計算終了フラグ
        costMap[openPos.x,openPos.y].status = "open";
    }

    //実コスト+推定コストが最小なものを取得
    private AStarCost MinSumCost(){
        int min = 999;
        var astarCostMin = new AStarCost();

        //for分をランダムで回すためのリスト
        List<int> randomList_i = new List<int>();
        List<int> randomList_j = new List<int>();
        for(int i = 0; i < costMap.GetLength(0); i++){
            randomList_i.Add(i);
        }
        for(int j = 0; j < costMap.GetLength(1); j++){
            randomList_j.Add(j);
        }
        randomList_i = randomList_i.OrderBy(_ => Guid.NewGuid()).ToList();
        randomList_j = randomList_j.OrderBy(_ => Guid.NewGuid()).ToList();

        //オープンされていないマスで、最小の実コスト＋推定コストをランダムに選択
        for(int i = 0 ; i < costMap.GetLength(0) ; i ++){
                for(int j = 0 ; j < costMap.GetLength(1) ; j ++){
                    if(costMap[randomList_i[i],randomList_j[j]].realCost != 999 && costMap[randomList_i[i],randomList_j[j]].status != "open"){
                        //最小コスト更新
                        if(min > costMap[randomList_i[i],randomList_j[j]].realCost+costMap[randomList_i[i],randomList_j[j]].estimateCost){
                            min = costMap[randomList_i[i],randomList_j[j]].realCost+costMap[randomList_i[i],randomList_j[j]].estimateCost;
                            
                            astarCostMin = costMap[randomList_i[i],randomList_j[j]];
                        }
                    }
                }
            }

        return astarCostMin;
    }

    //移動ルートを目的地マスに隣接したマスから取得
    private List<Vector2Int> TrackRoute(AStarCost lastPos, Vector2Int currentPos){
        var trackRoute = new List<Vector2Int>();

        //目的地を格納
        trackRoute.Add(lastPos.pos);
        //実コストが0になるまで隣接するマスを格納していく
        AStarCost trackPos = lastPos;
        List<AStarCost> _keepTrackPosList;

        int count = 0;
        //☆目的地に辿り着けない場合、フリーズする
        while(trackPos.pos != currentPos){
            _keepTrackPosList = new List<AStarCost>();
            for(int i = trackPos.pos.x - 1; i < trackPos.pos.x + 2; i++){
                for(int j = trackPos.pos.y - 1; j < trackPos.pos.y + 2; j++){
                    if(i >= 0 && j >= 0 && ((i == trackPos.pos.x && j != trackPos.pos.y)||(i != trackPos.pos.x && j == trackPos.pos.y))
                    && (i < AStarMap.astarMas.GetLength(0) && j < AStarMap.astarMas.GetLength(1))){
                        //移動経路実コストと移動コストが一致するもの（openしてきた経路）を移動経路候補として格納
                        if(costMap[i,j].realCost + AStarMap.astarMas[trackPos.pos.x,trackPos.pos.y].moveCost == trackPos.realCost){
                            _keepTrackPosList.Add(costMap[i,j]);
                        }
                    }
                }
            }

            //移動経路の候補からランダムで移動経路を入れる
            trackPos = _keepTrackPosList[UnityEngine.Random.Range(0, _keepTrackPosList.Count - 1)];
            trackRoute.Insert(0,trackPos.pos);
            
            count ++;
            if(count > 1000){
                //Debug.Log(AStarMap.astarMas[AStarMap._playerPos.x + 1,AStarMap._playerPos.y].moveCost + "YYYY");
                //Debug.Log(costMap[AStarMap._playerPos.x + 1,AStarMap._playerPos.y].status + "Status");
                //Debug.Log(trackPos.pos + "&" + currentPos + "AAAA");
                break;
            }
            
        }
        trackRoute.RemoveAt(0);
        return trackRoute;
    }


    //プレイヤーの位置についたとき、ランダムに近くの１マスへ移動
    private Vector2Int ArriveDestination(Vector2Int _destination){
        List<Vector2Int> _trackPosList = new List<Vector2Int>();
        if(_destination.x != 0) _trackPosList.Add(new Vector2Int(_destination.x - 1, _destination.y));
        if(_destination.x != AStarMap.max_pos_x_static - 1) _trackPosList.Add(new Vector2Int(_destination.x + 1, _destination.y));
        if(_destination.y != 0) _trackPosList.Add(new Vector2Int(_destination.x, _destination.y - 1));
        if(_destination.y != AStarMap.max_pos_z_static - 1) _trackPosList.Add(new Vector2Int(_destination.x, _destination.y + 1));

        return _trackPosList[UnityEngine.Random.Range(0, _trackPosList.Count - 1)];
    }
}
