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

    public static int _moveRowCount;

    void Awake(){
        //カウントの初期化
        _moveRowCount = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //時間経過でステージを移動
        if(StageMoveCount()) return;
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

    //ステージ移動を実行
    private void StageMoveExecute(){
        //☆前Updateメソッドの一番最初か最後に持ってきたい

        //プレイヤーがステージ最後列にいる場合、ゲームオーバー 
        //また、削除直前には移動場所にできないようにする。（ただし、削除予定エリアから削除予定エリアには移動できる（そうしないと時間ギリギリで動けなくなってしまうため））
        
        //直前にエネミーの移動経路に削除予定エリアがある場合、①削除予定エリア外にいる場合は、再度移動経路探索（☆黒水晶に囲まれる可能性あり、優先順位低くして壊せるようにする）　②削除予定エリア外にいる場合は移動停止
        //ステージ最後列にあるオブジェクトを全て削除

        //最後列のリストを全削除、最後列の床オブジェクト削除
        //最前列のリスト生成し、要素を削除したリストと同じものにする、最前列の床オブジェクト生成
        //元々のリストが何列スライドしているか変数に格納(別クラスの処理について、この変数を加味するようなメソッド処理を加える)

        // 例:ステージ移動が10回実行された場合
        // 　10 11 12 13 14 15 16 17 18 19 0 1 2 3 4 5 6 7 8 9
        // 列は上記の要素順になる。初期要素スライド回数変数に10を格納
        // ステージとして計算する時、全体に10をプラスし、リスト最大値を超える20以上の数字は全てマイナス20し、
        // 通常のリストのように計算する。
        // ⇨ これってgetter,setterで実現できない？

        //
        
        _stageMoveCount ++;
    }

    //ステージの最前列、最後列より前、後ろを参照しようとしていないかのチェックメソッド
    //StageMoveExecuteメソッドで最前列が要素0,最後列が最大要素値ではなくなるため、今の最前列から最後列または最後列から最前列の要素を経由している場合は参照不可（false）を返す
    //例①：
    // 要素順A 5 6 0 1 2 3 4
    //に、スライド回数の変数_moveRowCountをマイナスし、マイナスになった値を全体リスト要素数でプラスし、
    // 要素順B 0 1 2 3 4 5 6
    //を作る。
    //Aの二つの座標差分の絶対 < Bの二つの座標差分の絶対値 の時、最前列から最後列または最後列から最前列を経由しているため、参照不可とする。
    //☆一部バグあり　⇨ _judgeElementNoと_centerElementNoの差がステージ長さの半分以上ある場合
    //例②：要素順Aの6から右側へ５マス先の4を参照する場合
    //_judgeElementNo_x:判定したいx座標
    //_centerElementNo_x:判定の中心座標
    public static bool OutOfReferenceCheck(int _judgeElementNo_x, int _centerElementNo_x){
        bool _outOfReferenceFlag = false;

        //現在のリスト要素順での絶対値(要素順A)
        int _nowListAbs = Mathf.Abs(_judgeElementNo_x - _centerElementNo_x);

        //元のリスト要素順での絶対値(要素順B)
        //元のリスト要素順へ戻す(スライド回数変数でマイナスし、マイナスになっている場合は全体リスト要素数でプラス)
        int _undoJudgeElementNo_x = UndoListElement(_judgeElementNo_x);
        int _undoCenterElementNo_x = UndoListElement(_centerElementNo_x);

        int _undoListAbs = Mathf.Abs(_undoJudgeElementNo_x - _undoCenterElementNo_x);

        //現在リスト要素順での絶対値と元のリスト要素順での絶対値を比較する
        _outOfReferenceFlag = _nowListAbs - _undoListAbs < 0;
        
        return _outOfReferenceFlag;
    }

    //ワールド座標でステージを見た際の座標順を返す（一番左が0で一番右がリスト最大値）
    public static int UndoListElement(int _elementNo){
        int _undoElementNo;
        return _undoElementNo = (_elementNo - _moveRowCount >= 0)? _elementNo - _moveRowCount : _elementNo - _moveRowCount + AStarMap.max_pos_x_static;
    }
}
