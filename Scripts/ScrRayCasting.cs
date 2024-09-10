using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrRayCasting : MonoBehaviour
{
    public bool in_range = false;
    public bool alreadyUsed = false;
    public ScrGameMaster GameMaster;
    public GameObject target;
    private bool MouseClicked = false;
    private float ClickTimeDelayMax = 0.05f;
    private float ClickTimeDelayNow = 0;


    void Start()
    {
        GameMaster = GameObject.FindWithTag("GameMaster").GetComponent<ScrGameMaster>();
    }

    void Update()
    {
        #region Ajusta timing do mouse
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Mouse clicked");
            ClickTimeDelayNow = ClickTimeDelayMax;
            MouseClicked = true;
        }
        else if (ClickTimeDelayNow > 0)
        {
            //Debug.Log(ClickTimeDelayNow);
            ClickTimeDelayNow -= Time.deltaTime;
        }
        else MouseClicked = false;
        #endregion
    }


    // check if raycast is colliding with succeptible objects and calls respective functions
    private void OnTriggerStay(Collider other)
    {
        #region raycast with Museum Items
        if (other.CompareTag("MuseumItem"))
        {
            target = other.gameObject;
            
            Debug.Log("Colisão de Raycast com '" + target + "'");
            if (MouseClicked)
            {
                // calls ScrGameMaster function to turn off their mesh
                GameMaster.rf_MeshTurnOff(target);

            }
        }
        #endregion

        #region raycast with Destroyable Walls
        else if (other.CompareTag("DestroyWall"))
        {
            target = other.gameObject;
            Debug.Log("Colisão de Raycast com '" + target + "'");
            if (MouseClicked)
            {
                // calls ScrGameMaster function to check if you have the necessary key
                // and destroys wall if so

                ///@param
                ///_MeshRenderer = visibility of object
                ///_BoxCollider = collision of object
                ///_WallNumber = what wall is it destroying
                GameMaster.rf_DestroyWall(target.GetComponent<MeshRenderer>(), target.GetComponent<BoxCollider>(), target.name);
            }
        }
        #endregion

        #region raycast with Ghost
        if (other.CompareTag("GhostNPC"))
        {
            target = other.gameObject;

            Debug.Log("Colisão de Raycast com '" + target + "'");
            if (MouseClicked)
            {
                // calls ScrGameMaster function to turn off their mesh
                GameMaster.rf_GhostInteraction(target);

            }
        }
        #endregion
    }
}
