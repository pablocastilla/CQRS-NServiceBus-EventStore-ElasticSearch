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
        public string SerialNumber;
        public List<LoadProfileRead> LoadProfile;
        public MeterState state;
       
        public Meter()
        {
            RegisterTransition<MeterCreated>(Apply);
            RegisterTransition<LoadProfileReceived>(Apply);
        }

        private Meter(Guid id, string SerialNumber) : this()
        {
            RaiseEvent(new MeterCreated(id, SerialNumber));
           
        }

        public static Meter Create(Guid id, string serialNumber)
        {
            return new Meter(id, serialNumber);
        }

        public void AddLoadProfile(LoadProfileReceived obj)
        {

            var lpReceivedEvent = new LoadProfileReceived();
            lpReceivedEvent.LPReads = new List<LoadProfileRead>();

            lpReceivedEvent.Id = this.Id;
            lpReceivedEvent.LPReads.AddRange(obj.LPReads);


            RaiseEvent(lpReceivedEvent);
        }

        public void ChangeMeterState(MeterState state)
        {
            var meterStateChanged = new MeterStateChanged() 
            {
                Id=this.Id,
                State = state
            };

            RaiseEvent(meterStateChanged);
        }

        private void Apply(MeterCreated obj)
        {
            Id = obj.Id;
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
