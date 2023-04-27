using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace CherryDDNS
{
    [Serializable]
    public class Config
    {
        public string Secret { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public List<Record> Records { get; set; } = new List<Record>();

        private const string FILENAME = "config.json";
        private static Config _instance;
        public static Config Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Load();
                }
                return _instance;
            }
        }

        public static string Model
        {
            get
            {
                Config model = new Config().ToString();
                model.Records.Add(new Record());
                return model.ToString();
            }
        }

        [JsonIgnore]
        public static string FilePath
        {
            get
            {
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FILENAME);
            }
        }
        [JsonIgnore]
        public string AuthHeader
        {
            get
            {
                return $"sso-key {Key}:{Secret}";
            }
        }

        private Config() { }

        private static Config Load()
        {
            try
            {
                if (!File.Exists(FilePath)) new Config().Save();
                string json = string.Empty;
                using (StreamReader reader = new StreamReader(FilePath))
                {
                    json = reader.ReadToEnd();
                }
                return json;
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
                return null;
            }
        }
        public void Save()
        {
            try
            {
                if (this.Records.Count == 0) this.Records.Add(new Record()); //if there are no records, we want to create a blank one in the file for users to see/edit
                using (StreamWriter sw = new StreamWriter(FILENAME, false))
                {
                    string body = this.ToString();
                    sw.WriteLine(body);
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }



        public string ToMaskedString()
        {
            Config maskedConfig = this.ToString();
            if (!string.IsNullOrWhiteSpace(maskedConfig.Secret)) maskedConfig.Secret = "[redacted]";
            if (!string.IsNullOrWhiteSpace(maskedConfig.Key)) maskedConfig.Key = "[redacted]";
            return maskedConfig.ToString();
        }

        public static implicit operator Config(string json)
        {
            return JsonConvert.DeserializeObject<Config>(json);
        }
    }

    public class Record
    {
        public string HostName { get; set; } = string.Empty;
        public string DomainName { get; set; } = string.Empty;
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        public static implicit operator Record(string json)
        {
            return JsonConvert.DeserializeObject<Record>(json);
        }
    }
}
