Imports System
Imports System.Data.SqlClient
Imports System.Security.Cryptography
Imports System.Text
Imports System.Windows.Forms

Public Class Form1
    Public Sub New()
        InitializeComponent()
    End Sub

    ' Hash password using SHA256
    Private Shared Function HashPassword(password As String) As String
        Using sha256Hash As SHA256 = SHA256.Create()
            Dim bytes As Byte() = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password))
            Dim builder As New StringBuilder()
            For i As Integer = 0 To bytes.Length - 1
                builder.Append(bytes(i).ToString("x2"))
            Next
            Return builder.ToString()
        End Using
    End Function

    ' Button click event handler
    Private Sub button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim username As String = txtUsername.Text
        Dim password As String = txtPassword.Text
        Dim hashedPassword As String = HashPassword(password) ' Hash the password

        ' Define the connection string
        Dim connectionString As String = "Data Source=DESKTOP-S0EL170;Initial Catalog=Gearup;Integrated Security=True"

        Try
            ' Establish a connection to the database
            Using connection As New SqlConnection(connectionString)
                connection.Open()

                ' Check if the username already exists
                Dim checkUserQuery As String = "SELECT COUNT(*) FROM Users WHERE Username = @Username"
                Using checkCmd As New SqlCommand(checkUserQuery, connection)
                    checkCmd.Parameters.AddWithValue("@Username", username)
                    Dim userCount As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

                    If userCount > 0 Then
                        ' User already exists, inform the user
                        MessageBox.Show("Username already exists. Please choose a different username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Else
                        ' If the user doesn't exist, insert the new user
                        Dim insertQuery As String = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)"
                        Using insertCmd As New SqlCommand(insertQuery, connection)
                            insertCmd.Parameters.AddWithValue("@Username", username)
                            insertCmd.Parameters.AddWithValue("@Password", hashedPassword)

                            Dim rowsAffected As Integer = insertCmd.ExecuteNonQuery()

                            If rowsAffected > 0 Then
                                ' If the insert is successful, show success message
                                MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Else
                                ' If something went wrong with the insert
                                MessageBox.Show("Registration failed. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End If
                        End Using
                    End If
                End Using
            End Using
        Catch ex As Exception
            ' Handle any errors that occur during the database operations
            MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class
