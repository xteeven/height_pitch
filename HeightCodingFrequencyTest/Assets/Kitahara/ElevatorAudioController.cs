using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kitahara
{
    /// <summary>
    /// Simulates the sounds of a large elevator in a noisy environment.
    /// </summary>
    public class ElevatorAudioController : MonoBehaviour
    {
        [SerializeField]
        private AudioSource doorsOpenedLoop;
        [SerializeField]
        private AudioSource doorClosing;
        [SerializeField]
        private AudioSource accelerateUp;
        [SerializeField]
        private AudioSource goingUpLoop;
        [SerializeField]
        private AudioSource upDecelerateStop;
        [SerializeField]
        private AudioSource doorsOpenTransitionToLoop;

        private enum State
        {
            DoorsOpen, GoingUp
        }

        private State state;

        private void Start()
        {
            state = State.DoorsOpen;
            doorsOpenedLoop.loop = true;
            doorsOpenedLoop.Play();
        }

        public void OnGoUp()
        {
            if (state == State.DoorsOpen)
                StartCoroutine(DoUpEffects());
        }

        private IEnumerator DoUpEffects()
        {
            state = State.GoingUp;
            var goingUpAudioSources = new List<AudioSource> { doorClosing, accelerateUp, goingUpLoop, upDecelerateStop, doorsOpenTransitionToLoop };
            float remainingLoopTime = doorsOpenedLoop.clip.length - doorsOpenedLoop.time;
            yield return new WaitForSecondsRealtime(remainingLoopTime);
            doorsOpenedLoop.Stop();
            foreach(var audioSource in goingUpAudioSources)
            {
                audioSource.loop = false;
                audioSource.Play();
                yield return new WaitForSecondsRealtime(audioSource.clip.length);
            }
            doorsOpenedLoop.Play();
            state = State.DoorsOpen;
        }
    }
}
