using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.InputSystem;

public class PlayerProvider : MonoBehaviour, IPlayerProvider
{
    private PlayerInput _playerInput;

    public IReadOnlyReactiveProperty<Vector2> MoveDir => _moveDir;
    public IReadOnlyReactiveProperty<bool> Fire => _fire;

    private ReactiveProperty<Vector2> _moveDir = new ReactiveProperty<Vector2>(Vector2.zero);
    private readonly ReactiveProperty<bool> _fire = new ReactiveProperty<bool>(false);

    // Start is called before the first frame update
    void Start()
    {
        _playerInput = this.gameObject.GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    /*void Update()
    {
        Debug.Log(_playerInput.actions["Move"].ReadValue<Vector2>() + "AAA");
        //移動
        //if(_playerInput.actions["Move"].triggered)
        _moveDir.Value = _playerInput.actions["Move"].ReadValue<Vector2>();
        //攻撃
        //if(_playerInput.actions["Fire"].triggered)
        _fire.Value = _playerInput.actions["Fire"].ReadValue<bool>();
        
        
    }*/
}
