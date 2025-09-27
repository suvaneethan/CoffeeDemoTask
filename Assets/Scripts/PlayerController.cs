using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 4f;
    public Vector2 xLimits = new Vector2(-4f, 4f);
    public Vector2 zLimits = new Vector2(-4f, 4f);

    [Header("References")]
    public StackManager stackManager;
    public Transform cupHolder;
    public Renderer highlightRenderer; // assign player mesh in Inspector

    [Header("Highlight Colors")]
    public Color normalColor = Color.white;
    public Color cupColor = Color.green;
    public Color machineColor = Color.yellow;

    [Header("Interaction")]
    public float interactDistance = 3f; // interaction radius

    private CharacterController cc;
    private IInteractable currentInteractable;
    private GameObject carriedCup;

    // Mobile / Joystick input
    private Vector2 moveInput;
    private Vector2 currentInput; // smoothed

    public IInteractable CurrentInteractable => currentInteractable;

    
    void Awake()
    {
        cc = GetComponent<CharacterController>();

        if (highlightRenderer != null)
            highlightRenderer.material.color = normalColor;

      
    }

    void Update()
    {
        HandleMovement();

#if UNITY_EDITOR || UNITY_STANDALONE
        HandlePCInput();
#endif

        //  Cleanup old interactable
        if (currentInteractable != null)
        {
            if (currentInteractable is Object unityObj && unityObj == null)
            {
                currentInteractable = null;
            }
            else
            {
                float dist = Vector3.Distance(
                    transform.position,
                    ((MonoBehaviour)currentInteractable).transform.position
                );
                if (dist > interactDistance)
                {
                    currentInteractable = null;
                }
            }
        }

        //  Scan for cups first (priority)
        Collider[] hits = Physics.OverlapSphere(transform.position, interactDistance);
        IInteractable found = null;
        foreach (var hit in hits)
        {
            var cup = hit.GetComponent<CupPickup>();
            if (cup != null)
            {
                found = cup;
                break;
            }
        }

        //  If no cup, check for other interactables
        if (found == null)
        {
            foreach (var hit in hits)
            {
                var interact = hit.GetComponent<IInteractable>();
                if (interact != null && !(interact is CupPickup))
                {
                    found = interact;
                    break;
                }
            }
        }

        currentInteractable = found;

        //  Visual feedback
        if (highlightRenderer != null)
        {
            if (currentInteractable is CupPickup)
            {
                highlightRenderer.material.color = cupColor;
            }
            else if (currentInteractable != null)
            {
                highlightRenderer.material.color = machineColor;
            }
            else
            {
                highlightRenderer.material.color = normalColor;
            }
        }
    }

    // ---------------- PC Controls ----------------
    void HandlePCInput()
    {
        Vector3 rawInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        if (rawInput.sqrMagnitude > 0.001f)
        {
            moveInput = new Vector2(rawInput.x, rawInput.z);
        }
        else
        {
            moveInput = Vector2.zero;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    // ---------------- Movement ----------------
    void HandleMovement()
    {
        currentInput = Vector2.Lerp(currentInput, moveInput, Time.deltaTime * 10f);

        Vector3 input = new Vector3(currentInput.x, 0f, currentInput.y);
        if (input.sqrMagnitude > 0.001f)
        {
            cc.SimpleMove(input.normalized * moveSpeed);
            transform.forward = input.normalized;
        }
       
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, xLimits.x, xLimits.y);
        pos.z = Mathf.Clamp(pos.z, zLimits.x, zLimits.y);
        transform.position = pos;
    }

    // ---------------- Interact ----------------
    public void TryInteract()
    {
        if (currentInteractable == null) return;

        if (currentInteractable is Object unityObj && unityObj == null)
        {
            currentInteractable = null;
            return;
        }

        currentInteractable.Interact(this);
    }

    // ---------------- Mobile Joystick ----------------
    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    // ---------------- Trigger Detection ----------------
    void OnTriggerEnter(Collider other) => TrySetInteractable(other);
    void OnTriggerStay(Collider other) => TrySetInteractable(other);

    void OnTriggerExit(Collider other)
    {
        var mb = other.GetComponent<MonoBehaviour>();
        var interact = mb as IInteractable;
        if (interact != null && currentInteractable == interact)
        {
            currentInteractable = null;
        }
    }

    private void TrySetInteractable(Collider other)
    {
        var mb = other.GetComponent<MonoBehaviour>();
        var interact = mb as IInteractable;
        if (interact == null) return;

        if (interact is CupPickup)
        {
            currentInteractable = interact;
        }
        else if (!(currentInteractable is CupPickup))
        {
            currentInteractable = interact;
        }
    }

    // ---------------- Cup Methods ----------------
    public bool HasCup() => carriedCup != null;

    public void PickupCup(GameObject cup)
    {
        if (cup == null || carriedCup != null) return;

        carriedCup = cup;
        cup.transform.SetParent(cupHolder, worldPositionStays: true);
        cup.transform.localPosition = Vector3.zero;
        cup.transform.localRotation = Quaternion.identity;

        var rb = cup.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = true;
        var col = cup.GetComponent<Collider>();
        if (col) col.enabled = false;
    }

    public GameObject DropCupAt(Vector3 worldPos)
    {
        if (carriedCup == null) return null;
        var c = carriedCup;
        carriedCup = null;

        c.transform.SetParent(null);
        c.transform.position = worldPos;

        var rb = c.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = false;
        var col = c.GetComponent<Collider>();
        if (col) col.enabled = true;

        return c;
    }
}
