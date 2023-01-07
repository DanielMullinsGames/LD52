using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCursor : ManagedBehaviour
{
    public static PlayerCursor instance;

    public Interactable CurrentInteractable => currentInteractable;
    private Interactable currentInteractable;
    private Interactable cursorDownInteractable;

    public System.Action<Interactable> CurrentInteractableEntered;
    public System.Action<Interactable> CurrentInteractableExited;

    public ReferenceSetToggle DisableInput = new ReferenceSetToggle();
    private bool inputWasDisabled = false;

    [SerializeField]
    private Camera rayCamera = default;

    private List<string> excludedLayers = new List<string>
    {
        "NonInteractable",
    };

    protected override void ManagedInitialize()
    {
        instance = this;
    }

    public override void ManagedUpdate()
    {
        base.ManagedUpdate();
        UpdateInputDisabled();
        UpdatePosition();
        UpdateMainInput();
        UpdateDragInput();
    }

    public void UpdatePosition()
    {
        Vector2 mousePos = Input.mousePosition;

        float heightModifier = ReferenceResolutionSetter.Instance.ReferenceHeight / (float)Screen.height;
        float widthModifier = ReferenceResolutionSetter.Instance.ReferenceWidth / (float)Screen.width;
        Vector2 screenPoint = new Vector2(mousePos.x * widthModifier, mousePos.y * heightModifier);
        Vector2 cursorPos = rayCamera.ScreenToWorldPoint(screenPoint);
        transform.position = new Vector3(cursorPos.x, cursorPos.y, rayCamera.nearClipPlane);
    }

    private void UpdateInputDisabled()
    {
        if (!inputWasDisabled && DisableInput.True)
        {
            if (currentInteractable != null)
            {
                currentInteractable.CursorExit();
                currentInteractable = null;
            }
        }
        inputWasDisabled = DisableInput.True;
    }

    private void UpdateMainInput()
    {
        currentInteractable = UpdateCurrentInteractable(currentInteractable, excludedLayers.ToArray());

        if (!DisableInput.True)
        {
            if (currentInteractable != null)
            {
                currentInteractable.CursorStay();
            }

            if (currentInteractable != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    currentInteractable.CursorSelectStart();
                    cursorDownInteractable = currentInteractable;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    currentInteractable.CursorSelectEnd();
                }
            }
        }
    }

    private void UpdateDragInput()
    {
        if (cursorDownInteractable != null && !DisableInput.True)
        {
            if (currentInteractable != cursorDownInteractable)
            {
                cursorDownInteractable.CursorDragOff();
                cursorDownInteractable = null;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            cursorDownInteractable = null;
        }
    }

    private Interactable UpdateCurrentInteractable(Interactable current, string[] excludeLayers)
    {
        var hitInteractable = RaycastForInteractable(~LayerMask.GetMask(excludeLayers), transform.position);

        if (hitInteractable != current)
        {
            if (current != null)
            {
                if (current.CollisionEnabled)
                {
                    CurrentInteractableExited?.Invoke(current);
                    current.CursorExit();
                }
            }

            if (hitInteractable != null && !DisableInput.True)
            {
                CurrentInteractableEntered?.Invoke(hitInteractable);
                hitInteractable.CursorEnter();
            }
            else
            {
                return null;
            }
        }

        return hitInteractable;
    }

    private Interactable RaycastForInteractable(int layerMask, Vector3 cursorPosition)
    {
        Interactable hitInteractable = null;

        var rayHits = Physics2D.RaycastAll(cursorPosition, Vector2.zero, 1000f, layerMask);
        if (rayHits.Length > 0)
        {
            var hitInteractables = GetInteractablesFromRayHits(rayHits);
            if (hitInteractables.Count > 0)
            {
                hitInteractables.Sort((Interactable2D a, Interactable2D b) =>
                {
                    return a.CompareInteractionSortOrder(b);
                });
                hitInteractable = hitInteractables[0];
            }
        }
        return hitInteractable;
    }

    private List<Interactable2D> GetInteractablesFromRayHits(RaycastHit2D[] rayHits)
    {
        var hitInteractables = new List<Interactable2D>();
        for (int i = 0; i < rayHits.Length; i++)
        {
            var interactable = rayHits[i].transform.GetComponent<Interactable2D>();
            if (interactable != null)
            {
                hitInteractables.Add(interactable);
            }
        }
        return hitInteractables;
    }
}
