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

    public static bool _isGameOver;
    public static bool GetIsGameOver(){
        return _isGameOver;
    }

    void Awake(){
        //CoreクラスのParamをセット
        SetParamData();

        _playerCoreStatic = _playerCore;
        _isGameOver = false;
        
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

    //各クラスのAwakeをコール
    private void DoAwakeManager(){
        _starMap.AwakeManager();
        _playerCore.AwakeManager(_playerParamData.PlayerParamList[0]);
        _playerCore._playerMove.AwakeManager();
        _crystalListCore.AwakeManager(_crystalParamData.CrystalParamList[0]);
        _enemyListCore.AwakeManager(_enemyParamData.EnemyParamList[0]);
        _stageMove.AwakeManager();
        
        foreach(var _playerAction in _playerActionList){
            _playerAction.AwakeManager();
        }
    }
    
    //各クラスのUpdateをコール
    void FixedUpdate()
    {
        if(_isGameOver) return;
        // if(StageMove._stageMoveCount == StageMove._stageMoveMaxCount-1) Debug.Log(AStarMap.astarMas[CrystalListCore._crystalList[0]._crystalPos.x, CrystalListCore._crystalList[0]._crystalPos.y]._crystalCore + "RRR");
        // if(StageMove._stageMoveCount == 0) Debug.Log(AStarMap.astarMas[CrystalListCore._crystalList[0]._crystalPos.x, CrystalListCore._crystalList[0]._crystalPos.y]._crystalCore + "TTT");
        _playerCore.UpdateManager();
        
        foreach(var _playerAction in _playerActionList){
            _playerAction.UpdateManager();
        }
        _crystalListCore.UpdateManager();
        _enemyListCore.UpdateManager();
        _stageMove.UpdateManager(); 
        _cameraController.UpdateManager();
    }

    //ゲームオーバー
    public static void GameOver(){
        //ゲーム内の動きを停止
        _isGameOver = true;

        //プレイヤー操作を無効化
        _playerCoreStatic.InputInvalid();

        Debug.Log("がめおべら");
    }
}
