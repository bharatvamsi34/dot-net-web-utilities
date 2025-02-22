using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BV.Utilities.Models
{
    public class PdfModel
    {
        [Required(ErrorMessage = "Please select a PDF file.")]
        public IFormFile PdfFile { get; set; }

        [Required(ErrorMessage = "Please enter the password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
