using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/*
     * AchievementEnumGenerator.cs
     * Created by: Evan Robertson
     * Date Created: 2025-03-26
     * 
     * Description: This editor script is used in auto-generating an enum containing all achievements for easy indexing
     * 
     * Last Changed by: Evan Robertson
     * Last Date Changed: 2025-03-26
     *
     *  -> 1.0 - Created AchievementEnumGenerator.cs
     *   v1.0
     */
[CustomEditor(typeof(AchievementManager))]
public class AchievementEnumGenerator : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AchievementManager manager = (AchievementManager)target;

        if (GUILayout.Button("Generate Achievement Enum"))
        {
            GenerateEnum(manager);
        }
    }

    private void GenerateEnum(AchievementManager manager)
    {
        string path = "Assets/Scripts/Achievements/AchievementID.cs";

        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine("// Auto-generated AchievementID Enum");
            writer.WriteLine("public enum AchievementID");
            writer.WriteLine("{");

            foreach (var achievement in manager.achievements)
            {
                string trimmedName = achievement.title.Replace(" ", "").Replace("-", "_");
                writer.WriteLine($"    {trimmedName},");
            }

            writer.WriteLine("}");
        }

        AssetDatabase.Refresh();
        Debug.Log("AchievementID enum generated successfully!");
    }
}
