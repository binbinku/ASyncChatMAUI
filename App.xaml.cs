namespace MauiApp1;

public partial class App : Application
{
	private static readonly int windowWidth = 390;
	private static readonly int windowHeight = 800;

	public App()
	{
		InitializeComponent();
		MainPage = new AppShell();		
	}

	protected override Window CreateWindow(IActivationState activationState)
	{
		var window = base.CreateWindow(activationState);

		window.Width = windowWidth;
		window.Height = windowHeight;

		window.MinimumWidth = windowWidth;
		window.MinimumHeight = windowHeight;

		window.MaximumWidth	 = windowWidth;	
		window.MaximumHeight = windowHeight;

		window.Stopped+=(sender,e)=>
		{
			ASyncNetworkClient.SyncDisconnect();	
		};

		return window;
	}
	

	  


}
