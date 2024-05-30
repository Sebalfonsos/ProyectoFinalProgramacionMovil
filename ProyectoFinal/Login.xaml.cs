
namespace ProyectoFinal;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
	}

    private void Login_Clicked(object sender, EventArgs e)
    {
        var main = new NavigationPage(new MainPage());
        Application.Current.MainPage = main;
    }
}