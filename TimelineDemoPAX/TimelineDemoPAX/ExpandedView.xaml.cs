using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.UserActivities;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Shell;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace TimelineDemoPAX
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExpandedView : Page, INotifyPropertyChanged
    {
        private UserActivityChannel _userActivityChannel;
        private UserActivity _userActivity;
        private UserActivitySession _userActivitySession;

        private MediaContent _mediacontent;
        public ExpandedView()
        {
            this.InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            _userActivityChannel = UserActivityChannel.GetDefault();
            _userActivity = await _userActivityChannel.GetOrCreateUserActivityAsync("LikePhoto");

            if (e.Parameter is MediaContent mediacontent)
            {
                MediaContent = mediacontent;
            }

            if (Frame.CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                SystemNavigationManager.GetForCurrentView().BackRequested += DetailsPage_BackRequested;
            }


           
        }

        private async void SymbolIcon_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            
       
            var adaptiveCard = File.ReadAllText($@"{Package.Current.InstalledLocation.Path}\AdaptiveCard.json");
            
            // var adaptiveCard = File.ReadAllText("C:/Users/rarang/Desktop/TimelineDemoPOC/TimelineDemoPAX/TimelineDemoPAX/Assets/AdaptiveCard.json");

            adaptiveCard = adaptiveCard.Replace("{{event}}", _mediacontent.Event);

            _userActivity.ActivationUri = new Uri($"my-timeline://details?{_mediacontent.ImageSource.Replace("ms-appx:///Assets/Images/", "")}");

            
            _userActivity.VisualElements.DisplayText = _mediacontent.Event;

            
            _userActivity.VisualElements.Content = AdaptiveCardBuilder.CreateAdaptiveCardFromJson(adaptiveCard);

           
            await _userActivity.SaveAsync();

            _userActivitySession?.Dispose();
            _userActivitySession = _userActivity.CreateSession();

            await new MessageDialog("Added to timeline").ShowAsync();

        }

        private void DetailsPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
            else
            {
                Frame.Navigate(typeof(MainPage));
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested -= DetailsPage_BackRequested;
        }

        public MediaContent MediaContent
        {
            get
            {
                return _mediacontent;
            }
            set
            {
                _mediacontent = value;
                RaisePropertyChanged();
            }
        }

        private void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

