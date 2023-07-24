using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;

namespace AudioDeviceSwitcher;

public class Log
{
    private static Log _instance;

    public static Log Instance
    {
        get
        {
            if (_instance == null)
                _instance = new Log();
            return _instance;
        }
    }

    public TextBlock referenceToText;
    public bool init;
    
    public List<string> loggedMessages;
    
    private Log()
    {
        loggedMessages = new List<string>();
    }

    public static void Info(string info)
    {
        Log.Instance.AddMessage(info);
    }

    private void AddMessage(string s)
    {
        loggedMessages.Add(s + '\n');
        if (init)
            referenceToText.Text += s+ '\n';
    }

    public void SetReferenceToText(TextBlock text)
    {
        init = true;
        referenceToText = text;
        referenceToText.Text = "";

        foreach (string s in loggedMessages)
        {
            referenceToText.Text += s;
        }
    }
}
