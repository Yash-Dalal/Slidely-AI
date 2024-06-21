Imports System.Net.Http
Imports Newtonsoft.Json
Imports System.Windows.Forms

Public Class CreateSubmissionForm
    Private ReadOnly client As New HttpClient()

    Private WithEvents Timer1 As New System.Windows.Forms.Timer
    Private stopwatchRunning As Boolean = False
    Private stopwatchStartTime As DateTime

    Private Sub btnToggleStopwatch_Click(sender As Object, e As EventArgs) Handles btnToggleStopwatch.Click
        If Not stopwatchRunning Then
            ' Start the stopwatch
            stopwatchStartTime = DateTime.Now
            Timer1.Start()
            btnToggleStopwatch.Text = "Pause"
        Else
            ' Pause the stopwatch
            Timer1.Stop()
            btnToggleStopwatch.Text = "Resume"
        End If

        stopwatchRunning = Not stopwatchRunning
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        ' Calculate elapsed time since stopwatch started
        Dim elapsed As TimeSpan = DateTime.Now - stopwatchStartTime

        ' Display elapsed time in hh:mm:ss format
        lblStopwatchTime.Text = String.Format("{0:D2}:{1:D2}:{2:D2}", elapsed.Hours, elapsed.Minutes, elapsed.Seconds)
    End Sub

    Private Sub CreateSubmissionForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Interval = 1000 ' Set the interval to 1 second
        stopwatchRunning = False
        btnToggleStopwatch.Text = "Start"
        lblStopwatchTime.Text = "00:00:00"
    End Sub

    Private Async Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Try
            ' Create a dictionary to hold the form data
            Dim formData As New Dictionary(Of String, String) From {
                {"name", NameTextBox.Text},
                {"email", EmailTextBox.Text},
                {"phone", PhoneTextBox.Text},
                {"github_link", GitHubTextBox.Text},
                {"stopwatch_time", lblStopwatchTime.Text}
            }

            ' Serialize the dictionary to JSON
            Dim json As String = JsonConvert.SerializeObject(formData)

            ' Create the HTTP content
            Dim content As New StringContent(json, Encoding.UTF8, "application/json")

            ' Send the POST request
            Dim response As HttpResponseMessage = Await client.PostAsync("http://localhost:3000/submit", content)

            ' Display the response status
            If response.IsSuccessStatusCode Then
                MessageBox.Show("Submission successful!")
            Else
                MessageBox.Show("Submission failed: " & response.StatusCode)
            End If
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message)
        End Try
    End Sub
End Class
