using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class CSVExporter
{
    private const string _kReportDirectoryName = "Report";
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

        List<string> actualStrings = strings.ToList();
        actualStrings.Insert(0, "");

        using (StreamWriter sw = File.AppendText(GetFilePath()))
        {
            string finalString = string.Empty;
            foreach (string str in actualStrings)
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
        return Application.dataPath + "/" + _kReportDirectoryName;
    }

    private string GetFilePath()
    {
        return Path.Combine(GetDirectoryPath(), string.Format("{0}_VS_{1}_{2}", _reportHeaders[0], _reportHeaders[2], DateTime.UtcNow.ToString("dd-mm-yyyy")) + _kReportFileExtension);
    }
}
