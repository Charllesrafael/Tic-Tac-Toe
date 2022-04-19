//Copyright (c) 2015 Gumpanat Keardkeawfa
//Licensed under the MIT license

//Websocket Jslib for UnityWebgl
//We will save this file as *.jslib for using in UNITY

var WebSocketUnity = {
	InitWebSocket: function(url, nameGameObject){
		var init_url = Pointer_stringify(url);
		var nameObject = Pointer_stringify(nameGameObject);
		window.wsclient = new WebSocket (init_url);
		
		window.wsclient.onopen = function(evt){
			console.log("[open] " + init_url);
			SendMessage(nameObject, "Connected", "");
		};

		window.wsclient.onmessage = function(evt) {
			var data = evt.data;
			console.log("[recv] " + data);
			SendMessage(nameObject, "Message", data);
		};

		window.wsclient.onerror = function(evt) {
			var error_msg = evt.data;
			console.log("[error] ", error_msg);
			var data = evt.data;
			SendMessage(nameObject, "Error", data);
		};
	},
	Send: function(type, msg){
		var typeText = Pointer_stringify(type);
		var message = Pointer_stringify(msg);
		var dataMessage = {
			type: typeText,
			message: message
		};
		if (typeof window.wsclient !== "undefined")
		{
			console.log("[send] " + JSON.stringify(dataMessage));
			window.wsclient.send(JSON.stringify(dataMessage));
		}
		else
		{
			console.log("[send-failed] " + dataMessage);
		}
	}
}
mergeInto(LibraryManager.library, WebSocketUnity);