using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private StageMove _stageMove;
    [SerializeField]
    private PlayerController _playerController;
    [SerializeField]
    private LiftCrystal _liftCrystal;
    [SerializeField]
    private RepairCrystal _repairCrystal;
    [SerializeField]
    private CrystalListController _crystalListController;
    [SerializeField]
    private EnemyListController _enemyListController;
    
    // Update is called once per frame
    void FixedUpdate()
    {
        _playerController.UpdateManager();
        _liftCrystal.UpdateManager();
        _repairCrystal.UpdateManager();
        _crystalListController.UpdateManager();
        _enemyListController.UpdateManager();
        _stageMove.UpdateManager();

    }
}
