using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    private IAStar AStar;

    [SerializeField]
    private Vector2Int destination;

    [SerializeField]
    private Vector2Int currentPos;
    // Start is called before the first frame update
    void Start()
    {
        AStar = this.gameObject.GetComponent<AStar>();

        List<Vector2Int> a = AStar.AstarMain(currentPos,destination);
        foreach(var b in a){
            Debug.Log(b);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
