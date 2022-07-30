using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyListController : MonoBehaviour
{
    //敵のリスト
    public static List<EnemyController> _enemiesList;

    [SerializeField]
    private Transform _enemyListTr;

    void Awake(){
        //全てのエネミーを取得
        _enemiesList = new List<EnemyController>();
        
        for(int i = 0 ; i < _enemyListTr.childCount ; i++){
            _enemiesList.Add(_enemyListTr.GetChild(i).gameObject.GetComponent<EnemyController>());
        }
    }

    void Update(){
        //全エネミーに行動指示を出す
        EnemiesActions();
    }

    //エネミーリストから削除
    public static void DeleteEnemyInList(EnemyController _enemyController){
        _enemiesList.Remove(_enemyController);
    }

    //新規エネミーをリストへ格納
    private void SetEnemyController(EnemyController enemyController){
        _enemiesList.Add(enemyController);
    }

    
    private void EnemiesActions(){
        //エネミーをそれぞれの移動経路に従って移動
        for(int i = 0 ; i < _enemiesList.Count ; i ++){
            //Debug.Log(_enemiesList[i].TrackPos[0]);
            //移動経路に水晶がある時、水晶を攻撃(黒水晶は移動経路にならないため、黒水晶は攻撃しない)
            //if(AStarMap.AstarMas[_enemiesList[i].TrackPos[0].x,_enemiesList[i].TrackPos[0].y].obj.Gettype == "CrystalController")
            //Attack()
            //else{
            //移動
            _enemiesList[i].Move(_enemiesList[i].TrackPos[0] - _enemiesList[i].EnemyPos.Value);
            //Debug.Log("next:"+_enemiesList[i].TrackPos[0]+":Intpos:"+_enemiesList[i].EnemyPos.Value + ":floatpos:" + _enemiesList[i].gameObject.GetComponent<Transform>().position);
        }
    }
}
