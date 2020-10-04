using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVExporter
{
    private const string reportDirectoryName = "Report";
    private const string reportFileName = "report.csv";
    private const string reportSeparator = ",";

    private string[] reportHeaders = new string[4]
    {
        "Team 1",
        "Timestamp",
        "Team 2",
        "Timestamp"
    };

    public void SetTeams(string team1Name, string team2Name)
    {

    }

    public void CreateReport()
    {
        VerifyDirectory();

        using (StreamWriter sw = File.CreateText(GetFilePath()))
        {
            string finalString = "";
            for (int i = 0; i < reportHeaders.Length; i++)
            {
                if(!string.IsNullOrEmpty(finalString))
                {
                    finalString += reportSeparator;
                }
                finalString += reportHeaders[i];
            }
            sw.WriteLine(finalString);
        }
    }

    public void AppendToReport(string[] strings)
    {
        VerifyDirectory();
        VerifyFile();
        using(StreamWriter sw = File.AppendText(GetFilePath()))
        {
            string finalString = string.Empty;
            for (int i = 0; i < strings.Length; i++)
            {
                if(!string.IsNullOrEmpty(finalString))
                {
                    finalString += reportSeparator;
                }
                finalString += strings[i];
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
        return Application.dataPath + "/" + reportDirectoryName;
    }

    private string GetFilePath()
    {
        return GetDirectoryPath() + "/" + reportFileName;
    }
}
