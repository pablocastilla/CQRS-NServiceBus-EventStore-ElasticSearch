//ProjectionName: TotalMoneyInTheBank
//   /projection/TotalMoneyInTheBank/state

fromAll()
.when({
    $init: function () {
        return { TotalMoney: 0 }; // initial state
    },

    MoneyDeposited: function (s, e) {

        s.TotalMoney += e.data.Quantity;
        return s;
    },

    MoneyWithdrawn: function (s, e) {

        s.TotalMoney -= e.data.Quantity;
        return s;
    },



});
