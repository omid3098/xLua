using System.IO;
using UnityEditor.Experimental.AssetImporters;

[ScriptedImporter(1, "lua")]
public class LuaScriptsImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        var text = File.ReadAllText(ctx.assetPath);
        var luaScriptAsset = LuaScript.CreateFromString(text);

        //script name
        var fileName = Path.GetFileNameWithoutExtension(ctx.assetPath);
        luaScriptAsset.name = fileName;

        //script asset
#if UNITY_2017_3_OR_NEWER
        ctx.AddObjectToAsset("script", luaScriptAsset);
        ctx.SetMainObject(luaScriptAsset);
#else
		ctx.SetMainAsset("script", LuaScriptAsset);
#endif

    }
}