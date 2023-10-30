using System;
using System.ComponentModel.DataAnnotations;

namespace lab3.Models
{
	public class Musikerdetalj
	{
        public int Skivor_ID { get; set; }

        [Required(ErrorMessage = "Skivor_Name är obligatoriskt")]
        public string? Skivor_Name { get; set; }

        public string? Skivor_Genre { get; set; }

        public int Skivor_Musiker_Id { get; set; }

        public string? Musiker_Name { get; set; }

        public int Musiker_Id { get; set; }

        public string? Keywords { get; set; }
        public string? Description { get; set; }
        public string? OpenGraphTitle { get; set; }
        public string? OpenGraphDescription { get; set; }
    }
}

