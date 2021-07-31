using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialQuest
{
    public bool isStarted { private set; get; }
    public bool isFinished { private set; get; }

    private string startText;
    private Action startingFunction;
    private Func<bool> checkConditionsFunction;
    private Action finishingFunction;

    public TutorialQuest(string startText, Action startingFunction, Func<bool> checkConditionsFunction, Action finishingFunction)
    {
        isStarted = false;
        isFinished = false;

        this.startText = startText;

        this.startingFunction = startingFunction;
        this.checkConditionsFunction = checkConditionsFunction;
        this.finishingFunction = finishingFunction;
    }

    public string Start()
    {
        this.isStarted = true;
        this.startingFunction?.Invoke();

        return this.startText;
    }

    public void Finish()
    {
        this.isFinished = true;
        this.finishingFunction?.Invoke();
    }

    public bool Conditions
    {
        get
        {
            return this.checkConditionsFunction();
        }
    }
}

public class Tutorial : MonoBehaviour
{
    public GameObject ChestPrefab;
    public GameObject PotionPrefab;
    public GameObject OpponentPrefab;

    private TextDisplay textDisplay;
    private Dictionary<string, TutorialQuest> quests;

    private void Start()
    {
        this.textDisplay = FindObjectOfType<TextDisplay>();

        this.quests = new Dictionary<string, TutorialQuest>();

        this.TutorialScript();

        this.startQuest("welcome", 2);
    }

    private void Update()
    {
        foreach (var q in this.quests.Values)
        {
            if (q.isStarted && !q.isFinished && q.Conditions)
            {
                q.Finish();
            }
        }
    }


    private void newQuest(string questName, string startText, Action startFunction, Func<bool> conditionFunction, Action finishFunction)
    {
        this.quests.Add(
            questName,
            new TutorialQuest(startText, startFunction, conditionFunction, finishFunction)
            );
    }

    private void startQuest(string name, float textDuration)
    {
        string startText = this.quests[name].Start();
        this.textDisplay.displayText(startText, textDuration);
    }

    private void showText(string text)
    {
        this.textDisplay.displayText(text, 5);
    }    

    private void TutorialScript()
    {
        welcomeQuest();
        howToMoveQuest();
        jumpQuest();
        pointsQuest();
        rollQuest();
        attackQuest();
        shieldQuest();
        healthQuest();
        fightQuest();
        endQuest();

    }

    private void welcomeQuest()
    {
        float time = 0;

        newQuest("welcome",
                 "Welcome to the tutorial.",
                 () => time = Time.time,
                 () => time + 2f < Time.time,
                 () => startQuest("howToMove", 5) );
    }

    private void howToMoveQuest()
    {
        newQuest("howToMove",
                 "Press A and D keys to move the Hero.\n\nCollect the star on the right.",
                 () => { },
                 () => PlayerPrefs.GetInt("points", 0) == 2,
                 () => startQuest("jump", 5) );
    }

    private void jumpQuest()
    {
        newQuest("jump",
                "If you want to jump, press space.\n\nOpen the chest on the upper platform.",
                () => { Instantiate(ChestPrefab, new Vector2(-6.34f, 1.4f), Quaternion.identity ); },
                () => PlayerPrefs.GetInt("points", 0) == 12,
                () => startQuest("points", 5) );           
    }

    private void pointsQuest()
    {
        float time = 0;

        newQuest("points",
                "Collecting stars and opening chests give you points,\nTheir number is displayed in the upper left corner.",
                () => {time = Time.time; },
                () => time + 5f < Time.time,
                () => startQuest("roll", 10));
    }

    private void rollQuest()
    {
        newQuest("roll",
                "You can roll by pressing the left shift.\n\nTry it!",
                () => { },
                () => Input.GetKeyDown("left shift"),
                () => startQuest("attack", 5));
    }

    private void attackQuest()
    {
        newQuest("attack",
                "Click LEFT MOUSE BUTTON to attack.",
                () => { },
                () => Input.GetMouseButtonDown(0),
                () => startQuest("shield", 6));
    }

    private void shieldQuest()
    {
        newQuest("shield",
                "Very good!\n\nNow click RIGHT MOUSE BUTTON\nto cover yourself with the shield.",
                () => { },
                () => Input.GetMouseButtonDown(1),
                () => startQuest("health", 10));
    }

    private void healthQuest()
    {
        newQuest("health",
                "There is a health bar in the lower right corner.\nWhen it drops to zero, you will lose.\nPotions regenerate your health.\nDrink one of them.",
                () => { GameObject.FindObjectOfType<Player>().CurrentHealth = 40;
                        Instantiate(PotionPrefab, new Vector2(-6f, -0.5f), Quaternion.identity);
                        Instantiate(PotionPrefab, new Vector2(0f, 2.1f), Quaternion.identity); },
                () => GameObject.FindObjectOfType<Player>().CurrentHealth == GameObject.FindObjectOfType<Player>().MaxHealth,
                () => startQuest("fight", 6));
    }

    private void fightQuest()
    {
        newQuest("fight",
                "Let's try our skills in real combat!\nYour opponent is on the right.",
                () => { Instantiate(OpponentPrefab, new Vector2(8.5f, -1f), Quaternion.identity); },
                () => GameObject.FindObjectOfType<Opponent>().CurrentHealth <= 0,
                () => startQuest("end", 6));
    }

    private void endQuest()
    {
        float time = 0;

        newQuest("end",
                "You have completed this tutorial.\n\nYou can start your adventure now!",
                () => { time = Time.time; },
                () => time + 7f < Time.time,
                () => SceneManager.LoadScene("Menu"));
    }

}
