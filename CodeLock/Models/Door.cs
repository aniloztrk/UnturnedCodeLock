using System.Xml.Serialization;

namespace CodeLock.Models
{
    public class Door
    {
        [XmlAttribute]
        public int InstanceId;
        [XmlAttribute]
        public ulong Owner;
        [XmlAttribute]
        public uint Password;
        public Door() { }
        public Door(int instanceId, ulong owner, uint password)
        {
            InstanceId = instanceId;
            Password = password;
            Owner = owner;
        }
    }
}
