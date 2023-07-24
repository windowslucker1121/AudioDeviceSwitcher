using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AudioDeviceSwitcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool debugEnabled;
        private AudioDeviceSwitcher _switcher;
        private Dictionary<ComboBox, DropDownAudio> list;
        
        public MainWindow()
        {
            _switcher = new AudioDeviceSwitcher();
            list = new Dictionary<ComboBox, DropDownAudio>();
            InitializeComponent();
            DropDownAudio headset = new DropDownAudio(dropdown_audioDevice_headset,"headsetDevice");
            DropDownAudio speaker = new DropDownAudio(dropdown_audioDevice_speaker,"speakerDevice");
            
            list.Add(headset.GetBox(),headset);
            list.Add(speaker.GetBox(),speaker);
            
            Log.Info(_switcher.currentAudioDevice);
        }

        private void ButtonApply_OnClick(object sender, RoutedEventArgs e)
        {
            Config.Instance.ClickedSave();
        }

        private void ButtonDebug_OnClick(object sender, RoutedEventArgs e)
        {
            debugEnabled = !debugEnabled;

            DebugLogScrollViewer.Visibility = debugEnabled ? Visibility.Visible : Visibility.Collapsed;

            DebugLogScrollViewer.Height = debugEnabled ? 200 : 0;
            
            if (debugEnabled)
                this.Height += 200;
            else
                this.Height -= 200;

            Log.Instance.SetReferenceToText(DebugLog);
        }

        private void Dropdown_audioDevice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                list[(ComboBox)sender].NewSelection(e);
            }
            catch (Exception exception)
            {
                Log.Info(exception.ToString());
                // throw;
            }

        }

        private void ManualSwitch_OnClick(object sender, RoutedEventArgs e)
        {
            if (dropdown_audioDevice_headset.Text == dropdown_audioDevice_speaker.Text)
            {
                MessageBox.Show("Both devices are equal, change one.");
                return;
            }

            string currentDevice = AudioDeviceSwitcher.Instance.GetCurrentDevice();
            string deviceHeadset = list[dropdown_audioDevice_headset].GetCurrentSelectedDevice();
            string deviceSpeaker = list[dropdown_audioDevice_speaker].GetCurrentSelectedDevice();
            string targetDevice = "";
            
            if (currentDevice == deviceHeadset)
                targetDevice = list[dropdown_audioDevice_speaker].GetCurrentSelectedDevice();
            else if (currentDevice == deviceSpeaker)
                targetDevice = list[dropdown_audioDevice_headset].GetCurrentSelectedDevice();

            if (string.IsNullOrEmpty(targetDevice))
            {
                Log.Info("TargetDevice Variable is null, skipping...");
                return;
            }
            AudioDeviceSwitcher.Instance.SwitchAudioDevice(targetDevice);
        }
    }
}