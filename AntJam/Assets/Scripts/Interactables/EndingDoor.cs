using UnityEngine;

public class EndingDoor : Interactable
{
[SerializeField] private float yOffset;
    public override void Interact()
    {
        var targetVectr = new Vector2(transform.position.x, transform.position.y-yOffset);
       PlayerMotor.Instance.EndAnimation(targetVectr); 
    }
}
