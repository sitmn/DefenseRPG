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
            _enemiesList[i].EnemyAction();
        }
    }
}
