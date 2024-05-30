using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProyectoFinal
{
    public class ChannelItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }

        public string URL { get; set; }
        public string LogoURL { get; set; }
        public int Price { get; set; }
        public ICommand TappedCommand { get; set; }
    }
}
