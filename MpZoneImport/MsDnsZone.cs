namespace MpZoneImport
{
    using System.Collections.Generic;

    public class MsDnsZone
    {
        public string Name { get; set; }

        public MsDnsZoneSOA Soa { get; set; }
        public List<MsDnsZoneRecord> Records { get; set; }

        public MsDnsZone()
        {
            Records = new List<MsDnsZoneRecord>();
        }
    }


    public class MsDnsZoneSOA
    {
        public int ExpireLimit { get; set; }
        public int MinimumTTL { get; set; }
        public string PrimaryServer { get; set; }
        public int RefreshInterval { get; set; }
        public string ResponsibleParty { get; set; }
        public int RetryDelay { get; set; }
        public long SerialNumber { get; set; }        
    }

    public class MsDnsZoneRecord
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public int Priority { get; set; }
        public RecordTypes RType { get; set; }
    }

    public enum RecordTypes
    {
        None,
        A,
        AAAA,
        CNAME,
        MX,
        NS,
        TXT
    }
}
