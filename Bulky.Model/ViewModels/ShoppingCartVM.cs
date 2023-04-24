using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bulky.Model.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bulky.Model.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> ShoppingCartList{get; set;}
        public OrderHeader OrderHeader { get; set; }
        
    }
}