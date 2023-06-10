
namespace MauiApp1;

public partial class LoginPage : ContentPage
{	
	private Entry loginNameEntry;

	private Button loginBtn;

	private string loginName;
	public LoginPage()
	{
		InitializeComponent();
		Init();
		InitEvent();
	}


    private void Init()
    {
		loginNameEntry = LoginNameEntry;
		loginBtn = LoginBtn;

		ASyncNetworkClient.Init();
    }

    private void InitEvent()
    {
       loginNameEntry.TextChanged+=(sender,e)=>
	   {
			loginName = e.NewTextValue;
	   };


		loginBtn.Clicked += (sender, e)=>
		{
			Login();
		};

		ASyncNetworkClient.connectCallBackDelegate += NetworkClientConnectCallBack;

    }

	private async void Login()
	{
		Console.WriteLine("[LoginPage]:正在登录！！！！");

		if(loginName==null||loginName=="")
		{
			await DisplayAlert("提升🈲️","昵称不可为空","收到🫡");
			return;
		}

		ASyncNetworkClient.ASyncConnect();
		
		loginBtn.IsEnabled = false;

	}

	private async void NetworkClientConnectCallBack(bool isSuc)
	{
		MainThread.BeginInvokeOnMainThread(async()=>
		{
			if(isSuc)
			{
				await DisplayAlert("欢迎👏","点击进入聊天室","进入");
			
				PlayerInfoManager.Instance.Name = loginName;

				MainPage mainPage = new MainPage();
				
				await Shell.Current.GoToAsync("//MainPage");

			}else
			{
				await DisplayAlert("提示🔔","连接服务器失败","确认");
				loginBtn.IsEnabled = true;
			}
		});
	}

}

