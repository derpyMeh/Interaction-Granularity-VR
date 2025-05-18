using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(outroCutsceneController))]
public class outroCutsceneGenerator : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        outroCutsceneController controller = (outroCutsceneController)target;

        if (GUILayout.Button("Auto-Fill Outro Subtitle Cues"))
        {
            var subtitles = new List<(float time, string text)>
            {
                (0f,  "When the gods returned with Gleipnir, Fenrir grew suspicious."),
                (5f,  "It looked too light, too harmless."),
                (9f,  "He refused to be bound unless one of them placed"),
                (11.5f, "their hand in his mouth as a sign of good faith."),
                (15f, "The gods hesitated — only Tyr stepped forward."),
                (20f, "As Gleipnir tightened and Fenrir realized the trap,"),
                (23f, "he bit down, severing Tyr’s hand."),
                (27f, "Now bound, Fenrir was taken to a desolate place."),
                (31f, "The chain was fastened to a massive stone,"),
                (35f, "and a sword was wedged between his jaws to hold them open."),
                (39f, "There he waits, until the day of Ragnarok.")
            };

            var cues = new List<outroCutsceneController.SubtitleCue>();
            foreach (var (time, text) in subtitles)
            {
                cues.Add(new outroCutsceneController.SubtitleCue
                {
                    timestamp = time,
                    subtitle = text
                });
            }

            controller.subtitleCues = cues;
            EditorUtility.SetDirty(controller);
            Debug.Log($"Loaded {cues.Count} subtitle cues.");
        }

        if (GUILayout.Button("Auto-Fill Outro Image Cues"))
        {
            float[] timestamps = new float[]
            {
                0f,//1
                9f,//2
                15f,//3
                20f,//4
                27f,//5
            };

            string basePath = "Assets/Sounds/Voiceover/Images/Outro";
            var imageCues = new List<outroCutsceneController.ImageCue>();

            for (int i = 0; i < timestamps.Length; i++)
            {
                string imageFile = $"img_{i + 1}.png";
                string path = $"{basePath}/{imageFile}";
                Sprite image = AssetDatabase.LoadAssetAtPath<Sprite>(path);

                if (image == null)
                    Debug.LogWarning($"Could not find image at: {path}");

                imageCues.Add(new outroCutsceneController.ImageCue
                {
                    timestamp = timestamps[i],
                    image = image
                });
            }

            controller.imageCues = imageCues;
            EditorUtility.SetDirty(controller);
            Debug.Log($"Loaded {imageCues.Count} image cues.");
        }
    }
}
