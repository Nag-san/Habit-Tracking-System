Imports System.Security.Cryptography
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel
Imports MySql.Data.MySqlClient

Public Class Form1
    Dim connstring As String = "server=localhost;userid=localhost;password=root;database=habits"
    Dim conn As New MySqlConnection(connstring)
    Dim pwd As String
    Dim username1 As String
    Public Sub Form1_load(sender As Object, e As EventArgs) Handles MyBase.Load
        conn.Open()
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            conn.Open()
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try

        username1 = TextBox1.Text
        pwd = TextBox2.Text
        Dim dr As String = "-"
        Dim sqlcmd As New MySqlCommand
        Try
            sqlcmd = New MySqlCommand("select userpwd from users where username = " + "'" + username1 + "'", conn)
            dr = sqlcmd.ExecuteScalar()
            If dr = "" Then
                Label9.Text = "The account doesnt exist, please signup!"
                Return
            End If
        Catch ex As Exception
            Console.WriteLine(ex.ToString())

        End Try

        If dr = pwd Then
            sqlcmd = New MySqlCommand("select userid from users where username = " + "'" + username1 + "'" + " and userpwd = '" + pwd + "'", conn)
            Dim userid As String
            userid = sqlcmd.ExecuteScalar()
            Form2.Label13.Text = userid
            Form2.Label3.Text = "Hii " + username1 + "  (n_n)"
            Me.Hide()
            Form2.Show()
        Else
            Label9.Text = "Oops, Wrong password!"
        End If

        TextBox1.Text = ""
        TextBox2.Text = ""
        conn.Close()

    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            conn.Open()
        Catch ex As Exception
            Console.WriteLine(ex.ToString())
        End Try

        Dim username = TextBox3.Text
        Dim pwd = TextBox4.Text
        Dim cpwd = TextBox5.Text
        Dim sqlcmd As New MySqlCommand
        Dim dr As String = "-"
        Dim a As String = 0
        Dim msg As String

        If (username = "") Then
            Label8.Text = "Please enter your name!"
            Return
        ElseIf pwd = "" Then
            Label8.Text = "Please enter a password!"
            Return
        End If

        If (pwd = cpwd) Then
            Console.WriteLine("Passwords match")
        Else
            Label8.Text = "The passwords don't match!"
            Return
        End If

        Try
            sqlcmd = New MySqlCommand("select usercount from usercount", conn)
            dr = sqlcmd.ExecuteScalar()
            Console.WriteLine(dr)
        Catch ex As Exception
            Console.WriteLine(ex.ToString())
        End Try

        Convert.ToInt16(dr)
        dr = dr + 1

        Try

            Dim query As String = "Insert into users values (" + dr + "," + "'" + username + "'" + ",'" + pwd + "'," + "'Enter a message'" + "," + a + "," + a + ")"
            Console.WriteLine(query)
            sqlcmd = New MySqlCommand(query, conn)
            sqlcmd.ExecuteNonQuery()
            sqlcmd = New MySqlCommand("Update usercount set usercount =" + dr, conn)
            sqlcmd.ExecuteNonQuery()
        Catch ex As Exception
            Console.WriteLine(ex.ToString())
        End Try

        msg = InputBox("Give a message to motivate yourself!")

        sqlcmd = New MySqlCommand("update users set usermsg = """ + msg.ToString() + """ where userid =" + dr, conn)
        sqlcmd.ExecuteNonQuery()

        Label8.Text = "Account created succesfully!"
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
        conn.Close()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Close()
    End Sub


End Class
