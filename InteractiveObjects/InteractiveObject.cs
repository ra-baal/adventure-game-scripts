using UnityEngine;
using UnityEngine.UI;

public class InteractiveObject : MonoBehaviour
{
    public Sprite UnvisitedSprite;
    public Sprite VisitedSprite;
    public string Text;

    private bool _isVisited;
    private bool isVisited
    {
        set
        {
            this._isVisited = value;

            if (this._isVisited)
            {
                this.interaction = false;
                this.GetComponent<SpriteRenderer>().sprite = this.VisitedSprite;
            }
            else
                this.GetComponent<SpriteRenderer>().sprite = this.UnvisitedSprite;
        }

        get => _isVisited;
    }

    private bool _interaction;
    private bool interaction
    {
        set
        {
            this._interaction = value;

            if (this._interaction)
                this.gameObject.GetComponentInChildren<Text>().text = this.Text; // show a description
            else
                this.gameObject.GetComponentInChildren<Text>().text = ""; // hide a description
        }

        get => _interaction;
    }

    private void Start()
    {
        this.isVisited = false;
        this.interaction = false;
    }

    private void Update()
    {
        if (interaction && Input.GetKeyDown("e"))
        {
            this.isVisited = true;
            this.getProfits();
        }
    }

  /*  private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && !this.isVisited)
            this.interaction = true;
        else
            this.interaction = false;
    }
    */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && !this.isVisited)
            this.interaction = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
            this.interaction = false;
    }

    virtual protected void getProfits()
    {
        Debug.Log(this.gameObject.name + " has been visited.");
    }
}
