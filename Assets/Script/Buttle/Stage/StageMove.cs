using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 一定時間毎にステージをX方向に1マス移動させるためのクラス
/// オブジェクト位置や移動のためのマス情報を併せてX方向に移動させる
/// </summary>
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
    //ステージ床プレハブ
    [SerializeField]
    private GameObject _stageObj;
    //ステージ下床プレハブ
    [SerializeField]
    private GameObject _stageSupportObj;
    //ステージ下床親オブジェクト
    [SerializeField]
    private Transform _stageSupportParentTr;
    //ポイントライトプレハブ
    [SerializeField]
    private GameObject _lightObj;
    //ポイントライト親オブジェクト
    [SerializeField]
    private Transform _lightParentTr;

    //敵、水晶の自動生成用プレハブ
    [SerializeField]
    private GameObject _crystalPrefab;
    [SerializeField]
    private GameObject _enemy1Prefab;
    [SerializeField]
    private GameObject _enemy2Prefab;
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
    //エリア区画エフェクト移動用スクリプト
    [SerializeField]
    private FieldWall _fieldWallScript;
    

    public void AwakeManager(SystemParam _systemParam){
        _fieldWallScript.AwakeManager();

        _enemyParamData = Resources.Load("Data/EnemyParamData") as EnemyParamAsset;
        _crystalParamData = Resources.Load("Data/CrystalParamData") as CrystalParamAsset;
        
        _stageParentTr = this.gameObject.GetComponent<Transform>();

        SetParam(_systemParam);
        //SetStageRow();
    }
    //変数の初期値セット
    private void SetParam(SystemParam _systemParam){
        //カウントの初期化
        _moveRowCount = 0;
        _stageMoveMaxCount = _systemParam._stageMoveMaxCount;
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

        _fieldWallScript.FlashFieldWall((float)_stageMoveCount / (float)_stageMoveMaxCount);

        if(_stageMoveCount >= _stageMoveMaxCount){
            _stageMoveCount = 0;
            _stageMoveCountFlag = true;
            _fieldWallScript.FlashFieldWall((float)_stageMoveCount / (float)_stageMoveMaxCount);
        }

        return _stageMoveCountFlag;
    }
    
    //ステージ移動を実行
    private void DoStageMove(){
        //ステージ最後列にあるオブジェクトを全て削除
        DeleteObjInLastRow();
        //ステージ列移動前のクリスタル情報を全て消す
        DeleteCrystalInMap();
        //ステージ列移動前のエネミー情報を全て消す
        DeleteEnemyInMap();
        //カメラに映らなくなった後列床/下床/ライトオブジェクト削除、前方に新規で生成
        CreateAndDeleteStage();
        //エリア区画用のエフェクトを移動
        _fieldWallScript.MoveFieldWall();
        
        //プレイヤーがステージ最後列にいる場合または、輸送クリスタルが最後列かつリフトされていない場合、ゲームオーバー表示し、全ての入力を無効に
        if(MapManager.GetPlayerPos().x == 0 || MapManager.GetShippingCrystalPos().x == 0 && !MapManager.IsShippingCrystalLiftUp()){
            GameManager.GameOver();
            return;
        }
        
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
    private void CreateAndDeleteStage(){
        GameObject _deleteFloor = _stageParentTr.GetChild(0).gameObject;
        //ステージ床1個分移動したときに実行
        if((_moveRowCount + 1) % _deleteFloor.transform.localScale.x != 0) return;
        
        //最前列の床オブジェクトを生成
        GameObject _floorObj = Instantiate(_stageObj, new Vector3(_deleteFloor.transform.localScale.x * 2 + _deleteFloor.transform.position.x, -0.5f , MapManager.max_pos_z / 2), Quaternion.identity);
        _floorObj.transform.parent = _stageParentTr;
        //最前列の下床オブジェクトを生成
        GameObject _floorSupportObj = Instantiate(_stageSupportObj, new Vector3(_deleteFloor.transform.localScale.x * 2 + _deleteFloor.transform.position.x, -0.6f , MapManager.max_pos_z / 2), Quaternion.identity);
        _floorSupportObj.transform.parent = _stageSupportParentTr;
        //最後列のポイントライト削除
        Destroy(_lightParentTr.GetChild(0).gameObject);
        //最前列のライトオブジェクトを生成
        GameObject _lightSetObj = Instantiate(_lightObj, new Vector3(_deleteFloor.transform.localScale.x * 2 + _deleteFloor.transform.position.x, 3 , 0), Quaternion.identity);
        _lightSetObj.transform.parent = _lightParentTr;
        Transform _lightSetTr = _lightSetObj.GetComponent<Transform>();
        //ライトオブジェクトの各ライトの位置と光をランダムに変更
        for(int i = 0; i < _lightSetTr.childCount; i ++){
            Debug.Log(_lightSetTr.GetChild(i).position + "AAA");
            _lightSetTr.GetChild(i).position = new Vector3(Random.Range(0, _deleteFloor.transform.localScale.x), 0 , Random.Range(0, _deleteFloor.transform.localScale.z)) + _lightSetTr.position;
            Debug.Log(_lightSetTr.GetChild(i).position + "BBB");
        }
        
        //ステージ最後列オブジェクトを削除
        Destroy(_deleteFloor);
        Destroy(_stageSupportParentTr.GetChild(0).gameObject);
        
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
            int _randomNumber = Random.Range(0,_listNumberList.Count);
            _randomNumberList.Add(_listNumberList[_randomNumber]);
            
            _listNumberList.RemoveAt(_randomNumber);
        }

        //最前列に水晶を生成
        for(int i = 0 ; i < _randomNumberList.Count; i++){
            GameObject _crystal = Instantiate(_crystalPrefab, new Vector3(StageMove._moveRowCount + MapManager.max_pos_x - 1, 0 , _randomNumberList[i]), Quaternion.identity);
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

        //最前列にエネミーを生成
        for(int i = 0 ; i < _randomNumberList.Count; i++){
            GameObject _enemy;
            //enemy1かenemy2のどちらかを生成
            if(Random.Range(0, 2) == 0){
                _enemy = Instantiate(_enemy1Prefab, new Vector3(StageMove._moveRowCount + MapManager.max_pos_x - 1, ConstManager._enemy1PosY , _randomNumberList[i]), Quaternion.identity);
            }else{
                _enemy = Instantiate(_enemy2Prefab, new Vector3(StageMove._moveRowCount + MapManager.max_pos_x - 1, ConstManager._enemy2PosY , _randomNumberList[i]), Quaternion.identity);
            }
            
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
