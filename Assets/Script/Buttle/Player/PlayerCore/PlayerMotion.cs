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

    /// <summary>
    /// リフトダウンモーション開始メソッド
    /// </summary>
    public void StartLiftDownMotion(){
        _playerAnimator.SetBool("_liftDown", true);
    }
    /// <summary>
    /// リフトダウンモーション終了メソッド
    /// </summary>
    public void EndLiftDownMotion(){
        _playerAnimator.SetBool("_liftDown", false);
        _playerAnimator.SetBool("_crystalLift", false);
    }
    /// <summary>
    /// リフトダウンモーションキャンセルメソッド
    /// </summary>
    public void CancelLiftDownMotion(){
        _playerAnimator.SetBool("_liftDown", false);
    }
    /// <summary>
    /// リフトアップモーション開始メソッド
    /// </summary>
    public void StartLiftUpMotion(){
        _playerAnimator.SetBool("_liftUp", true);
    }
    /// <summary>
    /// リフトアップモーション終了メソッド
    /// </summary>
    public void EndLiftUpMotion(){
        _playerAnimator.SetBool("_liftUp", false);
        _playerAnimator.SetBool("_crystalLift", true);
    }
    /// <summary>
    /// リフトアップモーションキャンセルメソッド
    /// </summary>
    public void CancelLiftUpMotion(){
        _playerAnimator.SetBool("_liftUp", false);
    }
    /// <summary>
    /// リペアモーション開始メソッド
    /// </summary>
    public void StartRepairMotion(){
        _playerAnimator.SetBool("_repair", true);
    }
    /// <summary>
    /// リペアモーション終了メソッド
    /// </summary>
    public void EndRepairMotion(){
        _playerAnimator.SetBool("_repair", false);
    }
    /// <summary>
    /// ランクアップモーション開始メソッド
    /// </summary>
    public void StartRankUpMotion(){
        _playerAnimator.SetBool("_rankUp", true);
    }
    /// <summary>
    /// ランクアップモーション終了メソッド
    /// </summary>
    public void EndRankUpMotion(){
        _playerAnimator.SetBool("_rankUp", false);
    }
    /// <summary>
    /// 起動モーション開始メソッド
    /// </summary>
    public void StartLaunchMotion(){
        _playerAnimator.SetBool("_launch", true);
    }
    /// <summary>
    /// 起動モーション終了メソッド
    /// </summary>
    public void EndLaunchMotion(){
        _playerAnimator.SetBool("_launch", false);
    }

    /// <summary>
    /// ゲームオーバモーション開始メソッド
    /// </summary>
    public void StartGameOverMotion(){
        _playerAnimator.SetBool("_gameOver", true);
    }
}
