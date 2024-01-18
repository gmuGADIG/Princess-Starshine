using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

interface IHellevatorState {
    void Update(Hellevator hellevator);
    IHellevatorState Transition(Hellevator hellevator);
}

class Sleep : IHellevatorState {
    float duration;
    IHellevatorState next;

    public Sleep(float duration, IHellevatorState next) {
        this.duration = duration;
        this.next = next;
    }

    public void Update(Hellevator _hellevator) {
        duration -= Time.deltaTime;
    }

    public IHellevatorState Transition(Hellevator _hellevator) {
        if (duration <= 0) { 
            return next; 
        } else {
            return this;
        }
    }
}

class Stable : IHellevatorState {
    HellevatorStop stop;
    
    public Stable(HellevatorStop stop, Hellevator hellevator) {
        this.stop = stop;
        
        hellevator.titleAnimator.SetTrigger("FadeIn");
        hellevator.levelTitle.text = stop.DisplayName;
    }

    public void Update(Hellevator _hellevator) {
        if (Input.GetKeyDown(KeyCode.Space)) {
            SceneManager.LoadScene(stop.SceneName);
        }
    }

    public IHellevatorState Transition(Hellevator _hellevator) {
        return this;
    }
}

class Moving : IHellevatorState {
    HellevatorStop goal;

    public Moving(HellevatorStop goal) {
        this.goal = goal;
    }

    public void Update(Hellevator hellevator) {
        hellevator.transform.position = Vector3.MoveTowards(
            hellevator.transform.position, 
            goal.transform.position, 
            hellevator.speed * Time.deltaTime
        );
    }

    public IHellevatorState Transition(Hellevator hellevator) {
        if (hellevator.transform.position == goal.transform.position) {
            return new Stable(goal, hellevator);
        } else {
            return this;
        }
    }
}

public class Hellevator : MonoBehaviour {
    public float speed = 2f;
    public Animator titleAnimator;
    public TMP_Text levelTitle;

    [SerializeField] OptionsMenu optionsMenu;
    [Tooltip("How long the hellevator waits before beginning to move.")]
    [SerializeField] float delay = 0.5f;

    IHellevatorState state;
    bool optionsMenuOpen = false;

    void Start() {
        var nextLevel = SaveManager.SaveData.NextLevel;
        if (nextLevel > 7 || nextLevel < 2) {
            throw new ArgumentException("NextLevel should be between 2 and 7.");
        }

        transform.position = GameObject.Find((nextLevel - 1).ToString()).transform.position;
        var moving = new Moving(
            GameObject.Find(nextLevel.ToString()).GetComponent<HellevatorStop>()
        );

        state = new Sleep(delay, moving);
    }

    public void OpenOptionsMenu() {
        optionsMenuOpen = true;
        optionsMenu.gameObject.SetActive(true);
    }

    public void OnOptionsMenuClose() {
        optionsMenuOpen = false;
    }

    public void GoToMainMenu() {
        SceneManager.LoadScene("TitleScreenScene");
    }

    // Update is called once per frame
    void Update() {
        if (optionsMenuOpen) { return; }

        state.Update(this);
        state = state.Transition(this);
    }
}
