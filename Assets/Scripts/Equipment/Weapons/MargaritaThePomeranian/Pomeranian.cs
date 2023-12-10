using System;
using System.Linq;
using UnityEngine;

interface IPomeranianState {
    public const float fuzzyDistance = .25f;
    void Update(Pomeranian pomeranian);
    IPomeranianState NewState(Pomeranian pomeranian);
}

class Waiting : IPomeranianState {
    float duration;
    IPomeranianState nextState;

    public Waiting(float duration, IPomeranianState nextState) {
        this.duration = duration;
        this.nextState = nextState;
    }

    public void Update(Pomeranian _pomeranian) {
        duration -= Time.deltaTime;
    }

    public IPomeranianState NewState(Pomeranian _pomeranian) {
        if (duration <= 0) {
            return nextState;
        } else {
            return this;
        }
    }
}

class MoveToEnemy : IPomeranianState {
    GameObject enemy;

    public MoveToEnemy(GameObject previousEnemy) {
        var enemies = ProjectileWeapon.getEnemies()
            .Where(e => e != previousEnemy)
            .Where(e => TeaTime.pointInCameraBoundingBox(e.transform.position, .5f, .5f))
            .ToArray();

        if (enemies.Length != 0) {
            enemy = enemies[UnityEngine.Random.Range(0, enemies.Length)].gameObject;
        } else { // abusively use the player as the target if there are no enemies
            enemy = Player.instance.gameObject;
        }
    }

    public IPomeranianState NewState(Pomeranian pomeranian) {
        if (
            enemy == null ||
            Vector3.Distance(
                pomeranian.transform.position, 
                enemy.transform.position
            ) < IPomeranianState.fuzzyDistance
        ) {
            return new Waiting(pomeranian.PauseTime, new MoveToEnemy(enemy));
        } else {
            return this;
        }
    }

    public void Update(Pomeranian pomeranian) {
        if (enemy != null) {
            var step = pomeranian.Speed * Time.deltaTime;
            pomeranian.transform.position = Vector3.MoveTowards(
                pomeranian.transform.position, 
                enemy.transform.position,
                step
            );
        }
    }
}

class TwirlReaction : IPomeranianState {
    float duration = 0f;
    Vector3 velocity;
    public TwirlReaction(Pomeranian pomeranian) {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        velocity = input * Player.instance.twirlSpeed;
        // initialPosition = pomeranian.transform.position;
        // target = Player.instance.transform.position + (Vector3)(Player.instance.velocity * Player.instance.twirlDuration);
    }

    public IPomeranianState NewState(Pomeranian pomeranian) {
        if (duration >= Player.instance.twirlDuration) {
            return new MoveToEnemy(null);
        } else {
            return this;
        }
    }

    public void Update(Pomeranian pomeranian) {
        duration += Time.deltaTime;
        pomeranian.transform.position += velocity * Time.deltaTime;
        // pomeranian.transform.position = Vector3.Lerp(
        //     initialPosition,
        //     target,
        //     duration / Player.instance.twirlDuration
        // );
    }
}

public class Pomeranian : MonoBehaviour
{
    [HideInInspector]
    public float Speed = 3f;
    public float PauseTime = .5f;

    IPomeranianState state;

    // Start is called before the first frame update
    void Start()
    {
        state = new MoveToEnemy(null);

        Player.instance.PlayerTwirled += () => {
            state = new TwirlReaction(this);
        };
    }

    // Update is called once per frame
    void Update()
    {
        state.Update(this);
        state = state.NewState(this);
    }
}
