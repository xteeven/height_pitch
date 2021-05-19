using UnityEngine;

namespace Kitahara
{
    [RequireComponent(typeof(AudioHighPassFilter))]
    public class HeightHighPass : MonoBehaviour
    {
        const float MT_EVEREST = 8848f;

        [SerializeField]
        internal AnimationCurve altitudeToCutoffCurve = new AnimationCurve(
                new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f)
            );

        [SerializeField, Range(0f, MT_EVEREST)]
        internal float minAltitude = 0f;

        [SerializeField, Range(0f, MT_EVEREST)]
        internal float maxAltitude = 100f;

        [SerializeField, Range(0f, 22000)]
        internal float minHertz = 0f;

        [SerializeField, Range(0f, 22000)]
        internal float maxHertz= 100f;

        private AudioHighPassFilter highPassFilter;

        private float lastSeenY = -1f;

        [SerializeField, HideInInspector]
        private float altitudeRange = 0f;

        [SerializeField, HideInInspector]
        private float hertzRange = 0f;

        private void OnValidate()
        {
            altitudeRange = maxAltitude - minAltitude;
            if (altitudeRange <= 0f)
            {
                maxAltitude = minAltitude + 10f;
                altitudeRange = 10f;
            }
            hertzRange = maxHertz - minHertz;
            if (hertzRange <= 0f)
            {
                hertzRange = minHertz + 10f;
                hertzRange = 10f;
            }
        }

        private void Start()
        {
            highPassFilter = GetComponent<AudioHighPassFilter>();
            UpdateForAltitudeChange(true);
        }

        private void Update()
        {
            float y = transform.position.y;
            if (y != lastSeenY)
            {
                lastSeenY = y;
                UpdateForAltitudeChange();
            }
        }

        private void UpdateForAltitudeChange(bool updateOutsideOfRange = false)
        {
            float percent = (lastSeenY - minAltitude) / altitudeRange;
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
            float val = altitudeToCutoffCurve.Evaluate(percent);
            val = Mathf.Min(1f, Mathf.Max(0f, val));
            float newHz = minHertz + hertzRange * val;
            highPassFilter.cutoffFrequency = newHz;
        }
    }
}
