using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class TEMP_SoundMaker : MonoBehaviour
{
    [MenuItem("MyTools/Create Sound Resources")]
    static void CreateSoundResources()
    {
        var allSfx = Directory.GetFiles("Assets/Audio/SFX/", "*", SearchOption.AllDirectories);
        foreach (var soundDir in allSfx)
        {
            if (!soundDir.Contains("Voice SFX")) continue;
            if (soundDir.Contains(".meta")) continue;
            var soundAsset = AssetDatabase.LoadAssetAtPath<AudioClip>(soundDir);
            if (soundAsset == null)
            {
                Debug.LogWarning($"no AudioClip found for {soundDir!}");
                continue;
            }

            var voiceSfxIndex = soundAsset.name.IndexOf("_Voice SFX_");
            if (voiceSfxIndex == -1)
            {
                Debug.LogWarning($"Unexpected format for sound {soundAsset.name}!");
                continue;
            }

            var characterName = soundAsset.name[(voiceSfxIndex + 11) ..];
            var descriptorName = soundAsset.name[0 .. voiceSfxIndex]; 
            var finalName = characterName + "_" + descriptorName;

            Sound soundResource = ScriptableObject.CreateInstance<Sound>();
            soundResource.name = finalName;
            soundResource.audioVariants = new[] { soundAsset };
            soundResource.pitchVariance = 0;
            soundResource.volumeVariance = 0;
            
            AssetDatabase.CreateAsset(soundResource, $"Assets/Resources/Sounds/DialogueVoices/{finalName}.asset");
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
