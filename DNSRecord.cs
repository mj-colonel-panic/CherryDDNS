using Newtonsoft.Json;

namespace CherryDDNS
{
    [Serializable]
    public class DNSRecord
    {
        public string data = string.Empty;
        //public int port = 0;
        //public int priority = 1;
        //public string protocol = string.Empty;
        //public string service = string.Empty;
        //public int ttl = 0;
        //public int weight = 0;
        public DNSRecord(string newIPAddress)
        {
            data = newIPAddress;
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static implicit operator DNSRecord(string json)
        {
            return JsonConvert.DeserializeObject<DNSRecord>(json);
        }


    }
}
