using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{

    protected InteractableState state = InteractableState.IDLE;

    public InteractableState State { get => state; }

    protected abstract void GetInteracted();

    public enum InteractableState { IDLE, INTERACTING }
}
