using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributeDialog : MonoBehaviour {

	private Text attribute1;
	private Text attribute2;
	private Text attribute3;

	private Text value1;
	private Text value2;
	private Text value3;

	public enum DialogField{
		Attribute1,
		Attribute2,
		Attribute3,
		Value1,
		Value2,
		Value3
	}

	// Use this for initialization
	void Start () {
		SetVisible(false);
		this.attribute1 = GameObject.Find("label_attribute_1").GetComponent<Text>();
		this.attribute2 = GameObject.Find("label_attribute_2").GetComponent<Text>();
		this.attribute3 = GameObject.Find("label_attribute_3").GetComponent<Text>();

		this.value1 = GameObject.Find("label_attribute_value_1").GetComponent<Text>();
		this.value2 = GameObject.Find("label_attribute_value_2").GetComponent<Text>();
		this.value3 = GameObject.Find("label_attribute_value_3").GetComponent<Text>();
	}
	// Update is called once per frame
	void Update () {
		
	}

	public void SetVisible(bool value){
		this.gameObject.GetComponent<Canvas>().enabled = value;
	}

	public void SetDialogField(DialogField field, string text){
		switch(field){
			case DialogField.Attribute1:
				if(this.attribute1 != null)
					this.attribute1.text = formatAttributeText(text);
				break;
			case DialogField.Attribute2:
				if(this.attribute2 != null)
					this.attribute2.text = formatAttributeText(text);
				break;
			case DialogField.Attribute3:
				if(this.attribute3 != null)
					this.attribute3.text = formatAttributeText(text);
				break;
			
			case DialogField.Value1:
				if(this.value1 != null)
					this.value1.text = text;
				break;
			case DialogField.Value2:
				if(this.value2 != null)
					this.value2.text = text;
				break;
			case DialogField.Value3:
				if(this.value3 != null)
					this.value3.text = text;
				break;

			default:
				break;
		}
	}

	private string formatAttributeText(string text){
		return !text.Contains(":") ? string.Concat(text, ":") : text;
	}

	public string GetDialogField(DialogField field){
		switch(field){
			case DialogField.Attribute1:
				return this.attribute1.text;
				
			case DialogField.Attribute2:
				return this.attribute2.text;
				
			case DialogField.Attribute3:
				return this.attribute3.text;
			
			case DialogField.Value1:
				return this.value1.text;
				
			case DialogField.Value2:
				return this.value2.text;
				
			case DialogField.Value3:
				return this.value3.text;

			default:
				return null;
		}
	}
}
