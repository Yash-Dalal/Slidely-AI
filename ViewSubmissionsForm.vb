Imports System.Net.Http
Imports Newtonsoft.Json
Imports System.Windows.Forms

Public Class ViewSubmissionsForm
    Private ReadOnly client As New HttpClient()
    Private submissions As List(Of Dictionary(Of String, String))
    Private currentIndex As Integer = 0

    Private Async Sub ViewSubmissionsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await LoadSubmissions()
        DisplaySubmission()
    End Sub

    Private Async Function LoadSubmissions() As Task
        Try
            Dim response As HttpResponseMessage = Await client.GetAsync("http://localhost:3000/read")
            If response.IsSuccessStatusCode Then
                Dim json As String = Await response.Content.ReadAsStringAsync()
                submissions = JsonConvert.DeserializeObject(Of List(Of Dictionary(Of String, String)))(json)
            Else
                MessageBox.Show("Failed to load submissions.")
            End If
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message)
        End Try
    End Function

    Private Sub DisplaySubmission()
        If submissions IsNot Nothing AndAlso submissions.Count > 0 AndAlso currentIndex >= 0 AndAlso currentIndex < submissions.Count Then
            Dim submission As Dictionary(Of String, String) = submissions(currentIndex)
            NameTextBox.Text = submission("name")
            EmailTextBox.Text = submission("email")
            PhoneTextBox.Text = submission("phone")
            GitHubTextBox.Text = submission("github_link")
            lblStopwatchTime.Text = submission("stopwatch_time")
        End If
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        If currentIndex < submissions.Count - 1 Then
            currentIndex += 1
            DisplaySubmission()
        End If
    End Sub

    Private Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        If currentIndex > 0 Then
            currentIndex -= 1
            DisplaySubmission()
        End If
    End Sub
End Class
