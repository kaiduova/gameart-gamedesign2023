using Input;
using TMPro;
using UnityEngine;

public class DynamicTutorialText : InputMonoBehaviour {

    public TextMeshProUGUI TutorialText;
    public GameObject GhostHand;
    public GameObject PlayerConfiner1;
    public GameObject Target1;
    public GameObject Target2;

    public bool PlayerMoved;
    public bool PlayerJumped;
    public bool PlayerShot;
    public bool GhostHandActive;
    public bool CrossedWall;

    private void Update() {

        if (CurrentInput.LeftStick.x != 0) PlayerMoved = true;
        if (CurrentInput.GetKeyDownA || CurrentInput.GetKeyDownLT) PlayerJumped = true;

        if (Target1.GetComponent<TutorialTargets>().TargetOrder == 1 && Target1.GetComponent<TutorialTargets>().Hit) {
            PlayerShot = true;
            PlayerConfiner1.SetActive(false);
        }

        if (GhostHand.activeInHierarchy) GhostHandActive = true;

        if (!PlayerMoved|| !PlayerJumped) {
            TutorialText.text = "First of all, let's start with the basics. " +
                "Use the Left Stick to move. Use A or LT to jump.";
            PlayerShot = false;
        }

        if (PlayerJumped && PlayerMoved && !PlayerShot) {
            TutorialText.text = "Great! Now use the Right Stick to aim. " +
                "\nYou will see a prediction line that shows where your projectile will go. " +
                "Use RT to fire and try to hit that target.";
        }

        if (PlayerShot && !Target2.GetComponent<TutorialTargets>().Hit) {
            TutorialText.text = "Good job! Specific objects can interact with your projectile, however they may be out of reach." +
                "\nThankfully, the projectile can bounce off certain objects, these are called Totem blocks." +
                "\nTry to hit the target by bouncing the projectile off the Totems.";
        }

        if (Target2.GetComponent<TutorialTargets>().Hit) {
            TutorialText.text = "Nice Shot! Keep an eye out for suspicious places your projectile might be able to interact with..." +
                "\nThere's more to the Totems too! Hold LB to summon your Ghost Hand!";
        }

        if (GhostHandActive) {
            TutorialText.text = "While hovering over a Totem, hold RB to pick it up, and let go to drop it." +
                "\nHold LB to dismiss the Ghost Hand too. Try to get over this wall with the Totems.";
        }

        if (CrossedWall) {
            TutorialText.text = "That covers the basics! Good luck out there, however do be warned this world isn't safe..." +
                "\nThere are some nasty creatures that will try to get you on sight, have your trigger finger ready!";
        }
    }
}
