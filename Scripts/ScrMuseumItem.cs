using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ScrMuseumItem : MonoBehaviour
{
    public bool in_range = false;
    public bool alreadyUsed = false;
    public MeshRenderer m_MeshRenderer;
    public ScrGameMaster GameMaster;
    public GameObject self;
    public ClassGhostKey myClassVars;

    void Start()
    {
        myClassVars = new ClassGhostKey();
        myClassVars.GhostID = 0;
        myClassVars.KeyItemID = 0;

        m_MeshRenderer = GetComponent<MeshRenderer>();
        self = this.gameObject;

        GameMaster = GameObject.FindWithTag("GameMaster").GetComponent<ScrGameMaster>();

    }
}
