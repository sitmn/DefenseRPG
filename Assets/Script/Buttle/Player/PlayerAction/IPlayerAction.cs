public interface IPlayerAction
{
    //アクション中フラグ
    public bool IsAction{get;}
    //アクション有効化フラグ
    public bool IsActive{get;}
    //クラスの初期化
    public void AwakeManager(PlayerParam _playerParam, CrystalParamAsset _crystalParamAsset, UIManager _UIManager);
    public void UpdateManager();
    //アクションを有効に
    public void InputEnable();
    //アクションコスト不足時の処理
    public void ShortageActionCost();
    //アクションを無効に
    public void InputDisable();
    //アクション可能か
    public bool CanAction();
    //アクションコストが足りているか
    public bool EnoughActionCost();
}
