
using System.Collections.Generic;
using System.Linq;
using ArabyAds.Framework;
using ProtoBuf;


namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Integration
{

    [ProtoContract]
    public class BaseEventDataCustom
    {

    }
    /// <summary>
    /// event Broker args Base type
    /// this class is inherited from System.EventArgs
    /// contains Generic Data and the Event Name
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ProtoContract]
    public class EventArgsBaseCustom//<T> : System.EventArgs
    //where T : class, new()
    {
        //public new static read-only EventArgsBase<T> Empty = new EventArgsBase<T>();

        //public T Data { get; set; }
       [ProtoMember(1)]
        public string EventName { get; set; }
       [ProtoMember(2)]
        public string InstanceId { get; set; }
       [ProtoMember(3)]
        public IList<BaseEventDataCustom> Data { get; set; }

       //[ProtoMember(4)]
        public IList<ValueMessageWrapper<Dictionary<string, object>>> ExtraParameters { get; set; }

        public EventArgsBaseCustom()
        {
            Data = null;
            ExtraParameters = null;
            EventName = string.Empty;
        }

        //public EventArgsBase(T data, string eventName)
        public EventArgsBaseCustom(string eventName)
        {
            EventName = eventName;
        }
        public EventArgsBaseCustom(string eventName, IList<BaseEventDataCustom> data, IList<Dictionary<string, object>> extraParameters)
        {
            Data = data;
            EventName = eventName;
            ExtraParameters = extraParameters?.Select(x => ValueMessageWrapper.Create(x)).ToList();
        }
        public EventArgsBaseCustom(string eventName, string instanceId)
        {
            EventName = eventName;
            InstanceId = instanceId;
        }
        public EventArgsBaseCustom(string eventName, string instanceId, IList<BaseEventDataCustom> data, IList<Dictionary<string, object>> extraParameters)
        {
            Data = data;
            EventName = eventName;
            InstanceId = instanceId;
            ExtraParameters = extraParameters?.Select(x => ValueMessageWrapper.Create(x)).ToList();
        }
    }

}
