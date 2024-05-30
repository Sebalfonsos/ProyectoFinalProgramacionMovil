using CommunityToolkit.Maui.Views;

namespace ProyectoFinal.Views;

public partial class ViewChannel : ContentPage
{
	public ViewChannel()
	{
		InitializeComponent();
	}
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        mediaElement?.Pause();
        mediaElement?.Stop();
    }

}