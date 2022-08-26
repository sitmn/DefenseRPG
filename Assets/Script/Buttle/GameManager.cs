using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlayerParamAsset _playerParamData;
    private EnemyParamAsset _enemyParamData;
    private CrystalParamAsset _crystalParamData;
    [SerializeField]
    private AStarMap _starMap;
    [SerializeField]
    private StageMove _stageMove;
    [SerializeField]
    private PlayerCore _playerCore;
    public static PlayerCore _playerCoreStatic;
    private List<IPlayerAction> _playerActionList;
    [SerializeField]
    private CrystalListCore _crystalListCore;
    [SerializeField]
    private EnemyListCore _enemyListCore;
    [SerializeField]
    private CameraController _cameraController;

    public static bool _gameOverFlag;

    void Awake(){
        //CoreクラスのParamをセット
        SetParamData();

        _playerCoreStatic = _playerCore;
        _gameOverFlag = false;
        
        //コンポーネントを取得
        InitializeComponent();
        //各クラスのAwakeManagerを実行
        DoAwakeManager();
    }

    private void SetParamData(){
        _playerParamData = Resources.Load("Data/PlayerParamData") as PlayerParamAsset;
        _enemyParamData = Resources.Load("Data/EnemyParamData") as EnemyParamAsset;
        _crystalParamData = Resources.Load("Data/CrystalParamData") as CrystalParamAsset;
    }
    private void InitializeComponent(){
        _playerActionList = new List<IPlayerAction>();
        _playerActionList.Add(_playerCore.GetComponent<UseCrystal>());
        _playerActionList.Add(_playerCore.GetComponent<LiftUpCrystal>());
        _playerActionList.Add(_playerCore.GetComponent<LiftDownCrystal>());
        _playerActionList.Add(_playerCore.GetComponent<RepairCrystal>());
    }
    private void DoAwakeManager(){
        _starMap.AwakeManager();
        _playerCore.AwakeManager(_playerParamData.PlayerParamList[0]);
        _crystalListCore.AwakeManager(_crystalParamData.CrystalParamList[0]);
        _enemyListCore.AwakeManager(_enemyParamData.EnemyParamList[0]);

        foreach(var _playerAction in _playerActionList){
            _playerAction.AwakeManager();
        }
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if(_gameOverFlag) return;

        _playerCore.UpdateManager();
        foreach(var _playerAction in _playerActionList){
            _playerAction.UpdateManager();
        }
        _crystalListCore.UpdateManager();
        _enemyListCore.UpdateManager();
        //_stageMove.UpdateManager(); 
        _cameraController.UpdateManager();
    }

    //ゲームオーバー
    public static void GameOver(){
        //ゲーム内の動きを停止
        _gameOverFlag = true;

        //プレイヤー操作を無効化
        _playerCoreStatic.InputInvalid();

        Debug.Log("がめおべら");
    }
}
