using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private EnemyParamAsset _enemyParamData;
    private CrystalParamAsset _crystalParamData;
    [SerializeField]
    private AStarMap _starMap;
    [SerializeField]
    private StageMove _stageMove;
    [SerializeField]
    private PlayerCore _playerCore;
    public static PlayerCore _playerCoreStatic;
    [SerializeField]
    private LiftCrystal _liftCrystal;
    [SerializeField]
    private RepairCrystal _repairCrystal;
    [SerializeField]
    private CrystalListCore _crystalListCore;
    [SerializeField]
    private EnemyListCore _enemyListCore;
    [SerializeField]
    private CameraController _cameraController;

    public static bool _gameOverFlag;

    void Awake(){
        _enemyParamData = Resources.Load("Data/EnemyParamData") as EnemyParamAsset;
        _crystalParamData = Resources.Load("Data/CrystalParamData") as CrystalParamAsset;
        _playerCoreStatic = _playerCore;
        _gameOverFlag = false;

        _starMap.AwakeManager();
        _crystalListCore.AwakeManager(_crystalParamData.CrystalParamList[0]);
        _enemyListCore.AwakeManager(_enemyParamData.EnemyParamList[0]);
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if(_gameOverFlag) return;

        _playerCore.UpdateManager();
        _liftCrystal.UpdateManager();
        _repairCrystal.UpdateManager();
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
