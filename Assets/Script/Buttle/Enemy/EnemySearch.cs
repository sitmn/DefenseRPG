using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySearch : MonoBehaviour
{
    public static bool Search(Vector2Int enemyPos, float _searchDestination){
        bool _searchFlag = false;

        Vector2Int destination = enemyPos - new Vector2Int(StageMove.UndoElementStageMove(AStarMap._playerPos.x), AStarMap._playerPos.y);
        if((Mathf.Abs(destination.x) + Mathf.Abs(destination.y)) < _searchDestination){
            _searchFlag = true;
        }
        return _searchFlag;
    }

}
