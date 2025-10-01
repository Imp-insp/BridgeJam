
public class SpikesI : Interactable
{
    public override void Interact()
    {
      PlayerManager.Instance.Die();
    }
}
