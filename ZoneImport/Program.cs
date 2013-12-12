namespace ZoneImport
{
    using Mono.Options;
    using MpMigrate.MaestroPanel.Api;
    using MpMigrate.MaestroPanel.Api.Entity;
    using MpZoneImport;
    using System;
    using System.Linq;

    class Program
    {
        private static string _zoneDirectory;

        private static string _apiKey;
        private static string _apiHost;
        private static string _apiPort;
        private static string _apiSSL;
        private static string _defaultPlan;
        private static string _createDomain;

        private static bool _defaultSSL = false;
        private static int _defaultPort = 9715;

        static void Main(string[] args)
        {
            bool validation = true;

           
            var optionSet = new OptionSet
            {
                {"key=","MaestroPanel API Key",v => { _apiKey = v; }},
                {"host=","MaestroPanel Host",v => { _apiHost = v; }},
                {"port=","MaestroPanel Port Number",v => { _apiPort = v; }},
                {"ssl=","Enable SSL",v => { _apiSSL = v; }},
                {"plan=","Default Domain Plan",v => { _defaultPlan = v; }},
                {"path=","DNS Zone Directory",v => { _zoneDirectory = v; }},
                {"createDomain=", "Create Domain", v => {_createDomain = v;}}
            };

            optionSet.Parse(args);

            #region Validation
            if (String.IsNullOrEmpty(_apiKey))
            {
                Console.WriteLine("MaestroPanel API Key is null: " + _apiKey);

                validation = false;
                ShowUsage();
                return;
            }

            if (String.IsNullOrEmpty(_apiHost))
            {
                Console.WriteLine("MaestroPanel Host is null: " + _apiKey);

                validation = false;
                ShowUsage();
                return;
            }

            if (String.IsNullOrEmpty(_zoneDirectory))
            {
                Console.WriteLine("Zone Directory is null");

                validation = false;
                ShowUsage();
                return;
            }

            
            if (!System.IO.Directory.Exists(_zoneDirectory))
            {
                Console.WriteLine("Zone Directory is not exists.");

                validation = false;
                ShowUsage();
                return;
            }

            if (!int.TryParse(_apiPort, out _defaultPort))
            {
                Console.WriteLine("Invalid Port Numner: "+ _apiPort);

                validation = false;
                ShowUsage();
                return;
            }

            if (!bool.TryParse(_apiSSL, out _defaultSSL))
            {
                Console.WriteLine("Invalid SSL Value (true or false): " + _apiSSL);

                validation = false;
                ShowUsage();
                return;
            }
            #endregion

            if (validation)
                Start();            
        }

        static void ShowUsage()
        {
            Console.WriteLine("MaestroPanel Zone File Import - www.maestropanel.com");
            Console.WriteLine("Parameter List");
            Console.WriteLine("\t--path\tDNS Zone file directory");
            Console.WriteLine("\t--key\t\tMaestroPanel API Key");
            Console.WriteLine("\t--host\tMaestroPanel Host.");
            Console.WriteLine("\t--port\tMaestroPanel Port");
            Console.WriteLine("\t--ssl\tSSL Connection");
            Console.WriteLine("\t--plan\tMaestroPanel Default Domain Plan");
            Console.WriteLine("\t--createDomain\tIf you want to create domain set true. Default false");
        }

        static string GetPassword()
        {
            return System.Web.Security.Membership.GeneratePassword(8, 1);
        }

        static void Start()
        {
            var _parser = new MsDnsZoneParser(_zoneDirectory);
            var _api = new ApiClient(_apiKey, _apiHost, _defaultPort, _defaultSSL, format:"XML", suppressResponse:true,
                suppressDnsZoneIP: false, generatePassword:false);
            
            var ZoneList = _parser.Start();

            foreach (var item in ZoneList)
            {
                ApiResult<DomainOperationsResult> createResult = null;

                if (_createDomain == "true")
                {
                    Console.WriteLine("Creating {0}", item.Name);
                    createResult = _api.DomainCreate(item.Name, _defaultPlan, item.Name, GetPassword(), false);
                    Console.WriteLine("\tResult: {0}", createResult.Message);
                }
                
                var records = item.Records
                            .Select(m => new DnsZoneRecordItem() { name = m.Name, value = m.Value, type = m.RType.ToString(), priority = m.Priority })
                            .ToList();

                var serialNumber = Convert.ToInt32(DateTime.Now.ToString("yyyyMMddHH"));

                Console.WriteLine("Deploy Dns Zone {0}", item.Name);

                var dnsZoneResult = _api.SetDnsZone(item.Name,
                        item.Soa.ExpireLimit, item.Soa.MinimumTTL,
                        item.Soa.RefreshInterval, item.Soa.ResponsibleParty,
                        item.Soa.RetryDelay, serialNumber,
                        item.Soa.PrimaryServer, records);

                Console.WriteLine("\tResult: {0}", dnsZoneResult.Message);                
            }
            
        }

    }
}
