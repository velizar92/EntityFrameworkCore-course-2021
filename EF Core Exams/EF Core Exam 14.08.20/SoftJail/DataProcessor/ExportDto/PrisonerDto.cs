using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ExportDto
{
    [XmlType("Prisoner")]
    public class PrisonerDto
    {
        [XmlElement("Id")]
        [Required]
        public int Id { get; set; }

        [XmlElement("Name")]
        [Required]
        public string Name { get; set; }

        [XmlElement("IncarcerationDate")]
        [Required]
        public string IncarcerationDate { get; set; }

        public MessageDto[] EncryptedMessages { get; set; }

    }
}
