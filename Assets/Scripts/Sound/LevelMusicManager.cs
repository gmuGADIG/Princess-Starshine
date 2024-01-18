using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusicManager : MonoBehaviour {
    [SerializeField] string levelSong;
    AudioSource audioSource;

    const string deathSong = "DeathMusic";

    private void Start() {
        PlayLevelSong();

        if (Player.instance != null) {
            Player.instance.GetComponent<PlayerHealth>().PlayerDied += () => {
                audioSource.Stop();
                audioSource = SoundManager.Instance.PlaySoundGlobal(deathSong);
            };
        }
    }

    public void PlayLevelSong() {
        audioSource = SoundManager.Instance.PlaySoundGlobal(levelSong);
    }
}
