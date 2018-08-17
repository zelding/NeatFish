﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour {

    protected NeuralNet Brain;

    protected Movement Legs;

    public bool Alive { get; protected set; }

    public uint Enegry { get; protected set; }

    public void Awake()
    {
        Movement Legs = new Movement( GetComponent<Rigidbody>(), transform);

        Brain = new NeuralNet(5, 3, true);
    }
    
    public void InheritFrom(EntityManager parent)
    {
        Brain = new NeuralNet(parent.Brain);
    }

    // Use this for initialization
    private void Start () {
		
	}

    // Update is called once per frame
    private void FixedUpdate () {
        
	}
}