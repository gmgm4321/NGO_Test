using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloPlayerInit : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject PlayerPrefab;

    void Start()
    {
        if(!GameData.isCreatePlayer)
        { 
            Instantiate(PlayerPrefab);
            GameData.isCreatePlayer = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
