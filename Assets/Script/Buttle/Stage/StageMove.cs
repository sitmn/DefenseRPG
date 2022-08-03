using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMove : MonoBehaviour
{
    //ステージ移動カウント用
    private int _stageMoveCount;
    //ステージ移動時間間隔
    [SerializeField]
    public int _stageMoveMaxCount = 100;
    //ステージ列のスライド量
    public static int _moveRowCount;

    //ステージ親オブジェクト
    private Transform _stageParentTr;
    //ステージ列プレハブ
    [SerializeField]
    private GameObject _stageRowObj;
    //ステージ列オブジェクト
    private List<GameObject> _stageRowObjList;

    void Awake(){
        _stageParentTr = this.gameObject.GetComponent<Transform>();

        //カウントの初期化
        _moveRowCount = 0;
    }

    void Start(){
        SetStageRowObject();
    }

    //ステージ列をセット
    private void SetStageRowObject(){
        _stageRowObjList = new List<GameObject>();
        for(int i = 0 ; i < AStarMap.max_pos_x_static ; i++){
            _stageRowObjList.Add(_stageParentTr.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //時間経過でステージを移動
        if(!StageMoveCount()) return;
        StageMoveExecute();
    }

    //ステージ移動タイマー用カウント
    private bool StageMoveCount(){
        bool _stageMoveCountFlag = false;
        _stageMoveCount ++;

        if(_stageMoveCount >= _stageMoveMaxCount){
            _stageMoveCount = 0;
            _stageMoveCountFlag = true;
        }

        return _stageMoveCountFlag;
    }

    //ステージ最後列削除し、ステージ最前列生成
    private void StageRowCreateAndDelete(){
        Debug.Log(_stageRowObjList.Count);
        //ステージ最後列オブジェクトを削除
        Destroy(_stageRowObjList[0]);
        //リストの参照先を１列分スライド
        for(int i = 0 ; i < _stageRowObjList.Count - 1 ; i++){
            _stageRowObjList[i] = _stageRowObjList[i + 1];
        }
        //最前列のオブジェクトを生成
        GameObject obj = Instantiate(_stageRowObj, new Vector3(AStarMap.max_pos_x_static + _moveRowCount, -0.5f , AStarMap.max_pos_z_static / 2), Quaternion.identity);
        obj.transform.parent = _stageParentTr;

        //生成したステージ列をリストへ格納
        _stageRowObjList[_stageRowObjList.Count - 1] = obj;
    }

    //ステージ移動を実行
    private void StageMoveExecute(){
        //☆前Updateメソッドの一番最初か最後に持ってきたい

        //プレイヤーがステージ最後列にいる場合、ゲームオーバー 
        
        //削除直前にはプレイヤーは要素0へ移動場所にできないようにする。（ただし、削除予定エリアから削除予定エリアには移動できる（そうしないと時間ギリギリで動けなくなってしまうため））
        //フラグがTrueの時は、nextPosに0を入れられないようにする

        //ステージ最後列にあるオブジェクトを全て削除

        //列の情報を全て1つ隣に移動させる（要素をマイナス1する）
        // 　・エネミーのTrackPos
        // 　・判定座標
        // 　　・敵
        // 　　・水晶
        // 　　・プレイヤー
        // 　・移動用座標
        // 　　・敵
        // 　　・プレイヤー
        // 　・リスト内座標（コスト用）

        //最後列の床オブジェクト削除
        //最前列のリスト生成
        //最前列の床オブジェクト生成
        StageRowCreateAndDelete();
        //最前列に新規でエネミーと黒水晶を生成

        //TrackPosにマイナスになったものがあれば、再度移動経路探索する

        //元々のリストが何列スライドしているか変数に格納（ワールド座標をリストの要素に変換するために使用）

        _moveRowCount ++;
    }

    //ワールド座標でステージを見た際の座標順を返す（一番左が0で一番右がリスト最大値）
    public static int UndoListElement(int _elementNo){
        int _undoElementNo;
        return _undoElementNo = (_elementNo - _moveRowCount >= 0)? _elementNo - _moveRowCount : _elementNo - _moveRowCount + AStarMap.max_pos_x_static;
    }
}
