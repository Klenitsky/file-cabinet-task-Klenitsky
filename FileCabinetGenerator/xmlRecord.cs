using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml.Serialization;

namespace FileCabinetGenerator
{
    [Serializable]
    [XmlRoot(Namespace = "", IsNullable = false, ElementName = "records")]
    public class RecordsXml
    {
        [XmlElement("record")]
        public List<RecordXml> Records { get; } = new List<RecordXml>();

        public RecordsXml() { }

        public RecordsXml(List<RecordXml> records)
        {
            Records = records;
        }
    }

    [Serializable]
    public class RecordXml
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlElement("name")]
        public XmlName Name { get; set; } = new XmlName();

        [XmlElement("dateOfBirth")]
        public string DateOfBirth { get; set; }

        [XmlElement("height")]
        public short Height { get; set; }

        [XmlElement("weight")]
        public decimal Weight { get; set; }

        [XmlElement("drivingLicenseCategory")]
        public char DrivingLicenseCategory { get; set; }
    }

    [Serializable]
    public class XmlName
    {
        [XmlAttribute("first")]
        public string First { get; set; }

        [XmlAttribute("last")]
        public string Last { get; set; }
    }
}
