using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    /*// Start is called before the first frame update
    private GameObject ParentRoom;
    public RoomTransition TransitionPartner;
    public Vector2 offset;
    private BoxCollider2D collider;
    public LayerMask PlayerLayer;

    void Start()
    {
        ParentRoom = transform.parent.gameObject;
        collider = GetComponent<BoxCollider2D>();
    }

    void OnDrawGizmos()
    {
        //draw where the player will end up when teleported to this transtion
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(transform.position + (Vector3)Offset, new Vector3(1.27692604f, 1.77393925f, 0.0f));
    }

    void Update()
    {
        //check if the player enters the transition
        RaycastHit2D boxhit;
        if (boxhit = Physics2D.BoxCast(collider.bounds.center, collider.bounds.size,
            0.0f, new Vector2(0.0f, 0.0f), 0.0f, PlayerLayer))
        {
            boxhit.transform.position = TransitionPartner.transform.position + (Vector3)TransitionPartner.Offset;
            ParentRoom.GetComponent<Room>().ResetDynamics();
            TransitionPartner.ParentRoom.GetComponent<Room>().EnableDynamics();
        }
    }*/
}
