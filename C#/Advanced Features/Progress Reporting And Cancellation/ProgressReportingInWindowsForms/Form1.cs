﻿using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GemBox.Spreadsheet;

public partial class MainForm : Form
{
    public MainForm()
    {
        // If using Professional version, put your serial key below.
        SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
        // Use Trial Mode
        SpreadsheetInfo.FreeLimitReached += (eventSender, args) => args.FreeLimitReachedAction = FreeLimitReachedAction.ContinueAsTrial;
        InitializeComponent();
    }

    private async void loadButton_Click(object sender, EventArgs e)
    {
        // Capture the current context on UI thread
        var context = SynchronizationContext.Current;

        // Create load options
        var loadOptions = new XlsxLoadOptions();
        loadOptions.ProgressChanged += (eventSender, args) =>
        {
            var percentage = args.ProgressPercentage;
            // Invoke on UI thread
            context.Post(progressPercentage =>
            {
                // Update UI
                this.progressBar.Value = (int)progressPercentage;
                this.percentageLabel.Text = progressPercentage.ToString() + "%";
            }, percentage);
        };

        this.percentageLabel.Text = "0%";
        // Use tasks to run the load operation in a new thread.
        var file = await Task.Run(() => ExcelFile.Load("LargeFile.xlsx", loadOptions));
    }
}

