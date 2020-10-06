using Microsoft.Win32;
using MIM.Commands;
using MIM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TagLib.Id3v2;

namespace MIM.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        private Music selectedMusic;
        private ObservableCollection<Music> musics;
        private MediaPlayer mediaPlayer = new MediaPlayer();
        private bool _isPlaying = false;
        private string _iconPlay = "PlayArrow";

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public ObservableCollection<Music> Musics 
        {
            get { return musics; }
            set { musics = value; OnPropertyChanged("Musics"); }
        }

        public bool IsPlaying
        {
            get { return _isPlaying; }
            set { _isPlaying = value; OnPropertyChanged(); }
        }

        public string IconPlay
        {
            get { return _iconPlay; }
            set { _iconPlay = value; OnPropertyChanged(); }
        }

        public Music SelectedMusic
        {
            get { return selectedMusic; }
            set 
            {
                selectedMusic = value;
                if(selectedMusic != null)
                    mediaPlayer.Open(new Uri(selectedMusic.AudioPath));
                IsPlaying = false;
                IconPlay = "PlayArrow";
                OnPropertyChanged("SelectedMusic");
            }
        }

        public MainViewModel()
        {
            //Musics = new ObservableCollection<Music>
            //{
            //    new Music { Name = "Song1", AudioPath = "C:\\Users\\ybych\\Desktop\\ligalajz-buduschie-mamy_(muzzona.kz).mp3" },
            //    new Music { Name = "Song2", AudioPath = "C:\\Users\\ybych\\Desktop\\TOR BAND - мы не народец_[mp3mob.net].mp3" },
            //    new Music { Name = "Song3", AudioPath = "C:\\Users\\ybych\\Desktop\\ligalajz-buduschie-mamy_(muzzona.kz).mp3" }
            //};
        }

        public ICommand CloseApp {
            get { 
                return new DelegateCommand((obj) => {
                    Application.Current.Shutdown();
                });
            }
        }

        public ICommand MinimizeApp
        {
            get
            {
                return new DelegateCommand((obj) => {
                    
                });
            }
        }

        public ICommand AddMusic 
        {
            get 
            {
                return new DelegateCommand((obj) => {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "Audio Files (*.mp3)|*.mp3";
                    if (openFileDialog.ShowDialog() == true) 
                    {
                        Music music = new Music
                        {
                            Name = Path.GetFileName(openFileDialog.FileName.ToString()),
                            AudioPath = openFileDialog.FileName.ToString(),
                            Lenght = TagLib.File.Create(openFileDialog.FileName).Properties.Duration.ToString(@"mm\:ss")
                        };
                        if (Musics != null)
                        {
                            Musics.Add(music);
                            SelectedMusic = music;
                        }
                        else
                        {
                            Musics = new ObservableCollection<Music>
                            {
                                music
                            };
                            SelectedMusic = music;
                        }
                    }
                });
            }
        }

        public ICommand DeleteMusic
        {
            get
            {
                return new DelegateCommand((obj) => {
                    Music music = obj as Music;
                    if (music != null) 
                    {
                        Musics.Remove(music);
                        selectedMusic = Musics.FirstOrDefault();
                        mediaPlayer.Close();
                    }
                }, (obj) => obj != null);
            }
        }

        public ICommand PlayMusic 
        {
            get
            {
                return new DelegateCommand((obj) => {
                    if (!IsPlaying)
                    {
                        mediaPlayer.Play();
                        IsPlaying = true;
                        IconPlay = "Pause";
                    }
                    else
                    {
                        mediaPlayer.Pause();
                        IsPlaying = false;
                        IconPlay = "PlayArrow";
                    }
                }, (obj) => obj != null);
            }
        }
    }
}
