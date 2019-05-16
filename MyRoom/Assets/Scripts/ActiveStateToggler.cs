using UnityEngine;
using System.Collections;

public class ActiveStateToggler : MonoBehaviour {

    bool state;
    

	public void ToggleActive () {
		gameObject.SetActive (!gameObject.activeSelf);
	}

    public void PressDown()
    {
        state = gameObject.activeSelf;

        if (state  == true)
        {
            gameObject.SetActive(false);
        }
    }

    public void PressUp()
    {
        if(state)
        {
            gameObject.SetActive(true);
        }
     
    }
}
