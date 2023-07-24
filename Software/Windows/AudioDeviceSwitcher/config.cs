using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;

namespace AudioDeviceSwitcher
{
    public class Config
    {
        private static Config _instance;
        private string _configPath = "config.json";
        private Dictionary<string, string> _configs;

        public static Config Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Config();
                    _instance.LoadConfig();
                }

                return _instance;
            }
            private set => _instance = value;
        }

        public delegate string OnSave();

        public event OnSave OnSaveEvent;

        public void ClickedSave()
        {
            OnSaveEvent?.Invoke();
            SaveConfig();
        }

        private Config()
        {
            _configs = new Dictionary<string, string>();
        }

        private void SaveConfig()
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(_configs);
                File.WriteAllText(_configPath, jsonString);
                Log.Info($"Config saved to {_configPath}");
            }
            catch (Exception ex)
            {
                Log.Info("An error occurred while saving the configuration: " + ex.Message);
            }
        }

        private void LoadConfig()
        {
            try
            {
                if (File.Exists(_configPath))
                {
                    string jsonString = File.ReadAllText(_configPath);
                    _configs = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
                    Log.Info("Config loaded.");
                }
                else
                {
                    Log.Info("Configuration file not found.");
                }
            }
            catch (Exception ex)
            {
                Log.Info("An error occurred while loading the configuration: " + ex.Message);
            }
        }

        public void SaveData(string key, string jsonString)
        {
            if (_configs.ContainsKey(key))
            {
                _configs[key] = jsonString;
            }
            else
            {
                _configs.Add(key, jsonString);
            }
        }

        public T LoadData<T>(string key)
        {
            if (_configs.ContainsKey(key))
            {
                string value = _configs[key];
                
                if (!string.IsNullOrEmpty(value))
                    try
                    {
                        return JsonConvert.DeserializeObject<T>(value);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
            }

            Log.Info($"Key or Value \"{key}\" not found in the configuration.");
            return default;
        }
    }
}