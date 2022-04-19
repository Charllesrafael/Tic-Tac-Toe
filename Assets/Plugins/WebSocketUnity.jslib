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

		window.wsclient.onclose = function(evt) {
			console.log("[close] " + evt.code + ":" + evt.reason);
			var data = evt.data;
			SendMessage(nameObject, "Close", "Server Fechou?");
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
	State: function(){
		var status = 0;
		if ((typeof window.wsclient !== "undefined") && (window.wsclient !== null))
			status = window.wsclient.readystate;
		return status;
	},
	Send: function(type, msg){
		var message = Pointer_stringify(msg);
		var typeText = Pointer_stringify(type);
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
	},
	Close: function(nameGameObject){
		if ((typeof window.wsclient !== "undefined") && (window.wsclient !== null)){
			window.wsclient.close(1000);
			var nameObject = Pointer_stringify(nameGameObject);
			SendMessage(nameObject, "Close", "eu fechei ?");
		}
	}
}
mergeInto(LibraryManager.library, WebSocketUnity);