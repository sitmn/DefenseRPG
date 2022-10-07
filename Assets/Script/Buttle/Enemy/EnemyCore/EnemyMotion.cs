using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //ダメージモーション（ダメージ中は移動停止 or ダメージモーションなし）
}
