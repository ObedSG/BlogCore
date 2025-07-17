using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public class Slider
    {
        [Key]
        public int Id { get; set; }

        
        [Display(Name = "Nombre Slider")]
        [Required(ErrorMessage = "Ingrese un nombre para el slider")]
        public string Nombre { get; set; }

        [Required]
        public bool Estado { get; set; }


        [ValidateNever]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Imagen")]

        public string UrlImagen { get; set; }


    }
}
