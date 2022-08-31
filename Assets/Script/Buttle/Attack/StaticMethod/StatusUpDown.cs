using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

//バフデバフクラス
public class StatusUpDown
{
    //スピードを上昇させ、トークンをセット
    public static void ApplySpeedBuff<T>(T _applyCore, StatusBase _status){
        if(typeof(T) == typeof(PlayerCore)){
            PlayerCore _playerCore = (PlayerCore)(object)_applyCore;
            if(_playerCore != null){
                _playerCore._playerStatus._moveSpeedUp = _status._attackStatus._effectRate[_status._level];
                _playerCore._speedBuff.SetActive(true);
                _playerCore._playerStatus._cancelSpeedBuffToken.Cancel();
                _playerCore._playerStatus._cancelSpeedBuffToken = new CancellationTokenSource();
                UndoBuffSpeed(_applyCore, _status._attackStatus._effectTime[_status._level], _playerCore._speedBuff, _playerCore._playerStatus._cancelSpeedBuffToken, _playerCore._playerStatus._cancelSpeedBuffToken.Token);
            }
            
        }
        if(typeof(T) == typeof(EnemyCoreBase)){
            EnemyCoreBase _enemyCore = (EnemyCoreBase)(object)_applyCore;
            if(_enemyCore != null){
                _enemyCore._enemyStatus._moveSpeedUp = _status._attackStatus._effectRate[_status._level];
                _enemyCore._speedBuff.SetActive(true);
                _enemyCore._enemyStatus._cancelSpeedBuffToken.Cancel();
                _enemyCore._enemyStatus._cancelSpeedBuffToken = new CancellationTokenSource();
                UndoBuffSpeed(_applyCore, _status._attackStatus._effectTime[_status._level], _enemyCore._speedBuff, _enemyCore._enemyStatus._cancelSpeedBuffToken, _enemyCore._enemyStatus._cancelSpeedBuffToken.Token);
            }
        }
    }

    //スピードを減少させ、トークンをセット
    public static void ApplySpeedDebuff<T>(T _applyCore, StatusBase _status){
        if(typeof(T) == typeof(PlayerCore)){
            PlayerCore _playerCore = (PlayerCore)(object)_applyCore;
            if(_playerCore != null){
                _playerCore._playerStatus._moveSpeedDown = _status._attackStatus._effectRate[_status._level];
                _playerCore._speedDebuff.SetActive(true);
                _playerCore._playerStatus._cancelSpeedDebuffToken.Cancel();
                _playerCore._playerStatus._cancelSpeedDebuffToken = new CancellationTokenSource();
                UndoDebuffSpeed(_applyCore, _status._attackStatus._effectTime[_status._level], _playerCore._speedDebuff, _playerCore._playerStatus._cancelSpeedDebuffToken, _playerCore._playerStatus._cancelSpeedDebuffToken.Token);
            }
        }
        if(typeof(T) == typeof(EnemyCoreBase)){
            EnemyCoreBase _enemyCore = (EnemyCoreBase)(object)_applyCore;
            if(_enemyCore != null){
                _enemyCore._enemyStatus._moveSpeedDown = _status._attackStatus._effectRate[_status._level];
                _enemyCore._speedDebuff.SetActive(true);
                _enemyCore._enemyStatus._cancelSpeedDebuffToken.Cancel();
                _enemyCore._enemyStatus._cancelSpeedDebuffToken = new CancellationTokenSource();
                UndoDebuffSpeed(_applyCore, _status._attackStatus._effectTime[_status._level], _enemyCore._speedDebuff, _enemyCore._enemyStatus._cancelSpeedDebuffToken, _enemyCore._enemyStatus._cancelSpeedDebuffToken.Token);
            }
        }
    }

    //スピード上昇を元に戻す
    public static async UniTask UndoBuffSpeed<T>(T _applyCore, int _effectTime, GameObject _buffObj, CancellationTokenSource _cancelSpeedBuffToken, CancellationToken cancellationToken = default(CancellationToken)){
        await UniTask.Delay(TimeSpan.FromSeconds(_effectTime), cancellationToken: _cancelSpeedBuffToken.Token);
        if(typeof(T) == typeof(PlayerCore)) ((PlayerCore)(object)_applyCore)._playerStatus._moveSpeedUp = 0;
        if(typeof(T) == typeof(EnemyCoreBase)) ((EnemyCoreBase)(object)_applyCore)._enemyStatus._moveSpeedUp = 0;
        _buffObj.SetActive(false);
    }
    //スピード減少を元に戻す
    public static async UniTask UndoDebuffSpeed<T>(T _applyCore, int _effectTime, GameObject _buffObj, CancellationTokenSource _cancelSpeedDebuffToken, CancellationToken cancellationToken = default(CancellationToken)){
        
        await UniTask.Delay(TimeSpan.FromSeconds(_effectTime), cancellationToken: _cancelSpeedDebuffToken.Token);
        if(typeof(T) == typeof(PlayerCore)) ((PlayerCore)(object)_applyCore)._playerStatus._moveSpeedDown = 0;
        if(typeof(T) == typeof(EnemyCoreBase)) ((EnemyCoreBase)(object)_applyCore)._enemyStatus._moveSpeedDown = 0;
        _buffObj.SetActive(false);
    }
}
