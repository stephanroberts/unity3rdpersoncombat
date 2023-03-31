using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    private State currentState;

    void Start() {

    }

    void Update() {
            currentState?.Tick(Time.deltaTime);
    }
}
