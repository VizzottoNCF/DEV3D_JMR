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
    public string[] dialogue;
    private float yRotStart;
    private float RotationSpeed = 1f;


    void Start()
    {
        self = this.GetComponent<Transform>();

        GameMaster = GameObject.FindWithTag("GameMaster").GetComponent<ScrGameMaster>();

        PlayerCharacter = GameObject.FindWithTag("Player");
    }

    void Update()
    {
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

        Quaternion lookRotation = Quaternion.LookRotation(PlayerCharacter.transform.position - self.position);

        float time = 0;

        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(self.rotation, lookRotation, time);

            time += Time.deltaTime * RotationSpeed;

            yield return null;
        }

        IsCoroutinePlaying = false;

    }private IEnumerator rIE_LookAtDefault()
    {
        IsCoroutineBackPlaying = true;

        float time = 0;

        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(self.rotation, Quaternion.Euler(0,0,0), time);

            time += Time.deltaTime * RotationSpeed;

            yield return null;
        }

        IsCoroutineBackPlaying = false;
    }
}
