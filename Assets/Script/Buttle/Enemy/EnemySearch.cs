using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySearch : MonoBehaviour
{
    public static bool Search(Vector3 enemyPos, float _searchDestination){
        bool _searchFlag = false;

        Vector2Int destination = new Vector2Int((int)enemyPos.x, (int)enemyPos.z) - AStarMap._playerPos;
        if((Mathf.Abs(destination.x) + Mathf.Abs(destination.y)) < _searchDestination){
            _searchFlag = true;
        }
        return _searchFlag;
    }

}
