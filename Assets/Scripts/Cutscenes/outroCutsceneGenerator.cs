using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(outroCutsceneController))]
public class outroCutsceneGenerator : Editor
{
    public override void OnInspectorGUI() // Override Unity's default inspector GUI
    {
        DrawDefaultInspector(); // Draws all default serialized fields normally

        outroCutsceneController controller = (outroCutsceneController)target;  // Cast the target object to the correct type

        // Button: Automatically fill subtitle cues with pre-written data
        if (GUILayout.Button("Auto-Fill Outro Subtitle Cues"))
        {
            var subtitles = new List<(float time, string text)> // Define a list of subtitle entries with timestamps and text
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

            // Convert the above tuples into actual SubtitleCue objects
            var cues = new List<outroCutsceneController.SubtitleCue>();
            foreach (var (time, text) in subtitles)
            {
                cues.Add(new outroCutsceneController.SubtitleCue
                {
                    timestamp = time,
                    subtitle = text
                });
            }
            // Assign the new cues to the controller and flag the object as modified
            controller.subtitleCues = cues;
            EditorUtility.SetDirty(controller);
            Debug.Log($"Loaded {cues.Count} subtitle cues.");
        }

        // Button: Automatically fill image cues with sprite assets based on timestamps
        if (GUILayout.Button("Auto-Fill Outro Image Cues"))
        {
            // Array of timestamps when each image should appear
            float[] timestamps = new float[]
            {
                0f,// Image 1
                9f,//2
                15f,//3
                20f,//4
                27f,//5
            };

            string basePath = "Assets/Sounds/Voiceover/Images/Outro"; // Folder containing images
            var imageCues = new List<outroCutsceneController.ImageCue>();

            for (int i = 0; i < timestamps.Length; i++) // For each timestamp, load the corresponding image and create an image cue
            {
                string imageFile = $"img_{i + 1}.png"; // Image files named img_1.png, img_2.png, etc.
                string path = $"{basePath}/{imageFile}"; // Construct full path to image
                Sprite image = AssetDatabase.LoadAssetAtPath<Sprite>(path); // Try to load image from path

                if (image == null)
                    Debug.LogWarning($"Could not find image at: {path}"); // Warn if image is missing

                imageCues.Add(new outroCutsceneController.ImageCue
                {
                    timestamp = timestamps[i],
                    image = image
                });
            }

            // Assign the image cue list to the controller and flag changes
            controller.imageCues = imageCues;
            EditorUtility.SetDirty(controller);
            Debug.Log($"Loaded {imageCues.Count} image cues.");
        }
    }
}
