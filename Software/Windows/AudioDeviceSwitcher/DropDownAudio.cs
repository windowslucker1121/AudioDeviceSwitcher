using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Management;
using Newtonsoft.Json;

namespace AudioDeviceSwitcher;

public class DropDownAudio
{
    private System.Collections.ObjectModel.ObservableCollection<string> AudioDevices = new System.Collections.ObjectModel.ObservableCollection<string>();

    private string _saveKey;
    private MySettings settings;
    private ComboBox reference;
    
    public DropDownAudio(ComboBox box, string identifier = "none")
    {
        reference = box;
        _saveKey = identifier;
        settings = new MySettings();
        Config.Instance.OnSaveEvent += SaveSettings;

        LoadCurrentAvailableAudioDevices(box);
        
        
        MySettings _loadedSettings = Config.Instance.LoadData<MySettings>(_saveKey);
        
        if (_loadedSettings != null)
        {
            ApplySettings(_loadedSettings);
        }
    }

    public void LoadCurrentAvailableAudioDevices(ComboBox box)
    {
        try  
        {  
            // ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_SoundDevice");  
            // foreach (ManagementObject queryObj in searcher.Get())  
            // {  
            //     AudioDevices.Add((string)queryObj["Name"]);  
            // }  
            foreach (string device in AudioDeviceSwitcher.Instance.GetCurrentDevices())
            {
                AudioDevices.Add(device);
            }
        }  
        catch (ManagementException e)  
        {  
            MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);  
        }  
        box.ItemsSource = AudioDevices;
    }
    public string SaveSettings()
    {
        MySettings settings = GetSettings();
        string jsonString = JsonConvert.SerializeObject(settings);
        Config.Instance.SaveData(this._saveKey, jsonString);
        return jsonString;
    }

    public void NewSelection(SelectionChangedEventArgs e)
    {
        settings.selectedAudioDevice = e.AddedItems[0].ToString();
    }
    public ComboBox GetBox()
    {
        return reference;
    }
    private MySettings GetSettings()
    {
        return settings;
    }

    private void ApplySettings(MySettings settings)
    {
        ComboBox box = GetBox();
        this.settings.selectedAudioDevice = settings.selectedAudioDevice;
        // box.SelectedIndex = box.Items.IndexOf(this.settings.selectedAudioDevice);
        box.Text = this.settings.selectedAudioDevice;
    }

    public string GetCurrentSelectedDevice()
    {
        return this.settings.selectedAudioDevice;
    }
    
    public class MySettings
    {
        public string selectedAudioDevice = "";
    }

}