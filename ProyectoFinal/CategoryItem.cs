using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProyectoFinal
{
   public class CategoryItem
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public ICommand TappedCommand { get; set; }
    }
}
