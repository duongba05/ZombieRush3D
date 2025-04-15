using Fusion;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public CharacterController characterController;
    public float speed = 5f;
    public Animator animator;
    public override void FixedUpdateNetwork()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(horizontal, 0, vertical);
        characterController.Move(move * speed * Runner.DeltaTime);
    }
}
