using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrWallDestroy : MonoBehaviour
{
    public int WallNumber;
    public ScrGameMaster GameMaster;
    public GameObject self;
    void Start()
    {
        self = this.gameObject;

        GameMaster = GameObject.FindWithTag("GameMaster").GetComponent<ScrGameMaster>();
    }
}
