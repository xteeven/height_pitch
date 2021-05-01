using UnityEngine;
using UnityEngine.Events;

namespace Elevator
{
    public class ElevatorCameraController : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onUp = new UnityEvent();

        [Header("Accelerate and decelerate curve")]
        [SerializeField, Range(0.5f, 10f)]
        private float upSpeed = 2.4f;

        [SerializeField]
        private AnimationCurve accelerateCurve = new AnimationCurve(
                new Keyframe(0f, 0f), new Keyframe(1f, 1f)
            );

        [SerializeField]
        private AnimationCurve decelerateCurve = new AnimationCurve(
                new Keyframe(0f, 1f), new Keyframe(1f, 0f)
            );

        private enum State
        {
            Idle, Accelerate, GoingUp, Decelerate
        }
        private float currentAnimationDuration = 0f;
        private float animationTime = -1f;
        private State state = State.Idle;

        private float currentUpSpeed = 0f;

        public void OnUpPressed()
        {
            onUp.Invoke();
        }

        public void OnReset()
        {
            transform.position = Vector3.zero;
        }

        private void FixedUpdate()
        {
            Vector3 newPosition = transform.position;
            switch (state)
            {
                case State.Idle:
                    return;
                case State.Accelerate:
                case State.Decelerate:
                    if (animationTime < currentAnimationDuration)
                    {
                        float percent = animationTime / currentAnimationDuration;
                        var animationCurve = state == State.Accelerate ? accelerateCurve : decelerateCurve;
                        currentUpSpeed = animationCurve.Evaluate(percent) * upSpeed;
                        newPosition.y += currentUpSpeed * Time.deltaTime;
                        animationTime += Time.deltaTime;
                    }
                    else
                    {
                        state = State.Idle;
                    }
                    break;
                case State.GoingUp:
                    newPosition.y += currentUpSpeed * Time.deltaTime;
                    break;
            }
            transform.position = newPosition;
        }

        public void OnAudioEffectStartAccelerate(float duration)
        {
            state = State.Accelerate;
            currentAnimationDuration = duration;
            animationTime = 0f;
        }

        public void OnAudioEffectGoingUp()
        {
            animationTime = -1f;
            state = State.GoingUp;
        }

        public void OnAudioEffectDecelerate(float duration)
        {
            state = State.Decelerate;
            currentAnimationDuration = duration;
            animationTime = 0f;
        }

        public void OnAudioEffectStopped()
        {
            animationTime = -1f;
            state = State.Idle;
        }
    }
}
