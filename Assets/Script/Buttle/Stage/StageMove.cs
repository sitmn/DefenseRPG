using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMove : MonoBehaviour
{
    //ステージ移動カウント用
    public static int _stageMoveCount;
    //ステージ移動時間間隔
    public static int _stageMoveMaxCountStatic;
    [SerializeField]
    public int _stageMoveMaxCount;
    //ステージ列のスライド量
    public static int _moveRowCount;

    //ステージ親オブジェクト
    private Transform _stageParentTr;
    //ステージ列プレハブ
    [SerializeField]
    private GameObject _stageRowObj;
    //ステージ列オブジェクト
    private List<GameObject> _stageRowObjList;

    //敵、水晶の自動生成用プレハブ
    [SerializeField]
    private GameObject _crystalPrefab;
    [SerializeField]
    private GameObject _enemyPrefab;
    //生成先親オブジェクト
    [SerializeField]
    private GameObject _crystalParent;
    [SerializeField]
    private GameObject _enemyParent;
    //ステージ拡張時の生成数
    [SerializeField]
    private int _enemyInstantiateAmount;
    [SerializeField]
    private int _crystalInstantiateAmount;
    private EnemyParamAsset _enemyParamData;
    private CrystalParamAsset _crystalParamData;

    void Awake(){
        _enemyParamData = Resources.Load("Data/EnemyParamData") as EnemyParamAsset;
        _crystalParamData = Resources.Load("Data/CrystalParamData") as CrystalParamAsset;

        _stageParentTr = this.gameObject.GetComponent<Transform>();

        //カウントの初期化
        _moveRowCount = 0;

        _stageMoveMaxCountStatic = _stageMoveMaxCount;
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
    public void UpdateManager()
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
    
    //ステージ移動を実行
    private void StageMoveExecute(){
        //プレイヤーがステージ最後列にいる場合、ゲームオーバー表示し、全ての入力を無効に
        if(AStarMap._playerPos.x == 0) GameManager.GameOver();

        //ステージ最後列にあるオブジェクトを全て削除
        StageRowDestroy();
        //列の情報を全て1つ隣に移動させる（要素をマイナス1する）
        // 　・エネミーのTrackPos
        // 　・判定座標:AStarMap利用時にStageMove.UndoElementで変換するので移動不要
        // 　　・敵
        // 　　・水晶
        // 　　・プレイヤー
        // 　・移動用座標:AStarMap利用時にStageMove.UndoElementで変換するので移動不要
        // 　　・敵
        // 　　・プレイヤー

        //ステージ列移動前の水晶情報を全て消す
        CrystalInfomationInMapDelete();
        //ステージ列移動前の敵情報を全て消す
        EnemyInfomationInMapDelete();

        //最後列の床オブジェクト削除
        //最前列のリスト生成
        //最前列の床オブジェクト生成
        StageRowCreateAndDelete();
        //元々のリストが何列スライドしているか変数に格納（ワールド座標をリストの要素に変換するために使用）
        _moveRowCount ++;

        //ステージ列移動後の水晶情報を全て入れる
        CrystalInfomationInMapCreate();
        //ステージ列移動後の敵位置を全て更新。情報も全て入れる
        SlideAllPos();

        //最前列に新規でエネミーと黒水晶を生成
        InstantiateStageObj(_crystalInstantiateAmount,_enemyInstantiateAmount);
    }

    //ステージ最後列のオブジェクト（エネミーとクリスタル）を全て削除 削除列へ移動しようとしているエネミーも削除
    private void StageRowDestroy(){
        List<ACrystalCore> _crystalCoreList = new List<ACrystalCore>();
        List<AEnemyCore> _enemyCoreList = new List<AEnemyCore>();

        //削除前に別のリストに入れる（削除時、リストをRemoveするため）
        //最後列のエネミーとクリスタルをリストへ追加
        for(int i = 0; i < AStarMap.max_pos_z_static ; i++){
            if(AStarMap.astarMas[0, i]._crystalCore != null) _crystalCoreList.Add(AStarMap.astarMas[0, i]._crystalCore);
            for(int j = 0; j < AStarMap.astarMas[0, i]._enemyCoreList.Count ; j++){
                _enemyCoreList.Add(AStarMap.astarMas[0, i]._enemyCoreList[j]);
            }
        }
        //最後列へ移動しようとしているエネミーをリストへ追加
        for(int i = 0;i < EnemyListCore._enemiesList.Count ; i++){
            if(EnemyListCore._enemiesList[i]._enemyMove.TrackPos[0].x == 0){
                _enemyCoreList.Add(EnemyListCore._enemiesList[i]);
            }
        }

        //ステージ外になるオブジェクトを全て削除
        foreach(ACrystalCore __crystalCoreObj in _crystalCoreList){
            __crystalCoreObj.ObjectDestroy();
        }
        foreach(AEnemyCore _enemyCoreObj in _enemyCoreList){
            _enemyCoreObj.ObjectDestroy();
        }
    }

    //ステージ最後列削除し、ステージ最前列生成
    private void StageRowCreateAndDelete(){
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

    //水晶情報をMapから全て削除
    private void CrystalInfomationInMapDelete(){
        for(int i = 0; i < CrystalListCore._crystalList.Count ; i++){
            //リフト中の水晶は対象外
            if(CrystalListCore._crystalList[i] != LiftCrystal._crystalCore) CrystalListCore._crystalList[i].SetOffAStarMap();
        }
    }

    //水晶情報をMapに全て生成
    private void CrystalInfomationInMapCreate(){
        for(int i = 0; i < CrystalListCore._crystalList.Count ; i++){
            //リフト中の水晶は対象外
            if(CrystalListCore._crystalList[i] != LiftCrystal._crystalCore) CrystalListCore._crystalList[i].SetOnAStarMap();
        }
    }

    //敵情報をMapから全て削除
    private void EnemyInfomationInMapDelete(){
        for(int i = 0; i < EnemyListCore._enemiesList.Count ; i++){
            EnemyListCore._enemiesList[i].SetOffAStarMap(EnemyListCore._enemiesList[i].JudgePos.Value);
        }
    }

    //ステージ移動により１マスズレるTrackPos,EnemyPos,JudgePosを修正。JudgePos更新により、敵情報も作成される。
    private void SlideAllPos(){
        for(int i = 0; i < EnemyListCore._enemiesList.Count ; i++){
            //現在位置のスライド
            //EnemyListController._enemiesList[i].EnemyPos.Value = new Vector2Int(EnemyListController._enemiesList[i].EnemyPos.Value.x + 1, EnemyListController._enemiesList[i].EnemyPos.Value.y);

            //TrackPosが１つのものはEnemyPosが0の時、StageMoveのUndoElementすると最前列になってしまうため、_trackChangeFlagをTrueにしてEnemyListControllerのMove処理を変える
            // if(EnemyListController._enemiesList[i].TrackPos.Count == 1){
            //     EnemyListController._enemiesList[i].TrackPos[0] = new Vector2Int(EnemyListController._enemiesList[i].TrackPos[0].x - 1, EnemyListController._enemiesList[i].TrackPos[0].y);
            //     EnemyListController._enemiesList[i]._trackChangeFlag = true;
            // }else{
            //ステージ移動分、座標をスライド
            for(int j = 0; j < EnemyListCore._enemiesList[i]._enemyMove.TrackPos.Count ; j++){
                // if(EnemyListController._enemiesList[i].TrackPos[j].x == 0){
                //     EnemyListController._enemiesList[i]._trackChangeFlag = true;

                //     //☆ここでTrackPosする？
                //     break;
                // }
                //ステージ外が移動経路になっていなければ、ステージ移動に合わせて移動経路をスライド
                EnemyListCore._enemiesList[i]._enemyMove.TrackPos[j] = new Vector2Int(EnemyListCore._enemiesList[i]._enemyMove.TrackPos[j].x - 1, EnemyListCore._enemiesList[i]._enemyMove.TrackPos[j].y);
                
            }
            EnemyListCore._enemiesList[i].EnemyPos.Value = new Vector2Int(EnemyListCore._enemiesList[i].EnemyPos.Value.x - 1, EnemyListCore._enemiesList[i].EnemyPos.Value.y);
            //EnemyListController._enemiesList[i].SetJudgePos();
            EnemyListCore._enemiesList[i].JudgePos.Value = new Vector2Int(EnemyListCore._enemiesList[i].JudgePos.Value.x - 1, EnemyListCore._enemiesList[i].JudgePos.Value.y);
            
            EnemyListCore._enemiesList[i].SetOnAStarMap(EnemyListCore._enemiesList[i].JudgePos.Value);
            
            AStarMap._playerPos.x -= 1;
            AStarMap._playerPos = new Vector2Int(AStarMap._playerPos.x, AStarMap._playerPos.y);
            //Debug.Log("Track:" + EnemyListController._enemiesList[i]._enemyMove.TrackPos[0] + " Enemy:" + EnemyListController._enemiesList[i].EnemyPos + " Judge:" + EnemyListController._enemiesList[i].JudgePos + "AAA");
        }
    }


    //ゲームオーバー表示とプレイヤー入力を無効化
    private void GameOver(){
        Debug.Log("がめおべら"); //☆プレイヤーのHPを0にする
        
        
    }

    private void InstantiateStageObj(int _crystalAmount, int _enemyAmount){
        //黒水晶を最前列に配置
        List<int> _listNumberList = new List<int>();
        //配置位置決定用の整数リスト
        for(int i = 0; i < AStarMap.max_pos_z_static; i++){
            _listNumberList.Add(i);
        }

        //ランダムな数字を水晶生成数分取得(生成に位置の重複がないよう使用したリストは削除)
        List<int> _randomNumberList = new List<int>();
        for(int i = 0;i < _crystalAmount; i++){
            int _randomNumber = Random.Range(0,_listNumberList.Count - 1);
            _randomNumberList.Add(_listNumberList[_randomNumber]);
            
            _listNumberList.RemoveAt(_randomNumber);
        }

        //最前列に水晶を生成
        for(int i = 0 ; i < _randomNumberList.Count; i++){
            GameObject _crystal = Instantiate(_crystalPrefab, new Vector3(StageMove._moveRowCount + AStarMap.max_pos_x_static - 1, 0.5f , _randomNumberList[i]), Quaternion.identity);
            _crystal.transform.parent = _crystalParent.transform;
            //生成したクリスタルを管理しているリストにセット
            CrystalListCore.SetCrystalCoreInList(_crystal.GetComponent<CrystalCore>(), _crystalParamData.CrystalParamList[0]);
        }
        
        //ランダムな数字を敵生成数分取得(生成に位置の重複がないよう使用したリストは削除)
        _randomNumberList = new List<int>();
        for(int i = 0;i < _enemyAmount; i++){
            int _randomNumber = Random.Range(0,_listNumberList.Count - 1);
            _randomNumberList.Add(_listNumberList[_randomNumber]);
            
            _listNumberList.RemoveAt(_randomNumber);
        }

        //最前列に水晶を生成
        for(int i = 0 ; i < _randomNumberList.Count; i++){
            GameObject _enemy = Instantiate(_enemyPrefab, new Vector3(StageMove._moveRowCount + AStarMap.max_pos_x_static - 1, 0.5f , _randomNumberList[i]), Quaternion.identity);
            _enemy.transform.parent = _enemyParent.transform;
            EnemyListCore.SetEnemyCoreInList(_enemy.GetComponent<EnemyCore>(), _enemyParamData.EnemyParamList[0]);
        }
    }

    //ワールド座標でステージを見た際の列順を返す（一番左が0で一番右がリスト最大値）
    public static int UndoElementStageMove(int _judgePos_x){
        _judgePos_x -= _moveRowCount;

        return _judgePos_x;
    }
}
