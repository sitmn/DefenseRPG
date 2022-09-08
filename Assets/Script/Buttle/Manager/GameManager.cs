using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlayerParamAsset _playerParamData;
    private EnemyParamAsset _enemyParamData;
    private CrystalParamAsset _crystalParamData;
    private SystemParamAsset _systemParamData;
    [SerializeField]
    private StageMove _stageMove;
    [SerializeField]
    private PlayerCore _playerCore;
    private List<IPlayerAction> _playerActionList;
    private ActionCost _actionCost;
    [SerializeField]
    private CrystalListCore _crystalListCore;
    [SerializeField]
    private EnemyListCore _enemyListCore;
    [SerializeField]
    private CameraController _cameraController;
    [SerializeField]
    private UIManager _UIManager;

    public static bool _isGameOver;
    public static bool GetIsGameOver(){
        return _isGameOver;
    }

    void Awake(){
        //CoreクラスのParamをセット
        SetParamData();

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
        _systemParamData = Resources.Load("Data/SystemParamData") as SystemParamAsset;
        //上記とは別にアタックステータスクラスをインスタンス化
        _enemyParamData.SetAttackStatus();
        _crystalParamData.SetAttackStatus();
    }
    private void InitializeComponent(){
        _actionCost = _playerCore.GetComponent<ActionCost>();
        _playerActionList = new List<IPlayerAction>();
        _playerActionList.Add(_playerCore.GetComponent<LiftUpCrystal>());
        _playerActionList.Add(_playerCore.GetComponent<LiftDownCrystal>());
        _playerActionList.Add(_playerCore.GetComponent<RepairCrystal>());
        _playerActionList.Add(_playerCore.GetComponent<CrystalRankUp>());
    }

    //各クラスのAwakeをコール
    private void DoAwakeManager(){
        MapManager.AwakeManager(_systemParamData.SystemParamList[0]);
        _playerCore.AwakeManager(_playerParamData.PlayerParamList[0], _crystalParamData, _UIManager);
        _playerCore._playerMove.AwakeManager();
        EnemyMoveRoute.AwakeManager();
        _crystalListCore.AwakeManager(_crystalParamData);
        _enemyListCore.AwakeManager(_enemyParamData.EnemyParamList[0]);
        _stageMove.AwakeManager(_systemParamData.SystemParamList[0]);
        _UIManager.AwakeManager();
        _actionCost.AwakeManager(_systemParamData.SystemParamList[0]);
        foreach(var _playerAction in _playerActionList){
            _playerAction.AwakeManager(_playerParamData.PlayerParamList[0], _UIManager);
        }
    }
    
    //各クラスのUpdateをコール
    void FixedUpdate()
    {
        if(_isGameOver) return;

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
        _isGameOver = true;

        //プレイヤー操作を無効化
        MapManager._playerCore.InputInvalid();

        Debug.Log("がめおべら");
    }
}
