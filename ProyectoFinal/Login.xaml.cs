
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal;

public partial class Login : ContentPage
{
    private HttpClient _httpClient;
    public Login()
	{
		InitializeComponent();
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(EnviromentVariables.apiBaseURL);
        verifyToken();
    }

    private async void Login_Clicked(object sender, EventArgs e)
    {
        var email = EmailEntry.Text;
        var password = PasswordEntry.Text;

        var logindata = new
        {
            email = email,
            password = password
        };

        var parametrosJson = JsonSerializer.Serialize(logindata);
        var parametros = new StringContent(parametrosJson, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PostAsync("/api/user/login", parametros);

        Console.WriteLine((int)response.StatusCode);

        if ((int)response.StatusCode == 200)
        {
            string content = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<JsonElement>(content);

            if (responseObject.TryGetProperty("token", out var tokenElement))
            {
                var token = tokenElement.GetString();
                Preferences.Set("Token", token);

                EmailEntry.IsEnabled = false;
                PasswordEntry.IsEnabled = false;

                EmailEntry.IsEnabled = true;
                PasswordEntry.IsEnabled = true;

                abrirPaginaPrincipal();
            }
            else
            {
                // Manejar el caso donde la propiedad 'token' no está presente en la respuesta
                await DisplayAlert("Error", "No se encontró el token en la respuesta", "OK");
            }
        }
        else
        {
            // Manejar el caso donde la solicitud no fue exitosa (código de estado diferente de 200)
            await DisplayAlert("Error", $"La solicitud no fue exitosa. Código de estado: {response.StatusCode}", "OK");
        }
    }


    private void abrirPaginaPrincipal()
    {
        var main = new NavigationPage(new MainPage());
        Application.Current.MainPage = main;
    }

    public async void verifyToken()
    {
        bool removerIndicador = false;
        string bearerToken = Preferences.Get("Token", string.Empty);


        if (!string.IsNullOrEmpty(bearerToken))
        {
            // Establecer el token de portador en el encabezado de autorización
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

            // Establecer el encabezado 'Accept' en la solicitud
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await _httpClient.GetAsync("/api/validateToken");

            if ((int)response.StatusCode == 200)
            {
                abrirPaginaPrincipal();
            }
            else
            {
                removerIndicador = true;
            }

        }
        else
        {
            removerIndicador = true;
        }

        if (removerIndicador)
        {
            indicadorCargando.IsVisible = false;
            vistaLogin.IsVisible = true;
        }

    }

}