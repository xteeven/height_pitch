using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class InputToAction : MonoBehaviour
{
    [SerializeField]
    internal InputActionReference actionReference;

    [SerializeField]
    internal UnityEvent OnPerformed = new UnityEvent();

    private void OnEnable()
    {
        actionReference.action.Enable();
    }

    private void Update()
    {
        if (actionReference.action.ReadValue<float>() > 0.5f)
        {
            OnPerformed.Invoke();
        }
    }
    private void OnDisable()
    {
        actionReference.action.Disable();
    }
}
