using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;

namespace CherryDDNS;

public sealed class DDNSService : BackgroundService
{
    private static readonly string BASE_ADDRESS = "https://api.godaddy.com";
    private static readonly MediaTypeWithQualityHeaderValue CONTENT_TYPE = new MediaTypeWithQualityHeaderValue("application/json");
    private static readonly HttpClient client = new HttpClient() { };

    private string _ip;
    private static DDNSService _instance;
    public static DDNSService Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DDNSService();
            }
            return _instance;
        }
    }

    private DDNSService() 
    {
        VerifyConfig();
        client.DefaultRequestHeaders.Add("Authorization", Config.Instance.AuthHeader);
        client.DefaultRequestHeaders.Accept.Add(CONTENT_TYPE);
    }

    public async void Update()
    {
        try
        {
            _ip = client.GetStringAsync("https://api.ipify.org").Result;
            foreach (Record record in Config.Instance.Records)
            {
                string ip = await GetRecord(record);
                if (ip != _ip)
                {
                    StringContent content = new StringContent($"[{new DNSRecord(_ip)}]");
                    content.Headers.ContentType = CONTENT_TYPE;
                    Uri apiAddress = new Uri($"{BASE_ADDRESS}/v1/domains/{record.DomainName}/records/A/{record.HostName}");
                    HttpResponseMessage response = client.PutAsync(apiAddress, content).Result;
                    if (response != null)
                    {
                        string data = response.Content.ReadAsStringAsync().Result;
                        LogManager.ARecordUpdated(record);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            LogManager.Error(ex);
        }
    }

    private void VerifyConfig()
    {
        if (string.IsNullOrEmpty(Config.Instance.Key) ||
            string.IsNullOrEmpty(Config.Instance.Secret) ||
            Config.Instance.Records.Count == 0 ||
            string.IsNullOrEmpty(Config.Instance.Records[0].DomainName) ||
            string.IsNullOrEmpty(Config.Instance.Records[0].HostName))
        {
            LogManager.InvalidConfig();
            Environment.Exit(1);
        }
    }


    private async Task<string> GetRecord(Record record)
    {
        try
        {

            IPAddress[] ips = Dns.GetHostAddresses($"{record.HostName}.{record.DomainName}");
            if (ips.Length > 0)
            {
                return ips[0].ToString();
            }
            throw new Exception("No IP Address was returned by GetHostAddresses");
        }
        catch (Exception ex)
        {
            if (ex.GetType() == typeof(SocketException) && ex.Message == "No such host is known")
            {
                return "NO RECORD";
            }
            LogManager.Error(ex);
            return "ERROR";
        }
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Instance.ExecuteAsync(stoppingToken);
    }
}