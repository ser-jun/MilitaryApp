using NAudio.Wave;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Media;

namespace MilitaryApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

           
                var filePath = "D:\\Documents\\Cursach\\MilitaryApp\\MilitaryApp\\Music\\backgroundMusic.mp3";

               
                outputDevice = new WaveOutEvent();
                audioFile = new AudioFileReader(filePath);

                outputDevice.Init(audioFile);
                outputDevice.Play();     
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            outputDevice?.Stop();
            audioFile?.Dispose();
            outputDevice?.Dispose();
        }
    }

}
