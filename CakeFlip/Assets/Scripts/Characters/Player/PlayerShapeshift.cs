using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShapeshift : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    private InputSystem_Actions.PlayerActions playerActions;

    public bool IsSkateboard = false;
    private Shapeshiftable currentForm;
    [SerializeField] private CapsuleCollider myCapsuleCollider;

    [Header("Guy References")]
    [SerializeField] private GameObject myGuy;
    [SerializeField] private Guy guy;
    [SerializeField] private Animator guyAnimator;

    [Header("Skateboard References")]
    [SerializeField] private GameObject mySkateboard;
    [SerializeField] private Skateboard skateboard;
    [SerializeField] private Animator skateboardAnimator;

    public static event Action<Vector3> CameraOffsetChanged;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        playerActions = inputActions.Player;
    }

    private void OnEnable()
    {
        playerActions.Enable();
        playerActions.Shapeshift.performed += ShapeshiftOnPress;
    }

    private void OnDisable()
    {
        playerActions.Shapeshift.performed -= ShapeshiftOnPress;
        playerActions.Disable();
    }

    private void Start()
    {
        SetupReferences();

        //you must be swift as a raging river
        BecomeMan();
    }

    private void SetupReferences()
    {
        if (guy == null)
        {
            guy = FindAnyObjectByType<Guy>();
        }
        if (myGuy == null)
        {
            myGuy = guy.gameObject;
        }
        if (skateboard == null)
        {
            //can't leave home without me trusty skateboard
            skateboard = FindAnyObjectByType<Skateboard>();
        }
        if (mySkateboard == null)
        {
            mySkateboard = skateboard.gameObject;
        }
        if (guyAnimator == null)
        {
            guyAnimator = myGuy.GetComponent<Animator>();
        }
        if (skateboardAnimator == null)
        {
            skateboardAnimator = mySkateboard.GetComponent<Animator>();
        }
        if (myCapsuleCollider == null)
        {
            myCapsuleCollider = GetComponent<CapsuleCollider>();
        }
    }

    public void SetAnimationTrigger(string animationTrigger)
    {
        if (IsSkateboard)
        {
            skateboardAnimator.SetTrigger(animationTrigger);
        }
        else
        {
            guyAnimator.SetTrigger(animationTrigger);
        }
    }


    private void ShapeshiftOnPress(InputAction.CallbackContext context)
    {
        Shapeshift(!IsSkateboard);
    }

    public void Shapeshift(bool toSkateboard)
    {
        IsSkateboard = toSkateboard;

        mySkateboard.SetActive(toSkateboard);
        myGuy.SetActive(!toSkateboard);

        if (toSkateboard)
        {
            currentForm = skateboard;
        }
        else
        {
            currentForm = guy;
        }

        myCapsuleCollider.height = currentForm.ColliderHeight;
        myCapsuleCollider.radius = currentForm.ColliderRadius;
        myCapsuleCollider.center = currentForm.ColliderCenterValues;
        myCapsuleCollider.direction = (int)currentForm.Direction;

        CameraOffsetChanged?.Invoke(currentForm.CameraOffset);
    }

    public void BecomeSkateboard()
    {
        Shapeshift(true);
    }

    public void BecomeMan()
    {
        Shapeshift(false);
    }

    public void SetAnimationFloat(string animationFloat, float value)
    {
        //TODO both forms
        guyAnimator.SetFloat("Speed_f", value);
    }
}
