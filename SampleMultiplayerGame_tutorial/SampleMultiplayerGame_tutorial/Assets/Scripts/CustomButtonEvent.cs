using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class CustomButtonEvent : MonoBehaviour {

	public delegate void OnActionPress( GameObject unit, bool state );

	public event OnActionPress onPress;
	EventTrigger eventTrigger;


	void Start () {

		Debug.Log(this.gameObject.name);
		eventTrigger = this.gameObject.GetComponent<EventTrigger>();
		AddEventTrgger( OnPointDown, EventTriggerType.PointerDown);
		AddEventTrgger(OnPointUp, EventTriggerType.PointerUp);
	}


	void AddEventTrgger( UnityAction action, EventTriggerType triggerType ){

		EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
		trigger.AddListener( (eventData) => action());

		EventTrigger.Entry entry = new EventTrigger.Entry() { callback = trigger, eventID = triggerType };
		eventTrigger.triggers.Add(entry);

	}


	void OnPointDown(){
		if( onPress != null  ){
			Debug.Log("OnPointDown");
			onPress(this.gameObject, true);

		}else{
			Debug.Log("Event null");
		}

	}

	void OnPointUp(){
		if( onPress != null  ){
			Debug.Log("OnPointUp");
			onPress(this.gameObject, false);
			
		}
	}
	

}
