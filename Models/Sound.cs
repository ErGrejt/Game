using Avalonia.Interactivity;
using NAudio.Wave;
using Game.ViewModels;

namespace Game.Models;

public class Sound
{
    public void SoundOnButtonClick(string path)
    {
        using (var outputDevice = new WaveOutEvent())
        using (var audioFile = new Mp3FileReader(path))
        {
            outputDevice.Init(audioFile);
            outputDevice.Play();
            
            while (outputDevice.PlaybackState == PlaybackState.Playing)
            {
                System.Threading.Thread.Sleep(100);
            }
        }
    }
}