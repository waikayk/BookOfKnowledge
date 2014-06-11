using UnityEngine;
using UnityEditor;
using System.Collections;

public class ForceMeshRefresh : Editor {

	[MenuItem("Window/ProBuilder/Actions/Force Refresh Objects")]
	public static void Inuit()
	{
		pb_Object[] all = (pb_Object[])FindObjectsOfType(typeof(pb_Object));
		for(int i = 0; i < all.Length; i++)
		{
			EditorUtility.DisplayProgressBar(
				"Refreshing ProBuilder Objects",
				"Reshaping pb_Object " + all[i].id + ".",
				((float)i / all.Length));

			all[i].MakeUnique();
		}
		EditorUtility.ClearProgressBar();
		
		EditorUtility.DisplayDialog("Refresh ProBuilder Objects", "Successfully refreshed all ProBuilder objects in scene.", "Okay");
	}
}
