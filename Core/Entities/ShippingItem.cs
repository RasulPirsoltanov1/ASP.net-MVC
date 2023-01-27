using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities;

public class ShippingItem: IEntity
{
    public int Id{ get; set; }
    [Required]
    public string? Image{ get; set; }
    [Required(ErrorMessage ="must be not empty"), MaxLength(100)]
    public string? Title{ get; set; }
    public string? Description { get; set; }
}
