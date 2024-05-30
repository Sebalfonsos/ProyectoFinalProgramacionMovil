
using ProyectoFinal.Views;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
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

            ICommand refreshCommand = new Command(async () =>
            {
                Channels = new ObservableCollection<ChannelItem>
                {

                };

                categoriesCollection.ItemsSource = Categories;
                channelsCollection.ItemsSource = Channels;

                await LoadChannelsFromApi();
                refreshView.IsRefreshing = false;
            });
            refreshView.Command = refreshCommand;


            // Inicializar las colecciones
            Categories = new ObservableCollection<CategoryItem>
            {
                new CategoryItem { Name = "Página de Fútbol", Image = "dotnet_bot.png" },
                new CategoryItem { Name = "Página de Kid", Image = "dotnet_bot.png" },
                new CategoryItem { Name = "Página de Novela", Image = "dotnet_bot.png" }
            };

            Channels = new ObservableCollection<ChannelItem>
            {
                //new ChannelItem { Name = "Canal 1", Image = "dotnet_bot.png", Price = "$10" },
                //new ChannelItem { Name = "Canal 2", Image = "dotnet_bot.png", Price = "$15" },
                //new ChannelItem { Name = "Canal 3", Image = "dotnet_bot.png", Price = "$20" },
                //new ChannelItem { Name = "Canal 4", Image = "dotnet_bot.png", Price = "$50" },
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

            LoadChannelsFromApi();
        }

        private void OnCategoryTapped(CategoryItem category)
        {
            // Manejar el evento de tap en la categoría
            DisplayAlert("Categoría Tapped", $"Seleccionaste la categoría: {category.Name}", "OK");
        }

        private async void OnChannelTapped(ChannelItem channel)
        {
            // Manejar el evento de tap en el canal
            //DisplayAlert("Canal Tapped", $"Seleccionaste el canal: {channel.Name} con precio {channel.Price}", "OK");
            await Navigation.PushAsync(new ViewChannel(channel));
        }

  

        private async Task LoadChannelsFromApi()
        {
            // URL de tu API
            string apiUrl = EnviromentVariables.apiBaseURL+"/api/channels";
            Console.WriteLine(apiUrl);
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string bearerToken = Preferences.Get("Token", string.Empty);
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();
                   
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("API Response: " + jsonResponse);
                    using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                    {
                        JsonElement root = doc.RootElement.GetProperty("data");
                        foreach (JsonElement channelElement in root.EnumerateArray())
                        {
                            string name = channelElement.GetProperty("name").GetString();
                            string url = channelElement.GetProperty("URL").GetString();
                            string logo = channelElement.GetProperty("logoURL").GetString();
                            int price = channelElement.GetProperty("price").GetInt32();
                            Console.WriteLine("hola");
                            Console.WriteLine(name);
                            Channels.Add(new ChannelItem
                            {
                                Name = name,
                                URL = url,
                                Image = logo,
                                Price = price.ToString("C")  // Formatear como moneda
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar errores de la API
                await DisplayAlert("Error", $"No se pudieron cargar los canales: {ex.Message}", "OK");
            }
        }

       

        private async void Close_Clicked(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient();
            string bearerToken = Preferences.Get("Token", string.Empty);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

            string apiUrl = EnviromentVariables.apiBaseURL + "/api/validateToken";
            HttpResponseMessage response = await client.GetAsync(apiUrl);
            Console.WriteLine((int)response.StatusCode);
            if ((int)response.StatusCode == 200)
            {
                HttpResponseMessage closeresponse = await client.GetAsync(EnviromentVariables.apiBaseURL+"/api/user/logout");
                Console.WriteLine((int)closeresponse.StatusCode);
                if ((int)closeresponse.StatusCode == 200)
                {
                    var Login = new Login();
                    Application.Current.MainPage = Login;
                }
            }
        }
    }

}


