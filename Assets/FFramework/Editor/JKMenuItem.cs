using UnityEditor;
using UnityEngine;
public class JKMenuItem
{
    /// <summary>
    /// 打开存档文件夹
    /// </summary>
    [MenuItem("JKFramework/Open save director")]
    public static void OpenArchiveDirPath()
    {
        EditorUtility.RevealInFinder(Application.persistentDataPath);
    }
    /// <summary>
    /// 打开框架文档
    /// </summary>
    [MenuItem("JKFramework/Open document")]
    public static void OpenFrameDocument()
    {
        Application.OpenURL("http://www.yfjoker.com");
    }
    
}
