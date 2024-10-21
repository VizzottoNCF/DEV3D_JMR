using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScrGhostInteraction : MonoBehaviour
{
    public bool IsCoroutinePlaying = false;
    public bool IsCoroutineBackPlaying = false;
    private ScrGameMaster GameMaster;
    private Transform self;
    private GameObject PlayerCharacter;
    public GameObject GhostKey1;
    public GameObject GhostKey2;
    public GameObject Wall1;
    public GameObject Wall2;
    public string[] dialogue;
    public string[] dialogueStart;
    public string[] dialogueKey1;
    public string[] dialogueKey2;
    public string[] dialogueEnd;
    private float RotationSpeed = 1f;
    public bool GhostKey1Used;
    public bool GhostKey2Used;
    public bool FirstInteraction;


    void Start()
    {
        self = this.GetComponent<Transform>();

        GameMaster = GameObject.FindWithTag("GameMaster").GetComponent<ScrGameMaster>();

        PlayerCharacter = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (!FirstInteraction)
        {
            dialogue = dialogueStart;
        }
        else if (!GhostKey1Used)
        {
            dialogue = dialogueKey1;
        }
        else if (!GhostKey2Used)
        {
            dialogue = dialogueKey2;
        }
        else
        {
            dialogue = dialogueEnd;
        }


        // rotate to player
        if (Vector3.Distance(self.position, PlayerCharacter.transform.position) < 5f)
        {
            rf_StartRotating();
        }
        else if (IsCoroutinePlaying)
        {
            StopAllCoroutines();
            IsCoroutinePlaying = false;
            rf_RotateBack();
        }
        else
        {
            if (self.rotation != Quaternion.Euler(0, 0, 0))
            {
                rf_RotateBack();
            }
        }
    }



    // character spins and looks towards player

    private void rf_StartRotating()
    {
        StartCoroutine(rIE_LookAtTarget());
    }

    private void rf_RotateBack()
    {
        StartCoroutine(rIE_LookAtDefault());
    }

    private IEnumerator rIE_LookAtTarget()
    {
        IsCoroutinePlaying = true;
        Vector3 Target = new Vector3(PlayerCharacter.transform.position.x, self.position.y, PlayerCharacter.transform.position.z);
        Quaternion lookRotation = Quaternion.LookRotation(Target - self.position);

        float time = 0;

        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(self.rotation, lookRotation, time);

            time += Time.deltaTime * RotationSpeed;

            yield return null;
        }

        IsCoroutinePlaying = false;

    }
    private IEnumerator rIE_LookAtDefault()
    {
        IsCoroutineBackPlaying = true;
        Debug.Log("Entered coroutine");
        float time = 0;

        while (time < 2)
        {
            Debug.Log("In while");
            transform.rotation = Quaternion.Slerp(self.rotation, Quaternion.Euler(0,0,0), 2);

            time += Time.deltaTime * RotationSpeed;

            yield return null;
        }
        Debug.Log("Exited While Coroutine");
        IsCoroutineBackPlaying = false;
    }
}
