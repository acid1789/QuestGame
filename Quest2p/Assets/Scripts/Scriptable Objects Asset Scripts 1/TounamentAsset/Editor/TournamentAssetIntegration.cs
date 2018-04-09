using UnityEngine;
using UnityEditor;

static class TournamentAssetIntegration {

	[MenuItem("Assets/Create/TournamentAsset")]
	public static void CreateYourScriptableObject() {
		ScriptableObjectUtility2.CreateAsset<TournamentAsset>();
	}

}
