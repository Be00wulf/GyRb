using System.ComponentModel.DataAnnotations;

namespace GyRb.ViewModels
{
    public class TicketCodeVM
    {
        [Required(ErrorMessage = "Debe ingresar el código de su ticket") ]
        [StringLength(100)]
        public string Code { get; set; } = string.Empty;


    }
}
