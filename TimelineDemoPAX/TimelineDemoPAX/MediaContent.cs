using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TimelineDemoPAX
{
    public class MediaContent:INotifyPropertyChanged
    {
        private string _eventpax, _taglineevent, _imageSource;
        public MediaContent(string eventpax, string taglineevent, string imageName)
        {
            Event = eventpax;
            Tag = taglineevent;
            ImageSource = $"ms-appx:///Assets/Images/{imageName}";
        }

        public MediaContent()
        {
        }

        public string Event
        {
            get
            {
                return _eventpax;
            }
            set
            {
                _eventpax = value;
                RaisePropertyChanged();
            }
        }


        public string Tag
        {
            get
            {
                return _taglineevent;
            }
            set
            {
                _taglineevent = value;
                RaisePropertyChanged();
            }
        }

        public string ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = $"ms-appx:///Assets/Images/{value}";
                RaisePropertyChanged();
            }
        }


        private void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
