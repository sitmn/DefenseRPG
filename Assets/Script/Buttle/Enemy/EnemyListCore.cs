using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyListCore : MonoBehaviour
{
    //敵のリスト
    public static List<EnemyCoreBase> _enemiesList;

    [SerializeField]
    private Transform _enemyListTr;

    public void AwakeManager(EnemyParam _enemyParam){
        //全てのエネミーを取得
        _enemiesList = new List<EnemyCoreBase>();
        
        for(int i = 0 ; i < _enemyListTr.childCount ; i++){
            SetEnemyCoreInList(_enemyListTr.GetChild(i).gameObject.GetComponent<EnemyCore>(), _enemyParam);
        }
    }

    public void UpdateManager(){
        //全エネミーに行動指示を出す
        EnemiesActions();
    }

    //エネミーリストから削除
    public static void RemoveEnemyCoreInList(EnemyCoreBase _enemyCore){
        _enemiesList.Remove(_enemyCore);
    }

    //新規エネミーをリストへ格納
    public static void SetEnemyCoreInList(EnemyCore _enemyCore, EnemyParam _enemyParam){
        _enemiesList.Add(_enemyCore);
        _enemyCore.InitializeCore(_enemyParam);
    }

    
    private void EnemiesActions(){
        //全てのエネミーに行動指示
        for(int i = 0 ; i < _enemiesList.Count ; i ++){
            bool _attackFlag = false;
            

            _enemiesList[i].EnemyAction();
            // //移動経路に水晶がある時、水晶を攻撃(黒水晶は移動経路にならないため、黒水晶は攻撃しない)　TrackPosは既にステージ移動が考慮された座標のためStageMove.UndoElementStageMoveは不要
            // if(AStarMap.astarMas[_enemiesList[i].TrackPos[0].x,_enemiesList[i].TrackPos[0].y].obj.Count == 1){
            //     if(AStarMap.astarMas[_enemiesList[i].TrackPos[0].x,_enemiesList[i].TrackPos[0].y].obj[0].GetType().Name == "CrystalController"){
            //         _attackFlag = true;
            //     }
            // }
            // //攻撃
            // if(_attackFlag){
            //     _enemiesList[i].Attack(_enemiesList[i].TrackPos[0]);
            // }else{
            //     //移動
            //     //ステージ最後列削除時、元最後列と今の最後列で移動中のエネミーの移動時、移動用の位置がステージ外になってしまうための例外処理
            //     if(StageMove.UndoElementStageMove(_enemiesList[i].EnemyPos.Value.x) < 0){
            //         _enemiesList[i].Move(_enemiesList[i].TrackPos[0] - new Vector2Int(_enemiesList[i].TrackPos[0].x - 1,_enemiesList[i].TrackPos[0].y));
            //     }else{
            //         _enemiesList[i].Move(_enemiesList[i].TrackPos[0] - new Vector2Int(StageMove.UndoElementStageMove(_enemiesList[i].EnemyPos.Value.x),_enemiesList[i].EnemyPos.Value.y));
            //     }
            // }
        }
    }
}
