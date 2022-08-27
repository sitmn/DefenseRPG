using UnityEngine;
using System.Threading;

public class PlayerStatus
{
    public PlayerStatus(PlayerParam _playerParam){
        _moveSpeed = _playerParam._moveSpeed;
        _moveSpeedUp = 0;
        _moveSpeedDown = 0;
        _cancelSpeedBuffToken = new CancellationTokenSource();
        _cancelSpeedDebuffToken = new CancellationTokenSource();
    }

    //プレイヤーが装備しているクリスタル
    [SerializeField]
    public CrystalStatus[] _crystalStatus;

    //プレイヤーが装備しているクリスタルのマテリアル
    [SerializeField]
    public Material[] _material;

    private float _moveSpeed;
    public float GetMoveSpeed{
        get{
            return _moveSpeed * (1 + _moveSpeedUp + _moveSpeedDown);
        }
    }

    //バフ用変数
    public float _moveSpeedUp;
    //デバフ用変数
    public float _moveSpeedDown;
    public CancellationTokenSource _cancelSpeedBuffToken = new CancellationTokenSource();
    public CancellationTokenSource _cancelSpeedDebuffToken = new CancellationTokenSource();
}
