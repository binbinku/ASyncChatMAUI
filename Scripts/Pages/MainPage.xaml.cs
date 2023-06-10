
using System.ComponentModel;
using System.Text;
using Newtonsoft.Json;

namespace MauiApp1;

public partial class MainPage : ContentPage
{	
	private Button sendBtn;

	private Entry inputEntry;

	private VerticalStackLayout mainContent;

	private string inputStr;

	public MainPage()
	{
		InitializeComponent();
		//初始化
		Init();
		InitEvent();

		//测试
		Test();
	}

	async void Test()
	{
		var name = PlayerInfoManager.Instance.Name;
		await DisplayAlert("测试",$"昵称:{name}","确认");
	}

	private void Init()
	{
		sendBtn = MainSendBtn;
		inputEntry = MainInput;
		mainContent = MainContent;
	}

	private void InitEvent()
	{

		sendBtn.Clicked +=(sender,e)=>
		{
			//SendMessage();
			var name = PlayerInfoManager.Instance.Name;
			NetworkMessageSend();
			MadeMessageSection(name,inputStr);
			SendMessagePostHandle();
		};

		inputEntry.TextChanged +=(sender,e)=>
		{
			inputStr = e.NewTextValue;
		};

		ASyncNetworkClient.recCallBackDelegate += ReceiveCallBack;
	}

    private void NetworkMessageSend()
    {
		var mname = PlayerInfoManager.Instance.Name;

		NetworkMessage message = new NetworkMessage()
		{
			name = mname,
			message = inputStr
		};

		var sendStr =  JsonConvert.SerializeObject(message);

		ASyncNetworkClient.SyncSend(sendStr);
    }

    private void MadeMessageSection(string userName,string message)
	{
		//版块构造
		HorizontalStackLayout section = new HorizontalStackLayout()
		{
			BackgroundColor = Colors.White,
			MinimumHeightRequest = 60
		};

		//头像构造
		Image header = new Image()
		{
			Source = ImageSource.FromFile("csharp.png"),
			WidthRequest = 40,
			HeightRequest = 40,
			Margin = new Thickness(10),
			VerticalOptions = LayoutOptions.Start
		};

		//消息垂直区构造
		VerticalStackLayout infoZone = new VerticalStackLayout()
		{
			BackgroundColor = Colors.White,
			WidthRequest=330
		};

		//信息垂直区-Name标签构造
		Label userNameLabel = new Label()
		{
			Text = userName,
			Margin = new Thickness(0,5,0,0),
			FontFamily = "Consolas",
			FontAttributes = FontAttributes.Bold
		};

		//信息垂直区-Message标签构造
		Label messageLabel = new Label()
		{
			Text = RefactMessage(message),
			Margin = new Thickness(10),
			BackgroundColor = Colors.White,
			HorizontalOptions = LayoutOptions.Start,
			WidthRequest = 310,
			MinimumHeightRequest = 20,
			LineBreakMode = LineBreakMode.CharacterWrap
		};
		
		//右板块拼接
		infoZone.Add(userNameLabel);
		infoZone.Add(messageLabel);

		//总板块拼接
		section.Add(header);
		section.Add(infoZone);

		//填入总content
		mainContent.Add(section);
	}

	private string RefactMessage(string str)
	{
		if(str.Length<=40) return str;

		StringBuilder stringBuilder = new StringBuilder(str);
		
		for(int i=0; i<stringBuilder.Length; i++)
		{
			if(i!=0 && (i%39)==0)
			{
				stringBuilder.Insert(i,"\n");
			}			
		}

		return stringBuilder.ToString();
	}

	private void SendMessagePostHandle()
	{
		inputEntry.Text = "";
	}

	private void ReceiveCallBack(string message)
	{
		MainThread.InvokeOnMainThreadAsync(() =>
		{
			var networkMessage =  JsonConvert.DeserializeObject<NetworkMessage>(message);
			MadeMessageSection(networkMessage.name,networkMessage.message);
		});
	}
	private async void TestDisplayAlert()
	{
		await DisplayAlert("提示", "你点击了发送按钮", "同意","取消");
	}

	private async void TestDisplayActionSheet()
	{
		await DisplayActionSheet("标题","取消","销毁","选项1","选项2","选项3","选项4","选项5");
	}

	private async void TestDisplayPromptAsync()
	{
		await DisplayPromptAsync("标题","消息","同意","取消","占位符",-1,Keyboard.Default,"");
	}
}

