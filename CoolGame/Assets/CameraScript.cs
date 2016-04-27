﻿using UnityEngine;
using System.Collections;
using System;

public class CameraScript : MonoBehaviour {
    Transform myTransform;
    public Vector3 target;
    Vector3 currentNonGlitchPosition;
    Vector3 glitchOffset;
    bool glitching = false;
    DateTime glitchStart;
    [SerializeField]
    float glitchDuration = 500;
    [SerializeField]
    float lerpSpeed = .01f;
    [SerializeField]
    float glitchLerpSpeed = .5f;
    [SerializeField]
    float glitchDistance = 1;
	// Use this for initialization
	void Start () {
        myTransform = transform;
        currentNonGlitchPosition = transform.position;
        glitchOffset = Vector3.zero;
        ((GameManager)(GameObject.Find("GameManager").GetComponent(typeof(GameManager)))).ItemBought += (id) =>
        {
            if (id == "screen-shake")
            {
                StartCoroutine(CameraShake());
            }
        };
    }
	
	// Update is called once per frame
	void Update () {
        if (glitching && (DateTime.Now - glitchStart).Milliseconds > glitchDuration)
            glitching = false;
        currentNonGlitchPosition = Vector3.Lerp(myTransform.position, target, lerpSpeed);
        myTransform.position = Vector3.Lerp(myTransform.position, (glitching) ? currentNonGlitchPosition + glitchOffset : currentNonGlitchPosition, glitchLerpSpeed);
	}

    IEnumerator CameraShake()
    {
        glitching = true;
        glitchOffset = (UnityEngine.Random.insideUnitCircle * glitchDistance);
        while (glitching)
        {
            yield return null;
            if (myTransform.position == currentNonGlitchPosition + glitchOffset)
                glitchOffset = (UnityEngine.Random.insideUnitCircle * glitchDistance);
        }
    }
}