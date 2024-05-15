require('../node_modules/ws/index')
module.exports = class Event {

    send(ws, EventName, Data) {

        var d = {
            data: Data,
            eventName: EventName
        };
        ws.send(JSON.stringify(d));
        //console.log(JSON.stringify(d))
        
    }

    on(EventName,  Data, handler ) {
        var jso = JSON.parse(Data);
        if (jso.eventName === EventName) {

            //console.log('the event name is : ' + jso.eventName);
            //console.log('and the data is: ' + jso.data);
            var datad = jso.data;
            handler(datad);
            return jso.data;
            
            
        }



    }
    broadcast(ws, wss, eventName, Data) {
        //console.log(clients.length)
        var th = this;
        wss.clients.forEach(function each(client) {
            if (client != ws ) {
                th.send(client,eventName,Data);
            }
          });
    }

    lobbyBroadcast(ws,connections,eventName,Data) {
        var th = this;
        connections.forEach(con => {
            if(con.ws != ws){
                th.send(con.ws,eventName,Data);
            }
        });
    }

}