using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class SlideItem:IEntity
    {
        public int Id{ get; set; }
        public string? Photo { get; set; }
        public string? Offer{ get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
