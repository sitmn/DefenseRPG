using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotion : MonoBehaviour, IPlayerMotion
{
    private Animator _playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        _playerAnimator = this.gameObject.GetComponent<Animator>();
    }

    //移動モーション
    public void MoveMotion(){
        _playerAnimator.SetBool("_move", true);        
    }

    public void MoveMotionCancel(){
        _playerAnimator.SetBool("_move", false);
    }
}
