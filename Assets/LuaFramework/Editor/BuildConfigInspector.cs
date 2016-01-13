using UnityEngine;
using System.Collections;
using UnityEditor;


namespace SluaAdvanced{

	[CustomEditor(typeof(LuaBuildConfig))]
	public class BuildConfigInspector : Editor {

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();
			if(GUILayout.Button("Build")){
				(target as LuaBuildConfig).Build();
			}
		}
	}
}
