using UnityEngine;

namespace Elevator
{
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

        [SerializeField, Range(20f, 50f)]
        private float maxAltitude = 30f;

        [SerializeField, Range(0f, 15f)]
        private float minAltitude = 0f;

        private void Start()
        {
            upAction.OnPerformed.AddListener(OnUp);
            downAction.OnPerformed.AddListener(OnDown);
        }

        private void OnUp()
        {
            var y = camTransform.position.y;
            camTransform.position = new Vector3(0, y + metresPerSecond * Time.deltaTime, 0);
            if (camTransform.position.y > maxAltitude)
                camTransform.position = new Vector3(0, maxAltitude, 0);
        }

        private void OnDown()
        {
            var y = camTransform.position.y;
            camTransform.position = new Vector3(0, y - metresPerSecond * Time.deltaTime, 0);
            if (camTransform.position.y < minAltitude)
                camTransform.position = Vector3.zero;
        }
    }
}
