using System.Collections.Generic;
using UnityEngine;

public class StageMove : MonoBehaviour
{
    
    //ステージ移動カウント用
    public static int _stageMoveCount;
    //ステージ移動時間間隔
    public static int _stageMoveMaxCount;
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

    public void AwakeManager(SystemParam _systemParam){
        _enemyParamData = Resources.Load("Data/EnemyParamData") as EnemyParamAsset;
        _crystalParamData = Resources.Load("Data/CrystalParamData") as CrystalParamAsset;

        _stageParentTr = this.gameObject.GetComponent<Transform>();

        SetParam(_systemParam);
        SetStageRow();
    }
    //変数の初期値セット
    private void SetParam(SystemParam _systemParam){
        //カウントの初期化
        _moveRowCount = 0;
        _stageMoveMaxCount = _systemParam._stageMoveMaxCount;
    }

    //ステージ列をセット
    private void SetStageRow(){
        _stageRowObjList = new List<GameObject>();
        for(int i = 0 ; i < MapManager.max_pos_x ; i++){
            _stageRowObjList.Add(_stageParentTr.GetChild(i).gameObject);
        }
    }

    //Update処理
    public void UpdateManager()
    {
        //時間経過でステージを移動
        if(!CountStageMove()) return;
        DoStageMove();
    }

    //ステージ移動タイマー用カウント
    private bool CountStageMove(){
        bool _stageMoveCountFlag = false;
        _stageMoveCount ++;

        if(_stageMoveCount >= _stageMoveMaxCount){
            _stageMoveCount = 0;
            _stageMoveCountFlag = true;
        }

        return _stageMoveCountFlag;
    }
    
    //ステージ移動を実行
    private void DoStageMove(){
        //プレイヤーがステージ最後列にいる場合、ゲームオーバー表示し、全ての入力を無効に
        if(MapManager.GetPlayerPos().x == 0) GameManager.GameOver();

        //ステージ最後列にあるオブジェクトを全て削除
        DeleteObjInLastRow();
        //ステージ列移動前のクリスタル情報を全て消す
        DeleteCrystalInMap();
        //ステージ列移動前のエネミー情報を全て消す
        DeleteEnemyInMap();
        //最後列の床オブジェクト削除、最前列の床オブジェクト生成
        CreateAndDeleteStageRow();
        //元々のリストが何列スライドしているか変数に格納（ワールド座標をリストの要素に変換するために使用）
        _moveRowCount ++;
        //ステージ列移動後のクリスタル情報を全て入れる
        CreateCrystalInMap();
        //ステージ列移動後の敵位置を全て更新。情報も全て入れる
        SlideAllPos();
        //最前列に新規でエネミーと黒クリスタルを生成
        InstantiateStageObj(_crystalInstantiateAmount,_enemyInstantiateAmount);
    }

    //ステージ最後列のオブジェクト（エネミーとクリスタル）を全て削除 削除列へ移動しようとしているエネミーも削除
    private void DeleteObjInLastRow(){
        List<CrystalCoreBase> _crystalCoreList = new List<CrystalCoreBase>();
        List<EnemyCoreBase> _enemyCoreList = new List<EnemyCoreBase>();

        //削除前に別のリストに入れる（削除時、リストをRemoveするため）
        //最後列のエネミーとクリスタルをリストへ追加
        for(int i = 0; i < MapManager.max_pos_z ; i++){
            //最後列位置
            Vector2Int _pos = new Vector2Int(0 ,i);
            if(MapManager.GetMap(_pos)._crystalCore != null) _crystalCoreList.Add(MapManager.GetMap(_pos)._crystalCore);
            for(int j = 0; j < MapManager.GetMap(_pos)._enemyCoreList.Count ; j++){
                _enemyCoreList.Add(MapManager.GetMap(_pos)._enemyCoreList[j]);
            }
        }
        //最後列へ移動しようとしているエネミーをリストへ追加
        for(int i = 0;i < EnemyListCore._enemiesList.Count ; i++){
            if(EnemyListCore._enemiesList[i]._enemyMove.TrackPos[0].x == 0){
                _enemyCoreList.Add(EnemyListCore._enemiesList[i]);
            }
        }
        //ステージ外になるオブジェクトを全て削除
        foreach(CrystalCoreBase _crystalCoreObj in _crystalCoreList){
            _crystalCoreObj.ObjectDestroy();
        }
        foreach(EnemyCoreBase _enemyCoreObj in _enemyCoreList){
            _enemyCoreObj.ObjectDestroy();
        }
    }

