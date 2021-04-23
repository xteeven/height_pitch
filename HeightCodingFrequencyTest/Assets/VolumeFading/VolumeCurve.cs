using UnityEngine;

namespace VolumeFading
{
    [RequireComponent(typeof(AudioSource))]
    public class VolumeCurve : MonoBehaviour
    {
        const float MT_EVEREST = 8848f;

        [SerializeField]
        internal AnimationCurve altitudeToVolumeCurve = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 0f));

        [SerializeField, Range(0f, MT_EVEREST)]
        internal float minAltitude = 0f;

        [SerializeField, Range(0f, MT_EVEREST)]
        internal float maxAltitude = 100f;

        [SerializeField, Tooltip("Whether to use world or local space position.")]
        internal bool useLocalPosition = false;

        private float range = 0f;
        private float lastSeenY = 0f;
        private AudioSource audioSource;
        private float relativeVolume = 1f;

        private void OnValidate()
        {
            if (maxAltitude <= minAltitude)
                maxAltitude = minAltitude + 10f;
            relativeVolume = GetComponent<AudioSource>().volume;
        }

        private void OnEnable()
        {
            range = maxAltitude - minAltitude;
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            float y = useLocalPosition ? transform.localPosition.y : transform.position.y;
            if (y != lastSeenY && range > 0)
            {
                lastSeenY = y;
                float percent = (y - minAltitude) / range;
                if (percent > 1f || percent < 0f)
                    return;
                else
                {
                    float val = altitudeToVolumeCurve.Evaluate(percent);
                    val = Mathf.Min(1f, Mathf.Max(0f, val));
                    audioSource.volume = val * relativeVolume;
                }
            }
        }
    }
}
