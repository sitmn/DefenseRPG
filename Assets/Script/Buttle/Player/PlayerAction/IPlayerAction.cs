public interface IPlayerAction
{
    public bool ActionFlag{get;}
    public bool ActiveFlag{get;}

    public void AwakeManager();

    public void UpdateManager();

    public void InputEnable();
    public void InputDisable();
    public bool CanAction();
}
