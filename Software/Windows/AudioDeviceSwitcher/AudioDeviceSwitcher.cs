using System;
using System.Collections.Generic;
using System.Linq;
using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;

namespace AudioDeviceSwitcher;

public class AudioDeviceSwitcher
{
    private static AudioDeviceSwitcher _instance;

    public static AudioDeviceSwitcher Instance
    {
        get
        {
            if (_instance == null)
                _instance = new AudioDeviceSwitcher();
            return _instance;
        }
    }
    
    private AudioSwitcher.AudioApi.CoreAudio.CoreAudioController _controller;
    public string currentAudioDevice
    {
        private set;
        get;
    }

    public AudioDeviceSwitcher()
    {
        _controller = new AudioSwitcher.AudioApi.CoreAudio.CoreAudioController();
        currentAudioDevice = GetCurrentDevice();
    }
    
    public void SwitchAudioDevice(string deviceName)
    {
        var device = _controller.GetPlaybackDevices()
            .FirstOrDefault(dev => dev.FullName.Equals(deviceName, StringComparison.OrdinalIgnoreCase));

        if (device != null)
        {
            Log.Info($"Setting {device.FullName} to standard output");
            device.SetAsDefault();
            device.SetAsDefaultCommunications();
        }
        else
        {
            
            Log.Info($"{deviceName} not found....Sad..");
        }
    }

    public List<string> GetCurrentDevices()
    {
        List<string> returnList = new List<string>();
        foreach (CoreAudioDevice device in _controller.GetPlaybackDevices())
        {
            returnList.Add(device.FullName);
        }
        return returnList;
    }

    public string GetCurrentDevice()
    {
        return _controller.GetDefaultDevice(DeviceType.Playback, Role.Multimedia).FullName;
    }
}
