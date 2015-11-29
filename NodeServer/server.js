var io 				= require('socket.io')(2558);
var shortId 		= require('shortid');


var clients			= [];

io.on('connection', function (socket) {
	
	var currentUser;

	socket.on('USER_CONNECT', function (){

		console.log('Users Connected ');
		for (var i = 0; i < clients.length; i++) {
			
			
			socket.emit('USER_CONNECTED',{

				name:clients[i].name,
				id:clients[i].id,
				position:clients[i].position

			});

			console.log('User name '+clients[i].name+' is connected..');

		};

	});

	socket.on('PLAY', function (data){
		currentUser = {
			name:data.name,
			id:shortId.generate(),
			position:data.position
		}

		clients.push(currentUser);
		socket.emit('PLAY',currentUser );
		socket.broadcast.emit('USER_CONNECTED',currentUser);

	});

	socket.on('disconnect', function (){

		socket.broadcast.emit('USER_DISCONNECTED',currentUser);
		for (var i = 0; i < clients.length; i++) {
			if (clients[i].name === currentUser.name && clients[i].id === currentUser.id) {

				console.log("User "+clients[i].name+" id: "+clients[i].id+" has disconnected");
				clients.splice(i,1);

			};
		};

	});

	socket.on('MOVE', function (data){

		// currentUser.name = data.name;
		// currentUser.id   = data.id;
		currentUser.position = data.position;

		socket.broadcast.emit('MOVE', currentUser);
		console.log(currentUser.name+" Move to "+currentUser.position);


	});


});


console.log("------- server is running -------");