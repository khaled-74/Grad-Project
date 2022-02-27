using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    #region 
    [Header("Functional Options")]
    [SerializeField] private bool canInteract = true;

    [Header("Controls")]
    [SerializeField] private KeyCode interactionKey = KeyCode.E;

    [Header("Interaction Parameters")]
    [SerializeField] private Vector3 interactionRayPoint = default;
    [SerializeField] private float interactionDistance = default;
    [SerializeField] private LayerMask interactableLayer = 8;
    private Interactable currentInteractable;

    private Camera playerCamera;

    private void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
    }
    // Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        if (canInteract)
        {
            HandleInteractionCheck();//constantly raycasts looking 4 interactable obj
            HandleInteractionInput();//the interaction func
        }
    }

    private void HandleInteractionCheck()
    {
        RaycastHit hitInfo;
        Ray ray = playerCamera.ViewportPointToRay(interactionRayPoint);

        if (Physics.Raycast(ray, out hitInfo, interactionDistance))
        {
            Debug.DrawLine(ray.origin, hitInfo.point);

            if (hitInfo.collider.gameObject.layer == 8 && (currentInteractable == null || hitInfo.collider.gameObject.GetInstanceID() != currentInteractable.GetInstanceID()))
            {
                hitInfo.collider.TryGetComponent(out currentInteractable);

                if (currentInteractable)
                    currentInteractable.OnFocus();
            }
        }
        else if (currentInteractable)
        {
            currentInteractable.OnLoseFocus();
            currentInteractable = null;
        }
    }
    private void HandleInteractionInput()
    {
        Ray ray = playerCamera.ViewportPointToRay(interactionRayPoint);

        if (Input.GetKeyDown(interactionKey) && currentInteractable != null && Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactableLayer))
        {
            currentInteractable.OnInteract();//the interaction
        }
    }
    #endregion
}
