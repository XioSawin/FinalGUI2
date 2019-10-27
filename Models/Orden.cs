//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mvc6.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class Orden
    {
        public int OrderId { get; set; }
        [DisplayName("Usuario")]
        public string Username { get; set; }
        [Required(ErrorMessage = "'Nombre' es un campo obligatorio")]
        [DisplayName("Nombre")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "'Apellido' es un campo obligatorio")]
        [DisplayName("Apellido")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "'Dirección' es un campo obligatorio")]
        public string Direccion { get; set; }
        [Required(ErrorMessage = "'Ciudad' es un campo obligatorio")]
        public string Ciudad { get; set; }
        [Required(ErrorMessage = "'Teléfono' es un campo obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "Por favor, ingrese una serie de números")]
        [DisplayName("Teléfono")]
        public string Phone { get; set; }
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
            ErrorMessage = "Email ingresado no es válido.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public Nullable<int> Total { get; set; }
    }
}
