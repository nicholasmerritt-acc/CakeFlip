using UnityEngine;

public class PauseMenuBackButton : PanelBackButton
{
    public override void DeactivatePanel()
    {
        GameManager.Instance.TogglePause();
        base.DeactivatePanel();
    }
}
