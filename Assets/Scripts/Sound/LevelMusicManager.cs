using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusicManager : MonoBehaviour
{
    [SerializeField] string levelSong;

    private void Start()
    {
        PlayLevelSong();
    }

    public void PlayLevelSong()
    {
        SoundManager.Instance.PlaySoundGlobal(levelSong);
    }
}
