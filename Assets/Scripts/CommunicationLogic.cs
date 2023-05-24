using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CommunicationLogic : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ClientRpc]
    public void OutOfBoundseClientRpc(Vector3 position, Projectile.BoundaryDirection direction)
    {
        if (!IsOwner)
        {
            print(position + "  " + direction);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestOutOfBoundseServerRpc(Vector3 position, Projectile.BoundaryDirection direction)
    {
        OutOfBoundseClientRpc(position, direction);
    }
}
