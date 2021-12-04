using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;
using Theatre.Data.Models.Enums;

namespace Theatre.DataProcessor.ImportDto
{
    [XmlType("Play")]
    public class PlayDto
    {
        [Required]
        [XmlElement("Title")]
        [StringLength(50, MinimumLength = 4)]
        public string Title { get; set; }

        [Required]
        [XmlElement("Duration")]
        public string Duration { get; set; }

        [Required]
        [XmlElement("Rating")]
        [Range(0.00, 10.00)]
        public float Rating { get; set; }


        [Required]
        [XmlElement("Genre")]
        [EnumDataType(typeof(Genre))]
        public string Genre { get; set; }

        [Required]
        [XmlElement("Description")]
        [MaxLength(700)]
        public string Description { get; set; }

        [Required]
        [XmlElement("Screenwriter")]
        [StringLength(30, MinimumLength = 4)]
        public string Screenwriter { get; set; }




    }
}
