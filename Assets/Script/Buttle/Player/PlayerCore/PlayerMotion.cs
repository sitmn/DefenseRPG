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
        _playerAnimator.SetBool(ConstManager._moveAction, true);        
    }

    public void MoveMotionCancel(){
        _playerAnimator.SetBool(ConstManager._moveAction, false);
    }
}
