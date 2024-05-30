using CommunityToolkit.Maui.Views;

namespace ProyectoFinal.Views;

public partial class ViewChannel : ContentPage
{
	public ViewChannel(ChannelItem channel)
	{
		InitializeComponent();
        mediaElement.Source = channel.URL;

    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        mediaElement?.Pause();
        mediaElement?.Stop();
    }

}