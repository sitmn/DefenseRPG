using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// エネミーのモーション実行クラス。エネミーのアニメーションコントローラのParamを変更させる。
/// </summary>
public class EnemyMotion : MonoBehaviour
{
    //エネミーのアニメーションコントローラ
    public Animator _animationController;


    //移動モーション
    public void StartMoveMotion(){
        _animationController.SetBool("Move",true);
        _animationController.SetBool("Attack",false);
    }

    //停止モーション
    public void StartIdleMotion(){
        _animationController.SetBool("Move", false);
    }

    //攻撃モーション
    public void StartAttackMotion(){
        _animationController.SetBool("Attack", true);
        _animationController.SetBool("Move", false);
    }

    //死亡モーション
    public void StartDeathMotion(){
        _animationController.SetBool("Death", true);
        SetObjDestroy();
    }
    private async UniTask SetObjDestroy(){
        await UniTask.DelayFrame(1);
        await UniTask.WaitUntil(() => {
            AnimatorStateInfo stateInfo = _animationController.GetCurrentAnimatorStateInfo(0);
            return 1f <= stateInfo.normalizedTime;
        });
        
        Destroy(this.gameObject);
    }
}
