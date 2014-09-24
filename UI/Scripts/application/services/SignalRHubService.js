



uiAngularServices.service('SignalRHubService', function ($rootScope) {
    var proxy = null;
    


    var initialize = function () {
        //Getting the connection object
        connection = $.hubConnection("./signalr", { useDefaultPath: false });

        //Creating proxy
        this.proxy = connection.createHubProxy('NonPersistentHub');

        //Starting connection
        connection.start();

       

        //Publishing an event when server pushes a readInsertionFinished message
        this.proxy.on('clientPossiblyStolen', function (msg) {
           
            console.log("Evento Recibido del signalR" + msg);
            $rootScope.$emit('clientPossiblyStolen', { message: msg });
            console.log("Evento propagado");
           
        });
    };

    
    return {
        initialize: initialize
       
    };
});
