using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LuaScript))]
public class LuaScriptInspector : Editor {
	private const int maxChars = 7000;
	private GUIStyle _textStyle;
	public override void OnInspectorGUI ()
	{
		if (this._textStyle == null) {
			this._textStyle = "ScriptText";
		}
		bool enabled = GUI.enabled;
		GUI.enabled = true;
		LuaScript textAsset = base.target as LuaScript;
		if (textAsset != null) {
			string text;
			if (base.targets.Length > 1) {
				text = "";
			} else {
				text = textAsset.text;
				if (text.Length > maxChars) {
					text = text.Substring (0, maxChars) + @"...";
				}
			}
			Rect rect = GUILayoutUtility.GetRect (new GUIContent (text), this._textStyle);
			rect.x = 0;
			rect.width += 17;
			GUI.Box (rect, text, this._textStyle);
		}
		GUI.enabled = enabled;
	}
}
