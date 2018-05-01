
using Mixer.Base;
using Mixer.Base.Clients;
using Mixer.Base.Model.Channel;
using Mixer.Base.Model.OAuth;
using Mixer.Base.Model.User;
using Mixer.Base.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading.Tasks;
using Mixer_WPF;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Net;
using System.Windows.Navigation;
using mshtml;

namespace Mixer_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MixerConnection connection;
        private ExpandedChannelModel channel;
        private PrivatePopulatedUserModel user;
        private IEnumerable<ChannelAdvancedModel> follows;
        
        List<string> CUser = new List<string>();
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = user;
        }
        private void AvatarDone(object sender, NavigationEventArgs e)
        {
            IHTMLDocument2 documentText = (IHTMLDocument2)Avatar.Document;
            //this will access the document properties 
            documentText.body.style.overflow = "hidden";
            // This will hide the scrollbar (Set to "auto" if you want to see when it passes the surfacelimit)
        }


        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            this.LoginGrid.Visibility = Visibility.Visible;

            this.MainGrid.Visibility = Visibility.Collapsed;


            List<OAuthClientScopeEnum> scopes = new List<OAuthClientScopeEnum>()
            {

                OAuthClientScopeEnum.chat__chat,
                OAuthClientScopeEnum.chat__connect,


                OAuthClientScopeEnum.channel__details__self,
                OAuthClientScopeEnum.channel__update__self,

                OAuthClientScopeEnum.user__details__self,
                OAuthClientScopeEnum.user__log__self,
                OAuthClientScopeEnum.user__notification__self,
                OAuthClientScopeEnum.user__update__self,
            };

            this.connection = await MixerConnection.ConnectViaLocalhostOAuthBrowser(ConfigurationManager.AppSettings["ClientID"], scopes);
            List<string> following = new List<string>();

            if (this.connection != null)
            {
                this.user = await this.connection.Users.GetCurrentUser();

                this.follows = await this.connection.Users.GetFollows(user);

                this.UserName.Text = this.user.username;

                foreach(var follow in follows)
                {
                    following.Add(follow.token);
                }

                this.Avatar.Navigate("https://mixer.com/api/v1/users/"+user.channel.userId+"/avatar?w=60&h=60");
                this.Follows.ItemsSource = following;

                



            }

            this.LoginGrid.Visibility = Visibility.Collapsed;


            this.MainGrid.Visibility = Visibility.Visible;

        }
       
        
      
        
      
    }
}

