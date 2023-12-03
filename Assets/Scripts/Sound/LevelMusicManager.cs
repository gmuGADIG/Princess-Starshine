using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusicManager : MonoBehaviour
{
    [SerializeField] string levelSong;
    [SerializeField] string bossSong;

    private void Start()
    {
        PlayLevelSong();
    }

    public void PlayLevelSong()
    {
        SoundManager.Instance.PlaySoundGlobal(levelSong);
    }

    public void PlayBossSong()
    {
        SoundManager.Instance.StopPlayingGlobal(levelSong);
        SoundManager.Instance.PlaySoundGlobal(bossSong);
    }

}
