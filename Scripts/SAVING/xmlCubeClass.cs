using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xmlCubeClass : MonoBehaviour
{
    public MeshRenderer m_MeshRenderer;
    public ScrGameMaster GameMaster;
    public GameObject self;
    private bool MouseClicked = false;

    void Start()
    {
        m_MeshRenderer = GetComponent<MeshRenderer>();
        self = this.gameObject;

        GameMaster = GameObject.FindWithTag("GameMaster").GetComponent<ScrGameMaster>();

    }
    //void Update()
    //{
    //    // turns off cube's mesh renderer
    //    #region cubes
    //    if (self.name == "greenCube")
    //    {
    //        if (GameMaster.greenAcquired) { m_MeshRenderer.enabled = false; }
    //        else { m_MeshRenderer.enabled = true; }
    //    }
    //    else if (self.name == "blueCube")
    //    {
    //        if (GameMaster.blueAcquired) { m_MeshRenderer.enabled = false; }
    //        else { m_MeshRenderer.enabled = true; }
    //    }
    //    else if (self.name == "yellowCube")
    //    {
    //        if (GameMaster.yellowAcquired) { m_MeshRenderer.enabled = false; }
    //        else { m_MeshRenderer.enabled = true; }
    //    }
    //    else if (self.name == "redCube")
    //    {
    //        if (GameMaster.redAcquired) { m_MeshRenderer.enabled = false; }
    //        else { m_MeshRenderer.enabled = true; }
    //    }
    //    #endregion

    //    #region fix mouse
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Debug.Log("Mouse clicked");
    //        ClickTimeDelayNow = ClickTimeDelayMax;
    //        MouseClicked = true;
    //    }
    //    else if (ClickTimeDelayNow > 0)
    //    {
    //        ClickTimeDelayNow -= Time.deltaTime;
    //    }
    //    else MouseClicked = false;
    //    #endregion
    //}

    private void OnTriggerStay(Collider other)
    {
        #region raycast
        if (other.CompareTag("RayCastPlayer"))
        {
            Debug.Log("Colisão de Raycast");
            if (MouseClicked)
            {
                if (self.name == "greenCube")
                {
                    GameMaster.greenAcquired = true;
                }
                else if (self.name == "blueCube")
                {
                    GameMaster.blueAcquired = true;
                }
                else if (self.name == "yellowCube")
                {
                    GameMaster.yellowAcquired = true;
                }
                else if (self.name == "redCube")
                {
                    GameMaster.redAcquired = true;
                }
            }
        }
        #endregion

    }
}
