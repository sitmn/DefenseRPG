using UnityEngine;

public class PlayerMotion : MonoBehaviour, IPlayerMotion
{
    [SerializeField]
    private Animator _playerAnimator;


    //移動モーション
    public void MoveMotion(){
        _playerAnimator.SetBool(ConstManager._moveAction, true);        
    }

    public void MoveMotionCancel(){
        _playerAnimator.SetBool(ConstManager._moveAction, false);
    }
}
