using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField]
    private Transform camTransform;

    [SerializeField]
    private InputToAction upAction;
    [SerializeField]
    private InputToAction downAction;

    [SerializeField, Range(1f, 20f)]
    private float metresPerSecond = 1f;

    private void Start()
    {
        upAction.OnPerformed.AddListener(OnUp);
        downAction.OnPerformed.AddListener(OnDown);
    }

    private void OnUp()
    {
        var y = camTransform.transform.position.y;
        camTransform.transform.position = new Vector3(0, y + metresPerSecond * Time.deltaTime, 0);
    }

    private void OnDown()
    {
        var y = camTransform.transform.position.y;
        camTransform.transform.position = new Vector3(0, y - metresPerSecond * Time.deltaTime, 0);
        if (camTransform.position.y < 0)
            camTransform.position = Vector3.zero;
    }
}
