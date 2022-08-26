using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform _cameraTr;
    //前フレームのステージ移動カウント
    private int _beforeStageMoveCount;

    // Start is called before the first frame update
    void Start()
    {
        _cameraTr = this.gameObject.GetComponent<Transform>();

        _beforeStageMoveCount = StageMove._stageMoveCount;
    }

    // Update is called once per frame
    public void UpdateManager()
    {
        //前フレームとのカウントの差をとり、その差分分移動。1回のステージ移動毎に、1マスカメラも合わせて移動
        if(_beforeStageMoveCount < StageMove._stageMoveCount){
            float _move = ((float)(StageMove._stageMoveCount - _beforeStageMoveCount)) / (float)StageMove._stageMoveMaxCountStatic;
            _cameraTr.position = new Vector3(_cameraTr.position.x + _move , _cameraTr.position.y, _cameraTr.position.z);
        }
        _beforeStageMoveCount = StageMove._stageMoveCount;
    }
}
