using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MIM.Model
{
    class Music : INotifyPropertyChanged
    {
        private string name { get; set;}
        private string lenght { get; set; }
        private string audioPath { get; set; }

        public string Name 
        {
            get { return name; }
            set { name = value; OnPropertyChanged(); }
        }

        public string Lenght 
        {
            get { return lenght; }
            set { lenght = value; OnPropertyChanged(); }
        }

        public string AudioPath
        {
            get { return audioPath; }
            set { audioPath = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
