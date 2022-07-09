using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IAStar
{
    //AStar処理、現在位置と目的地から移動経路算出
    List<Vector2Int> AstarMain(Vector2Int currentPos, Vector2Int destination);
}