    //クリスタル情報をMapから全て削除
    private void DeleteCrystalInMap(){
        for(int i = 0; i < CrystalListCore._crystalList.Count ; i++){
            //リフト中の水晶は対象外
            if(CrystalListCore._crystalList[i] != PlayerCore.GetLiftCrystalCore()) CrystalListCore._crystalList[i].SetOffMap();
        }
    }
    //エネミー情報をMapから全て削除
    private void DeleteEnemyInMap(){
        for(int i = 0; i < EnemyListCore._enemiesList.Count ; i++){
            EnemyListCore._enemiesList[i].SetOffMap(EnemyListCore._enemiesList[i].JudgePos.Value);
        }
    }
    //クリスタル情報をMapに全て生成
    private void CreateCrystalInMap(){
        for(int i = 0; i < CrystalListCore._crystalList.Count ; i++){
            //リフト中の水晶は対象外
            if(CrystalListCore._crystalList[i] != PlayerCore.GetLiftCrystalCore()) CrystalListCore._crystalList[i].SetOnMap();
        }
    }

    //ステージ最後列の床を削除し、ステージ最前列の床を生成
    private void CreateAndDeleteStageRow(){
        //ステージ最後列オブジェクトを削除
        Destroy(_stageRowObjList[0]);
        //リストの参照先を１列分スライド
        for(int i = 0 ; i < _stageRowObjList.Count - 1 ; i++){
            _stageRowObjList[i] = _stageRowObjList[i + 1];
        }
        //最前列のオブジェクトを生成
        GameObject _obj = Instantiate(_stageRowObj, new Vector3(MapManager.max_pos_x + _moveRowCount, -0.5f , MapManager.max_pos_z / 2), Quaternion.identity);
        _obj.transform.parent = _stageParentTr;

        //生成したステージ列をリストへ格納
        _stageRowObjList[_stageRowObjList.Count - 1] = _obj;
    }

    //ステージ移動により１マスズレるTrackPos,EnemyPos,JudgePosを修正。JudgePos更新により、敵情報も作成される。
    private void SlideAllPos(){
        //エネミー位置のスライド
        for(int i = 0; i < EnemyListCore._enemiesList.Count ; i++){
            //移動経路のスライド
            for(int j = 0; j < EnemyListCore._enemiesList[i]._enemyMove.TrackPos.Count ; j++){
                EnemyListCore._enemiesList[i]._enemyMove.TrackPos[j] = new Vector2Int(EnemyListCore._enemiesList[i]._enemyMove.TrackPos[j].x - 1, EnemyListCore._enemiesList[i]._enemyMove.TrackPos[j].y);
            }
            //移動用位置のスライド
            EnemyListCore._enemiesList[i].EnemyPos.Value = new Vector2Int(EnemyListCore._enemiesList[i].EnemyPos.Value.x - 1, EnemyListCore._enemiesList[i].EnemyPos.Value.y);
            //判定用位置のスライド
            EnemyListCore._enemiesList[i].JudgePos.Value = new Vector2Int(EnemyListCore._enemiesList[i].JudgePos.Value.x - 1, EnemyListCore._enemiesList[i].JudgePos.Value.y);
        }
        //プレイヤー位置のスライド
        MapManager.SetPlayerPos(new Vector2Int(MapManager.GetPlayerPos().x - 1, MapManager.GetPlayerPos().y));
    }

    //最前列に新規でエネミーと黒クリスタルを生成
    private void InstantiateStageObj(int _crystalAmount, int _enemyAmount){
        //黒水晶を最前列に配置
        List<int> _listNumberList = new List<int>();
        //配置位置決定用の整数リスト
        for(int i = 0; i < MapManager.max_pos_z; i++){
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
            GameObject _crystal = Instantiate(_crystalPrefab, new Vector3(StageMove._moveRowCount + MapManager.max_pos_x - 1, 0.5f , _randomNumberList[i]), Quaternion.identity);
            _crystal.transform.parent = _crystalParent.transform;
            //生成したクリスタルを管理しているリストにセット
            CrystalListCore.SetCrystalCoreInList(_crystal.GetComponent<CrystalCoreBase>(), _crystalParamData.CrystalParamList[0]);
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
            GameObject _enemy = Instantiate(_enemyPrefab, new Vector3(StageMove._moveRowCount + MapManager.max_pos_x - 1, 0.5f , _randomNumberList[i]), Quaternion.identity);
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
