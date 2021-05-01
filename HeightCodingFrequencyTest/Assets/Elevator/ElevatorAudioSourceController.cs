using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elevator
{
    /// <summary>
    /// Simulates the sounds of a large elevator in a noisy environment.
    /// </summary>
    public class ElevatorAudioSourceController : MonoBehaviour
    {
        [SerializeField]
        private ElevatorCameraController elevatorCameraController;
        [SerializeField]
        private AudioSource audioSource;

        [Header("Audio Clips")]

        [SerializeField]
        private AudioClip doorsOpenedLoop;
        [SerializeField]
        private AudioClip doorClosing;
        [SerializeField]
        private AudioClip accelerateUp;
        [SerializeField]
        private AudioClip goingUpLoop;
        [SerializeField]
        private AudioClip upDecelerateStop;
        [SerializeField]
        private AudioClip doorsOpenTransitionToLoop;

        private enum State
        {
            DoorsOpen, GoingUp
        }

        private State state;

        private void Start()
        {
            state = State.DoorsOpen;
            audioSource.clip = doorsOpenedLoop;
            audioSource.loop = true;
            audioSource.Play();
        }

        public void OnGoUp()
        {
            if (state == State.DoorsOpen)
                StartCoroutine(DoUpEffects());
        }

        private IEnumerator DoUpEffects()
        {
            Debug.Log("Will be going up shortly!");
            state = State.GoingUp;
            var goingUpAudioClips = new List<AudioClip> { doorClosing, accelerateUp, goingUpLoop, upDecelerateStop, doorsOpenTransitionToLoop };
            yield return new WaitForSecondsRealtime(GetRemainigAudioClipTime());
            audioSource.loop = false;
            foreach(var audioClip in goingUpAudioClips)
            {
                audioSource.clip = audioClip;
                audioSource.Play();
                /// Callback to the ElevatorCameraController so the camera moves in sync with the audio
                if (audioClip == accelerateUp)
                    elevatorCameraController.OnAudioEffectStartAccelerate(audioClip.length);
                else if (audioClip == goingUpLoop)
                    elevatorCameraController.OnAudioEffectGoingUp();
                else if (audioClip == upDecelerateStop)
                    elevatorCameraController.OnAudioEffectDecelerate(audioClip.length);
                else if (audioClip == doorsOpenTransitionToLoop)
                    elevatorCameraController.OnAudioEffectStopped();
                /// wait until clip has played
                yield return new WaitForSecondsRealtime(audioClip.length);
            }
            audioSource.clip = doorsOpenedLoop;
            audioSource.loop = true;
            audioSource.Play();
            state = State.DoorsOpen;
        }

        private float GetRemainigAudioClipTime()
        {
            return audioSource.clip.length - audioSource.time;
        }
    }
}
