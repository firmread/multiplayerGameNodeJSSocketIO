using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class JoyStickerController : MonoBehaviour {

	public delegate void OnMove( Vector3 vec3 );

	public event OnMove OnCommandMove;


	public CustomButtonEvent Left;
	public CustomButtonEvent Right;
	public CustomButtonEvent Backward;
	public CustomButtonEvent Forward;

	[HideInInspector]
	public GameObject playerObj;

	public bool leftMove;
	public bool rightMove;
	public bool backMove;
	public bool frontMove;

	public void ActivejooyStick(){


		Left.onPress += onPress;
		Right.onPress += onPress;
		Backward.onPress += onPress;
		Forward.onPress += onPress;
	}

	void onPress (GameObject unit, bool state)
	{


		switch( unit.name ){

			case "Left":
					LeftMove(state);
			break;

			case "Right":
					RightMove(state);
			break;

			case "Backward":
					BackMove(state);
			break;

			case "Forward":
					FrontMove(state);
			break;

		}
	}

	private void LeftMove ( bool stat ){
		leftMove = stat;
	}

	private void RightMove ( bool stat ){
		rightMove = stat;
	}

	private void BackMove ( bool stat ){
		backMove = stat;
	}

	private void FrontMove ( bool stat ){
		frontMove = stat;
	}

	void Update(){

		Transform tranf = playerObj.transform;

		if( leftMove ){
			playerObj.transform.position = new Vector3( tranf.position.x-(2f*Time.deltaTime),   tranf.position.y,  tranf.position.z);

			if( OnCommandMove != null ){
				OnCommandMove( playerObj.transform.position );
			}

		}

		if( rightMove ){
			playerObj.transform.position = new Vector3( tranf.position.x+(2f*Time.deltaTime),   tranf.position.y,  tranf.position.z);
			if( OnCommandMove != null ){
				OnCommandMove( playerObj.transform.position );
			}
		}

		if( backMove ){
			playerObj.transform.position = new Vector3( tranf.position.x,   tranf.position.y,  tranf.position.z-(2f*Time.deltaTime));
			if( OnCommandMove != null ){
				OnCommandMove( playerObj.transform.position );
			}
		}

		if( frontMove ){
			playerObj.transform.position = new Vector3( tranf.position.x-(2f*Time.deltaTime),   tranf.position.y,  tranf.position.z+(2f*Time.deltaTime));
			if( OnCommandMove != null ){
				OnCommandMove( playerObj.transform.position );
			}
		}

	}

}
