using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(introCutsceneController))]
public class introCutsceneGenerator : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        introCutsceneController controller = (introCutsceneController)target;

        if (GUILayout.Button("Auto-Fill Subtitles from Float Timestamps"))
        {
            var subtitles = new List<(float time, string text)>
            {
                (0f, "Loki, the trickster god, had three monstrous children"),
                (4f, "with the giantess Angrboda:"),
                (7f, "Jormungand, the world-serpent;"),
                (10f, "Hel, the ruler of the underworld;"),
                (12f, "and the wolf Fenrir."),
                (14f, "The gods foresaw disaster in their future — and they were right."),
                (18f, "Jormungand would one day poison Thor,"),
                (21f, "Hel would refuse to release the god Baldur from death,"),
                (25f, "and Fenrir… Fenrir would kill Odin himself."),
                (30f, "To delay fate, the gods took action."),
                (33f, "Jormungand they threw into the sea,"),
                (36f, "where he grew so large he encircled the world."),
                (39f, "Hel they sent to the underworld to rule over the dead."),
                (43f, "But Fenrir they kept close."),
                (46f, "Though he terrified them, the gods dared not let him roam freely."),
                (51f, "Instead, they kept him in Asgard, hoping to contain the threat."),
                (56f, "Yet even in their own realm, none dared approach him"),
                (60f, "none but Tyr, the god of justice and courage,"),
                (64f, "who alone was brave enough to feed him."),
                (67f, "Fenrir grew with terrifying speed."),
                (70f, "Knowing they couldn’t control him forever,"),
                (73f, "the gods sought to bind him"),
                (76f, "but they knew he would never agree willingly."),
                (79f, "So they devised a ruse:"),
                (82f, "they told him each chain was merely a test of his strength."),
                (86f, "Fenrir, proud and eager to prove himself,"),
                (90f, "allowed them to try."),
                (92f, "The gods clapped and cheered when he broke each chain,"),
                (95f, "pretending to be impressed"),
                (97f, "but none of their bindings could hold him."),
                (100f, "At last, they turned to the dwarves of Svartalfheim,"),
                (104f, "master smiths of the realms."),
                (107f, "The dwarves forged a chain unlike any other,"),
                (111f, "light and silky but unbreakable."),
                (113f, "It was called Gleipnir, made from six impossible things:"),
                (118f, "the sound of a cat’s footsteps, a woman’s beard, the roots of mountains,"),
                (122f, "a fish’s breath, the spittle of a bird, and the sinews of a bear."),
                (128f, "This is where your journey begins"),
                (131f, "the forging of Gleipnir, the unbreakable chain."),
                (136f, "To succeed, you must find and collect the scattered ingredients"),
                (141f, "and bring it together in the dwarven forge.")
            };

            var cues = new List<introCutsceneController.SubtitleCue>();
            foreach (var (time, text) in subtitles)
            {
                cues.Add(new introCutsceneController.SubtitleCue
                {
                    timestamp = time,
                    subtitle = text
                });
            }

            controller.subtitleCues = cues;
            EditorUtility.SetDirty(controller);
            Debug.Log($"Loaded {cues.Count} subtitle cues.");
        }

        if (GUILayout.Button("Auto-Fill Image Cues"))
        {
            float[] timestamps = new float[]
            {
                0f, //1
                14f, //2
                18f, //3
                30f, //4
                39f, //5
                43f, //6
                56f, //7
                67f, //8
                79f, //9
                92f, //10
                100f, //11
                111f, //12
                128f //13
            };

            string basePath = "Assets/Sounds/Voiceover/Images/Intro";
            var imageCues = new List<introCutsceneController.ImageCue>();

            for (int i = 0; i < timestamps.Length; i++)
            {
                string imageFile = (i >= 12) ? "img_13.png" : $"img_{i + 1}.png";
                string path = $"{basePath}/{imageFile}";
                Sprite image = AssetDatabase.LoadAssetAtPath<Sprite>(path);

                if (image == null)
                    Debug.LogWarning($"Could not find image at: {path}");

                imageCues.Add(new introCutsceneController.ImageCue
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
