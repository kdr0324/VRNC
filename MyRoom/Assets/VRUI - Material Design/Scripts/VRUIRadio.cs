using UnityEngine;
using UnityEngine.UI;

// VRUI Radio Button Component

public class VRUIRadio : UnityEngine.UI.Toggle {

	Color accentColor = Color.white;
	Color outlineColor = Color.white;

	Image outline;
	Image dot;

	protected override void Awake () {

		// Override base method to add listener to set colors on value change

		base.Awake ();

		this.onValueChanged.AddListener(delegate {

			SetRadioColor ();

		});

	}

	public void setColors (Color a, Color b) {

		// Initialize colors from the GUI

		accentColor = a;
	
		outlineColor = b;

		SetRadioColor ();
	}


	void SetRadioColor () 
	{
		// Assign the right color base on isOn state

		if (outline == null)
		{
			outline = transform.Find ("Background/Outline").GetComponent<Image> ();
		}
			
		if (dot == null)
		{
			dot = transform.Find ("Background/Dot").GetComponent<Image> ();
		}

		outline.CrossFadeColor(this.isOn ? accentColor : outlineColor, 0f, true, true);

		dot.CrossFadeColor(this.isOn ? accentColor : new Color(1f, 1f, 1f, 0), 0f, true, true);
	}
}
