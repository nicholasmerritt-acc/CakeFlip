public class Yggdrasil : InteractableEnvironmentItem
{
    protected override string CloseEnoughToInteractMessage
    {
        get => "ALL HAIL THE MIGHTY TREE.";
        set => base.CloseEnoughToInteractMessage = value;
    }
}
