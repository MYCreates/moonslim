using UnityEngine;

public class Door : MonoBehaviour {

    Animator anim;

    void Start () {
        anim = GetComponent<Animator>();
        anim.enabled = false;
    }

    public void OpenDoor()
    {
        anim.enabled = true;
        anim.SetTrigger("openDoor");
    }
    public void OpenDoor ( Button button )
    {
        anim.enabled = true;
        anim.SetTrigger("openDoor");
        button.Status = Status.DISABLED;
	}
}
