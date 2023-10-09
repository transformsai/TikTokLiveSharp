using System;
using System.Linq;
using System.Windows.Forms;
using TikTokLiveSharp.Client;
using TikTokLiveSharp.Client.Config;
using TikTokLiveSharp.Events;

namespace TikTokLiveSharpWinFormsTestApp
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            TikTokLiveClient client = new TikTokLiveClient(inputHostId.Text, "", new ClientSettings()
            {
                EnableCompression = false
            });
            client.OnConnected += Client_OnConnected;
            client.OnDisconnected += Client_OnDisconnected;
            client.OnRoomUpdate += Client_OnViewerData;
            client.OnLiveEnded += Client_OnLiveEnded;
            client.OnJoin += Client_OnJoin;
            client.OnChatMessage += Client_OnComment;
            client.OnFollow += Client_OnFollow;
            client.OnShare += Client_OnShare;
            client.OnSubscribe += Client_OnSubscribe;
            client.OnLike += Client_OnLike;
            client.OnGiftMessage += Client_OnGiftMessage;
            client.OnEmoteChat += Client_OnEmote;
            client.Run(new System.Threading.CancellationToken());
        }



        private static void Client_OnConnected(TikTokLiveClient sender, bool e)
        {
            SetConsoleColor(ConsoleColor.White);
            Console.WriteLine($"Connected to Room! [Connected:{e}]");
        }

        private static void Client_OnDisconnected(TikTokLiveClient sender, bool e)
        {
            SetConsoleColor(ConsoleColor.White);
            Console.WriteLine($"Disconnected from Room! [Connected:{e}]");
        }

        private static void Client_OnViewerData(TikTokLiveClient sender, RoomUpdate e)
        {
            SetConsoleColor(ConsoleColor.Cyan);
            Console.WriteLine($"Viewer count is: {e.NumberOfViewers}");
            SetConsoleColor(ConsoleColor.White);
        }

        private static void Client_OnLiveEnded(TikTokLiveClient sender, ControlMessage e)
        {
            SetConsoleColor(ConsoleColor.White);
            Console.WriteLine("Host ended Stream!");
        }

        private static void Client_OnJoin(TikTokLiveClient sender, Join e)
        {
            SetConsoleColor(ConsoleColor.Green);
            Console.WriteLine($"{e.User.UniqueId} joined!");
            SetConsoleColor(ConsoleColor.White);
        }

        private static void Client_OnComment(TikTokLiveClient sender, Chat e)
        {
            SetConsoleColor(ConsoleColor.Yellow);
            Console.WriteLine($"{e.Sender.UniqueId}: {e.Message}");
            SetConsoleColor(ConsoleColor.White);
        }

        private static void Client_OnFollow(TikTokLiveClient sender, Follow e)
        {
            SetConsoleColor(ConsoleColor.DarkRed);
            Console.WriteLine($"{e.User?.UniqueId} followed!");
            SetConsoleColor(ConsoleColor.White);
        }

        private static void Client_OnShare(TikTokLiveClient sender, Share e)
        {
            SetConsoleColor(ConsoleColor.Blue);
            Console.WriteLine($"{e.User?.UniqueId} shared!");
            SetConsoleColor(ConsoleColor.White);
        }

        private static void Client_OnSubscribe(TikTokLiveClient sender, Subscribe e)
        {
            SetConsoleColor(ConsoleColor.DarkCyan);
            Console.WriteLine($"{e.User.UniqueId} subscribed!");
            SetConsoleColor(ConsoleColor.White);
        }

        private static void Client_OnLike(TikTokLiveClient sender, Like e)
        {
            SetConsoleColor(ConsoleColor.Red);
            Console.WriteLine($"{e.Sender.UniqueId} liked!");
            SetConsoleColor(ConsoleColor.White);
        }

        private static void Client_OnGiftMessage(TikTokLiveClient sender, GiftMessage e)
        {
            SetConsoleColor(ConsoleColor.Magenta);
            Console.WriteLine($"{e.User.UniqueId} sent {e.Amount}x {e.Gift.Name}!");
            SetConsoleColor(ConsoleColor.White);
        }

        private static void Client_OnEmote(TikTokLiveClient sender, EmoteChat e)
        {
            SetConsoleColor(ConsoleColor.DarkGreen);
            Console.WriteLine($"{e.User.UniqueId} sent {e.Emotes?.First()?.Id}!");
            SetConsoleColor(ConsoleColor.White);
        }

        private static void SetConsoleColor(ConsoleColor color)
        {
            if (Console.ForegroundColor != color)
                Console.ForegroundColor = color;
        }
    }
}
