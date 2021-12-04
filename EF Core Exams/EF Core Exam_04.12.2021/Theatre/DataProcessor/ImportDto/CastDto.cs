using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Theatre.DataProcessor.ImportDto
{
    [XmlType("Cast")]
    public class CastDto
    {

       
        [XmlElement("FullName")]
        [Required]
        [StringLength(30, MinimumLength = 4)]
        public string FullName { get; set; }

        
        [XmlElement("IsMainCharacter")]
        [Required]
        public bool IsMainCharacter { get; set; }
       
        [XmlElement("PhoneNumber")]
        [Required]
        [RegularExpression(@"^\+44\-[\d]{2}\-[\d]{3}\-[\d]{4}$")]
        public string PhoneNumber { get; set; }

       
        [XmlElement("PlayId")]
        [Required]
        public int PlayId { get; set; }
    }
}
