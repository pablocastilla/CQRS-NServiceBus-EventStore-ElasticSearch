using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossCutting.DomainBase;
using Domain.Events;

namespace Domain
{
    public class Meter : AggregateBase
    {
        public override string Id
        {
            get { return this.SerialNumber; }
        }
      
        public string SerialNumber;

        public List<LoadProfileRead> LoadProfile;
        public MeterState state;
       
        public Meter()
        {
            RegisterTransition<MeterCreated>(Apply);
            RegisterTransition<LoadProfileReceived>(Apply);
        }

        private Meter(string SerialNumber) : this()
        {
            RaiseEvent(new MeterCreated(SerialNumber));
           
        }

        public static Meter Create(string serialNumber)
        {
            return new Meter(serialNumber);
        }

        public void AddLoadProfile(LoadProfileReceived obj)
        {

            var lpReceivedEvent = new LoadProfileReceived();
            lpReceivedEvent.LPReads = new List<LoadProfileRead>();

            
            lpReceivedEvent.LPReads.AddRange(obj.LPReads);


            RaiseEvent(lpReceivedEvent);
        }

        public void ChangeMeterState(MeterState state)
        {
            var meterStateChanged = new MeterStateChanged() 
            {                
                State = state
            };

            RaiseEvent(meterStateChanged);
        }

        private void Apply(MeterCreated obj)
        {           
            SerialNumber = obj.SerialNumber;
            state = MeterState.Imported;
            LoadProfile = new List<LoadProfileRead>();
        }

        private void Apply(LoadProfileReceived obj)
        {
            LoadProfile.AddRange(obj.LPReads);
        }

        private void Apply(MeterStateChanged obj)
        {
            state = obj.State;
        }



        public enum MeterState
        {
            Imported,
            Operative
            
        }

       
    }
}
