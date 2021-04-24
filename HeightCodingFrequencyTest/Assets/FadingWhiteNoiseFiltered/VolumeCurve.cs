using UnityEngine;

namespace FadingWhiteNoiseFiltered
{
    [RequireComponent(typeof(AudioSource))]
    public class VolumeCurve : MonoBehaviour
    {
        const float MT_EVEREST = 8848f;

        [SerializeField]
        internal AnimationCurve altitudeToVolumeCurve = new AnimationCurve(
                new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f)
            );

        [SerializeField, Range(0f, MT_EVEREST)]
        internal float minAltitude = 0f;

        [SerializeField, Range(0f, MT_EVEREST)]
        internal float maxAltitude = 100f;

        [SerializeField, Tooltip("Whether to use world or local space position.")]
        internal bool useLocalPosition = false;

        private float range = 0f;
        private float lastSeenY = -1f;
        private AudioSource audioSource;
        [SerializeField, HideInInspector]
        private float volumeFactor = 1f;

        private void OnValidate()
        {
            if (maxAltitude <= minAltitude)
                maxAltitude = minAltitude + 10f;
            volumeFactor = GetComponent<AudioSource>().volume;
        }

        private void OnEnable()
        {
            range = maxAltitude - minAltitude;
            audioSource = GetComponent<AudioSource>();

            if (range > 0f)
            {
                lastSeenY = useLocalPosition ? transform.localPosition.y : transform.position.y;
                UpdateForAltitudeChange(updateOutsideOfRange:true);
            }
            else
            {
                // no range, no volume change!
                enabled = false;
                UpdateVolume(0f);
            }
        }

        private void Update()
        {
            float y = useLocalPosition ? transform.localPosition.y : transform.position.y;
            if (y != lastSeenY)
            {
                lastSeenY = y;
                UpdateForAltitudeChange();
            }
        }

        private void UpdateForAltitudeChange(bool updateOutsideOfRange = false)
        {
            float percent = (lastSeenY - minAltitude) / range;
            if (percent > 1f || percent < 0f)
            {
                if (!updateOutsideOfRange)
                    return;
            }
            percent = Mathf.Min(1f, Mathf.Max(0f, percent));
            UpdateVolume(percent);
        }

        private void UpdateVolume(float percent)
        {
            float val = altitudeToVolumeCurve.Evaluate(percent);
            val = Mathf.Min(1f, Mathf.Max(0f, val));
            audioSource.volume = val * volumeFactor;
        }
    }
}
