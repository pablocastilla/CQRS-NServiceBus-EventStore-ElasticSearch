//    fromAll() 
// fromStream('streamId') | fromStreams(['sream1', 'stream2']) | fromCategory('category')
//NOTE: fromCategory requires $by_category standard projection to be enabled

// .foreachStream() | .partitionBy(function(e) { return e.body.useId; })
fromAll()
.when({
    $init: function () {
        return { TotalMoney: 0 }; // initial state
    },

    AmountDeposited: function (s, e) {

        s.TotalMoney += e.data.Quantity;
        return s;
    },

})
