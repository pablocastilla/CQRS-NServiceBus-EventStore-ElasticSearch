



uiAngularServices.service('SignalRHubService', function ($rootScope) {
    var proxy = null;
    


    var initialize = function () {
        //Getting the connection object
        connection = $.hubConnection("./signalr", { useDefaultPath: false });

        //Creating proxy
        this.proxy = connection.createHubProxy('NonPersistentHub');

        //Starting connection
        connection.start();

       
        this.proxy.on('clientPossiblyStolen', function (msg) {
           
           
            $rootScope.$emit('clientPossiblyStolen', { message: msg });
            
           
        });
    };

    
    return {
        initialize: initialize
       
    };
});
