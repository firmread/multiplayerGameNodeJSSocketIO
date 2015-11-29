using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SocketIO;

public class GameController : MonoBehaviour {


	public LoginPanelController LoginPanel;
	public JoyStickerController JoyStick;
	public SocketIOComponent socketIO;
	public Player	playerGameObj;

	void Start () {
	
		socketIO.On("USER_CONNECTED", OnUserConnected );
		socketIO.On("PLAY", onUserPlay);
		socketIO.On("MOVE", onUserMove);
		socketIO.On("USER_DISCONNECTED", OnUserDisconnected );
		Debug.Log("Game is start");
		JoyStick.gameObject.SetActive(false);
		StartCoroutine( "CalltoServer" );
		LoginPanel.plaBtn.onClick.AddListener(OnClickPlayBtn);
		JoyStick.OnCommandMove += OnCommandMove;
	}

	void OnCommandMove (Vector3 vec3)
	{
		Dictionary<string, string> data = new Dictionary<string, string>();
		Vector3 position = new Vector3( vec3.x,vec3.y,vec3.z );
		data["position"] = position.x+","+position.y+","+position.z;
		socketIO.Emit("MOVE", new JSONObject(data));

	}

	private IEnumerator CalltoServer(){

		yield return new WaitForSeconds(1f);

		Debug.Log("Send message to the server");
		socketIO.Emit("USER_CONNECT");

	}

	void OnClickPlayBtn ()
	{
		if(LoginPanel.inputField.text != ""  ){
	
			Dictionary<string, string> data = new Dictionary<string, string>();
			data["name"] = LoginPanel.inputField.text;
			Vector3 position  = new Vector3(0,0,0);
			data["position"] = position.x+","+position.y+","+position.z;
			socketIO.Emit("PLAY", new JSONObject(data));


		}else{
			LoginPanel.inputField.text = "Please enter your name again ";
		}
	}

	void onUserPlay (SocketIOEvent obj)
	{

		LoginPanel.gameObject.SetActive(false);
		JoyStick.gameObject.SetActive(true);
		JoyStick.ActivejooyStick();

		GameObject player =  GameObject.Instantiate( playerGameObj.gameObject, playerGameObj.position, Quaternion.identity) as GameObject;
		Player playerCom = player.GetComponent<Player>();

		playerCom.playerName = JsonToString( obj.data.GetField("name").ToString(), "\"");
		player.transform.position = JsonToVecter3( JsonToString(obj.data.GetField("position").ToString(), "\"") );
		playerCom.id = JsonToString(obj.data.GetField("id").ToString(), "\"");
		JoyStick.playerObj = player;
	}

	void OnUserDisconnected (SocketIOEvent obj)
	{

		Destroy( GameObject.Find( JsonToString(obj.data.GetField("name").ToString(), "\"")));

	}

	void onUserMove (SocketIOEvent obj)
	{
		GameObject player = GameObject.Find(  JsonToString( obj.data.GetField("name").ToString(), "\"") ) as GameObject;
		player.transform.position =  JsonToVecter3( JsonToString(obj.data.GetField("position").ToString(), "\"") );

	}

	string  JsonToString( string target, string s){

		string[] newString = Regex.Split(target,s);

		return newString[1];

	}

	Vector3 JsonToVecter3(string target ){

		Vector3 newVector;
		string[] newString = Regex.Split(target,",");
		newVector = new Vector3( float.Parse(newString[0]), float.Parse(newString[1]), float.Parse(newString[2]));

		return newVector;

	}

	void OnUserConnected (SocketIOEvent obj)
	{
		Debug.Log( "all user born on this client" );

		GameObject otherPlater =  GameObject.Instantiate( playerGameObj.gameObject, playerGameObj.position, Quaternion.identity ) as GameObject;
		Player otherPlayerCom = otherPlater.GetComponent<Player>();
		otherPlayerCom.playerName = JsonToString(obj.data.GetField("name").ToString(), "\"");
		otherPlater.transform.position =  JsonToVecter3( JsonToString(obj.data.GetField("position").ToString(), "\"") );
		otherPlayerCom.id = JsonToString(obj.data.GetField("id").ToString(), "\"");

	}
}
