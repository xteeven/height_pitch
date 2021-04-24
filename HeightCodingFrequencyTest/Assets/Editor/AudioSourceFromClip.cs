using UnityEditor;
using UnityEngine;

namespace FadingWhiteNoiseFiltered
{
	public class AudioSourceFromClip : ScriptableObject
	{
		[MenuItem("Assets/Create/Audio Source in Scene")]
		public static void CreateAudioSource()
		{
			string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
			if (string.IsNullOrEmpty(assetPath))
			{
				Debug.LogWarning("Please select an existing AudioClip in the Project window!");
				return;
			}
			var audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(assetPath);
			if (audioClip == null)
			{
				Debug.LogWarning("Please select an existing AudioClip in the Project window!");
				return;
			}
			var go = new GameObject(audioClip.name);
			var source = go.AddComponent<AudioSource>();
			source.clip = audioClip;
			source.playOnAwake = true;
			source.loop = true;
			source.spatialize = false;
			source.spatialBlend = 0f;
			source.dopplerLevel = 0f;
			go.AddComponent<VolumeCurve>();
			var parent = GameObject.Find("AudioSources");
			if (parent)
            {
				go.transform.parent = parent.transform;
            }
			go.transform.localPosition = Vector3.zero;
			go.transform.localRotation = Quaternion.identity;
		}
	}
}
