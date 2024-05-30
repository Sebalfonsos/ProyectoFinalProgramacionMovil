using System.Text.Json;
using System.Text;
using System.Threading.Channels;
using System.Numerics;

namespace ProyectoFinal.Views;

public partial class Canal : ContentPage
{
    ChannelItem ChannelItem { get; set; }
	public Canal(ChannelItem channel)
	{
        ChannelItem= channel;   
		InitializeComponent();
		imagenCanal.Source = channel.Image;
        nombreCanal.Text = channel.Name;
        
        precioCanal.Text = channel.Price.ToString();
	}

    private async void btnComprar_Clicked(object sender, EventArgs e)
    {
        List<int> channelIds = new List<int> { ChannelItem.Id }; // Ejemplo de lista de IDs de canales seleccionados
        

        var data = new
        {
            channels = channelIds,
            total = ChannelItem.Price
        };

        var parametrosJson = JsonSerializer.Serialize(data);
        var parametros = new StringContent(parametrosJson, Encoding.UTF8, "application/json");

        string apiUrl = EnviromentVariables.apiBaseURL + "/api/comprar";

        HttpClient client = new HttpClient();
        string bearerToken = Preferences.Get("Token", string.Empty);
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        HttpResponseMessage response = await client.PostAsync(apiUrl, parametros);
        response.EnsureSuccessStatusCode();

        string jsonResponse = await response.Content.ReadAsStringAsync();
        Console.WriteLine("API Response: " + jsonResponse);

        await Navigation.PushAsync(new ViewChannel(ChannelItem));

        Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);

        // Aquí puedes procesar la respuesta según lo necesites
    }


}