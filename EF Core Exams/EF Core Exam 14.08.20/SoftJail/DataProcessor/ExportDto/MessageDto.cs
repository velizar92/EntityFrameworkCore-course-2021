using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ExportDto
{
    [XmlType("Message")]
    public class MessageDto
    {

        [XmlElement("Description")]
        [Required]
        public string Description { get; set; }
    }
}