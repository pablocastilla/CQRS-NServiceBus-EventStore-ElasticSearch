//ProjectionName: PossiblyStolenCards
///projection/PossiblyStolenCards/state

fromCategory('Bank')
  .foreachStream()
  .when({
      $init: function (state, ev) {
          return {};
      },

      MoneyWithdrawn: function (state, ev) {
          if (state.lastWithdraw) {
              var newDate = new Date(ev.body.TimeStamp)
                , lastDate = new Date(state.lastWithdraw.body.TimeStamp)
                , difference = (newDate.getTime() - lastDate.getTime()) / 1000;

              if (difference < 120) {
                  emit('PossiblyStolenCardClients', "PossiblyStolenCardClient", {
                      ClientID: ev.body.ClientID
                  });
              }
          }

          state.lastWithdraw = ev;
          return state;

      },
  });