using ProyectoFinal.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ProyectoFinal
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<CategoryItem> Categories { get; set; }
        public ObservableCollection<ChannelItem> Channels { get; set; }
        public ICommand CategoryTappedCommand { get; set; }
        public ICommand ChannelTappedCommand { get; set; }
        public MainPage()
        {
            InitializeComponent();
            // Inicializar las colecciones
            Categories = new ObservableCollection<CategoryItem>
            {
                new CategoryItem { Name = "Página de Fútbol", Image = "dotnet_bot.png" },
                new CategoryItem { Name = "Página de Kid", Image = "dotnet_bot.png" },
                new CategoryItem { Name = "Página de Novela", Image = "dotnet_bot.png" }
            };

            Channels = new ObservableCollection<ChannelItem>
            {
                new ChannelItem { Name = "Canal 1", Image = "dotnet_bot.png", Price = "$10" },
                new ChannelItem { Name = "Canal 2", Image = "dotnet_bot.png", Price = "$15" },
                new ChannelItem { Name = "Canal 3", Image = "dotnet_bot.png", Price = "$20" },
                new ChannelItem { Name = "Canal 4", Image = "dotnet_bot.png", Price = "$50" },
                // Añade más canales según sea necesario
            };

            // Definir los comandos
            CategoryTappedCommand = new Command<CategoryItem>(OnCategoryTapped);
            ChannelTappedCommand = new Command<ChannelItem>(OnChannelTapped);

            // Establecer los ItemsSource
            categoriesCollection.ItemsSource = Categories;
            channelsCollection.ItemsSource = Channels;

            // Establecer el contexto de datos de la página para el enlace de comandos
            BindingContext = this;
        }

        private void OnCategoryTapped(CategoryItem category)
        {
            // Manejar el evento de tap en la categoría
            DisplayAlert("Categoría Tapped", $"Seleccionaste la categoría: {category.Name}", "OK");
        }

        private void OnChannelTapped(ChannelItem channel)
        {
            // Manejar el evento de tap en el canal
            DisplayAlert("Canal Tapped", $"Seleccionaste el canal: {channel.Name} con precio {channel.Price}", "OK");
        }

        private async void OnFutbolTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Paquetes());
        }

        private async void OnCanalTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Canal());
        }


        public class CategoryItem
        {
            public string Name { get; set; }
            public string Image { get; set; }
            public ICommand TappedCommand { get; set; }
        }

        public class ChannelItem
        {
            public string Name { get; set; }
            public string Image { get; set; }
            public string Price { get; set; }
            public ICommand TappedCommand { get; set; }
        }

    }

}
