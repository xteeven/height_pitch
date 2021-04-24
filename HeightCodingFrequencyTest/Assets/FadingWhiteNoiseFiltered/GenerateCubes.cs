using UnityEngine;

namespace FadingWhiteNoiseFiltered
{
    public class GenerateCubes : MonoBehaviour
    {
        [SerializeField]
        private GameObject cloneSource;

        [SerializeField, Header("This is added to the world position of the source for each cloned object.")]
        private Vector3 absolutePosDifference;

        [SerializeField, Header("This is multiplied by the number of created objects for each new object.")]
        private Vector3 relativePosDifference;

        [SerializeField, Range(0, 1000)]
        private int numberOfObjectsToCreate = 0;

        private void Awake()
        {
            for (int i = 1; i <= numberOfObjectsToCreate; i++)
            {
                var pos = cloneSource.transform.position + absolutePosDifference;
                pos += relativePosDifference * i;
                Instantiate(cloneSource, pos, cloneSource.transform.rotation, transform);
            }
        }
    }
}
