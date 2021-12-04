using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Theatre.DataProcessor.ExportDto
{
    [XmlType("Play")]
    public class PlayExportDto
    {
        //Title="A Raisin in the Sun" Duration="01:40:00" Rating="5.4" Genre="Drama

        [XmlAttribute("Title")]
        
        public string Title { get; set; }


        [XmlAttribute("Duration")]
       
        public string Duration { get; set; }


        [XmlAttribute("Rating")]
      
        public string Rating { get; set; }

        [XmlAttribute("Genre")]
       
        public string Genre { get; set; }


        [XmlArray("Actors")]
        public ActorDto[] Actors { get; set; }
    }
}
