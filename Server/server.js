const WebSocket = require('ws')
const wss = new WebSocket.Server(
    { host: 'localhost', port: 8080 },
    () => { console.log('Server started ') }
)
const limitMax = 2
const players = []
const gameGrid = [ '', '', '',
                   '', '', '',
                   '', '', '' ]

const dataClient = {
    type: '',
    message: '',
    grid: gameGrid
}

wss.on('listening', OnListening)

wss.on('close' , OnCloseServer)

wss.on('connection', (ws) => {
    
    ValideLimit(ws)
    OnStartGame(ws)
    
    ws.on('message', OnMessage(ws))
    ws.on('close', OnCloseClient)
})

function OnStartGame(ws) {
    players.push(ws)

    dataClient.type = 'STATE'
    dataClient.message = 'WAIT_OPPONENT'

    if (ws == players[0]) {
        ws.send(JSON.stringify(dataClient))
        return
    }
    
    ws.send(JSON.stringify(dataClient))

    dataClient.message = 'PLAYER_CHOICE'
    players[0].send(JSON.stringify(dataClient))
}

function OnMessage(ws) {
    return (data) => {
        dataReceived = JSON.parse(data)
        
        switch (dataReceived.type) {
            case 'CHOICE':
                OnChoice(ws, dataReceived.message)
                break
            case 'ACTION':
                if (message = 'RESTART')
                    OnRestart()
                break

            default:
                break
        }
    }
}

function OnRestart() {
    
    for (let index = 0; index < gameGrid.length; index++) {
        gameGrid[index] = ''
    }
    dataClient.messages = gameGrid
    dataClient.type = 'RESTART'

    dataClient.message = 'PLAYER_CHOICE'
    players[0].send(JSON.stringify(dataClient))

    dataClient.message = 'WAIT_OPPONENT'
    players[1].send(JSON.stringify(dataClient))
}

function OnChoice(ws, message) {

    let index = parseInt(message)
    let fisrtPlayer = ws == players[0]
    gameGrid[index] = fisrtPlayer ? 'X' : 'O'
    dataClient.grid = gameGrid

    let gameOver = GameLogic(fisrtPlayer)
    if(gameOver)
        return
    
    dataClient.type = 'STATE'
    dataClient.message = fisrtPlayer ? 'WAIT_OPPONENT' : 'PLAYER_CHOICE' 
    players[0].send(JSON.stringify( dataClient ))

    dataClient.message = fisrtPlayer ? 'PLAYER_CHOICE' : 'WAIT_OPPONENT'  
    players[1].send(JSON.stringify( dataClient ))
}

function GameLogic(fisrtPlayer) {
    
    let gameOver = true
    let victory = [ gameGrid[0] != '' && gameGrid[0] == gameGrid[1] && gameGrid[1] == gameGrid[2],
                    gameGrid[3] != '' && gameGrid[3] == gameGrid[4] && gameGrid[4] == gameGrid[5],
                    gameGrid[6] != '' && gameGrid[6] == gameGrid[7] && gameGrid[7] == gameGrid[8],
                    gameGrid[0] != '' && gameGrid[0] == gameGrid[3] && gameGrid[3] == gameGrid[6],
                    gameGrid[1] != '' && gameGrid[1] == gameGrid[4] && gameGrid[4] == gameGrid[7], 
                    gameGrid[2] != '' && gameGrid[2] == gameGrid[5] && gameGrid[5] == gameGrid[8],
                    gameGrid[0] != '' && gameGrid[0] == gameGrid[4] && gameGrid[4] == gameGrid[8],
                    gameGrid[2] != '' && gameGrid[2] == gameGrid[4] && gameGrid[4] == gameGrid[6] ]

    
    if(victory.includes(true))
    {
        dataClient.type = 'RESULT'
        dataClient.message = fisrtPlayer ? 'WIN' : 'LOSE' 
        players[0].send(JSON.stringify( dataClient ))

        dataClient.message = fisrtPlayer ? 'LOSE' : 'WIN'
        players[1].send(JSON.stringify( dataClient ))

        return gameOver
    }

    for (let index = 0; index < gameGrid.length; index++) {
        if(gameGrid[index] == ''){
            gameOver = false
            break
        }
    }
    
    if(!gameOver)
        return gameOver
    
    dataClient.type = 'RESULT'
    dataClient.message = 'DRAW'
    players[0].send(JSON.stringify( dataClient ))
    players[1].send(JSON.stringify( dataClient ))
    return gameOver
    
}


function ValideLimit(ws) {
    console.log('wss.clients.size = ' +wss.clients.size)
    if (wss.clients.size > limitMax) {
        ws.close();
    }
}

function OnCloseClient(ws, req) {
    wss.clients.delete(ws)
    console.log(ws.id + '  desconectou ' + wss.clients.size)
}

function OnListening() {
    console.log('SERVIDOR ESTA OUVINDO PORTA 8080 ')
}

function OnCloseServer() {
    console.log('alguem desconectou')
}