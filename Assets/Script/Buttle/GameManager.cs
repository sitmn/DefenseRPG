using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private StageMove _stageMove;
    [SerializeField]
    private PlayerController _playerController;
    public static PlayerController _playerControllerStatic;
    [SerializeField]
    private LiftCrystal _liftCrystal;
    [SerializeField]
    private RepairCrystal _repairCrystal;
    [SerializeField]
    private CrystalListController _crystalListController;
    [SerializeField]
    private EnemyListController _enemyListController;
    [SerializeField]
    private CameraController _cameraController;

    public static bool _gameOverFlag;

    void Awake(){
        _playerControllerStatic = _playerController;
        _gameOverFlag = false;
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if(_gameOverFlag) return;

        _playerController.UpdateManager();
        _liftCrystal.UpdateManager();
        _repairCrystal.UpdateManager();
        _crystalListController.UpdateManager();
        _enemyListController.UpdateManager();
        //_stageMove.UpdateManager(); 
        _cameraController.UpdateManager();
    }

    //ゲームオーバー
    public static void GameOver(){
        //ゲーム内の動きを停止
        _gameOverFlag = true;

        //プレイヤー操作を無効化
        _playerControllerStatic.InputInvalid();

        Debug.Log("がめおべら");
    }
}
