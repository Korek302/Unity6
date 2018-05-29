using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Messenger.Broadcast(GameEvent.PICK_UP);
            Destroy(this.gameObject);
        }
    }
}
