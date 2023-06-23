using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBookModels
{
    public class ShoppingCart
    {
        public Product Product { get; set; }
        [Range(1, 1000, ErrorMessage = "please enter a value between 1 and 100")]
        public int Count { get; set; }
    }
}
