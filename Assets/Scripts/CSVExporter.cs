using System;
using System.IO;
using UnityEngine;

public class CSVExporter
{
    private const string _kReportDirectoryName = "RefBuddy_Reports";
    private const string _kReportFileExtension = ".csv";
    private const string _kReportSeparator = ",";

    private string[] _reportHeaders = new string[4]
    {
        "Team 1",
        "Timestamp",
        "Team 2",
        "Timestamp"
    };

    public void SetTeams(string team1Name, string team2Name)
    {
        _reportHeaders[0] = team1Name;
        _reportHeaders[2] = team2Name;
    }

    public void CreateReport()
    {
        VerifyDirectory();

        using (StreamWriter sw = File.CreateText(GetFilePath()))
        {
            string finalString = "";
            for (int i = 0; i < _reportHeaders.Length; i++)
            {
                if(!string.IsNullOrEmpty(finalString))
                {
                    finalString += _kReportSeparator;
                }
                finalString += _reportHeaders[i];
            }
            sw.WriteLine(finalString);
        }
    }

    public void ClearReport()
    {
        VerifyDirectory();
        VerifyFile();

        FileStream fileStream = File.Open(GetFilePath(), FileMode.Open);
        fileStream.SetLength(0);
        fileStream.Close();
    }

    public void AppendToReport(string[] strings)
    {
        VerifyDirectory();
        VerifyFile();

        using (StreamWriter sw = File.AppendText(GetFilePath()))
        {
            string finalString = string.Empty;
            foreach (string str in strings)
            {
                if (!string.IsNullOrEmpty(finalString))
                {
                    finalString += _kReportSeparator;
                }
                finalString += str;
            }
            sw.WriteLine(finalString);
        }
    }

    private void VerifyDirectory()
    {
        string dir = GetDirectoryPath();
        if(!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }

    private void VerifyFile()
    {
        string file = GetFilePath();
        if(!File.Exists(file))
        {
            CreateReport();
        }
    }

    private string GetDirectoryPath()
    {
        return Application.persistentDataPath + "/" + _kReportDirectoryName;
    }

    private string GetFilePath()
    {
        return Path.Combine(GetDirectoryPath(), string.Format("{0}_VS_{1}_{2}", _reportHeaders[0], _reportHeaders[2], DateTime.Now.ToString("dd-MM-yyyy")) + _kReportFileExtension);
    }
}
