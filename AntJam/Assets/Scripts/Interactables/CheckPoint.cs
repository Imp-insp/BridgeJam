
public class CheckPoint : Interactable
{
    public override void Interact()
    {
        PlayerManager.Instance.SetCheckPoint(transform.position);
    }
}