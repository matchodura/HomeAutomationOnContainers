using HomeControl.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeControl.Domain.AggregatesModel.RoomAggregate
{
    public class DeviceAddress : ValueObject
    {
        public DeviceAddress(string name, string iP, string topic, string mosquittoUsername, string mosquittoPassword)
        {
            Name = name;
            IP = iP;
            Topic = topic;
            MosquittoUsername = mosquittoUsername;
            MosquittoPassword = mosquittoPassword;
        }

        public string Name { get; private set; }
        public string IP { get; private set; }
        public string Topic { get; private set; }
        public string MosquittoUsername { get; private set; }
        public string MosquittoPassword { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return IP;
            yield return Topic;
            yield return MosquittoUsername;
            yield return MosquittoPassword;
        }
    }
}
