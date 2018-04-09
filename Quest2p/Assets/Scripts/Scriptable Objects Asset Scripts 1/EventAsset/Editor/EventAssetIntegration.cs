using UnityEngine;
using UnityEditor;

static class EventAssetIntegration {

	[MenuItem("Assets/Create/EventAsset")]
	public static void CreateYourScriptableObject() {
		ScriptableObjectUtility2.CreateAsset<EventAsset>();
	}

}
