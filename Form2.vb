Imports System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel
Imports Google.Protobuf
Imports Google.Protobuf.WellKnownTypes
Imports MySql.Data.MySqlClient
Imports Mysqlx.Datatypes
Imports Mysqlx.Serialization
Imports Newtonsoft.Json.Linq
Imports ZstdSharp.Unsafe

Public Class Form2
    Dim connstring As String = "server=localhost;userid=localhost;password=root;database=habits"
    Dim conn As New MySqlConnection(connstring)
    Dim username1 As String
    Dim userid1 As String
    Dim query As String
    Dim sqlcmd As New MySqlCommand
    Dim habitid1 As String
    Dim habitname1 As String

    Public Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim usermsg1 As String
        Try
            conn.Open()
        Catch ex As Exception
            Console.WriteLine(ex.ToString())
        End Try
        userid1 = Label13.Text.ToString()
        Console.WriteLine(userid1)
        query = "select habitname from goodhabits where userid =" + userid1
        sqlcmd = New MySqlCommand(query, conn)
        Dim dr As MySqlDataReader
        dr = sqlcmd.ExecuteReader()
        While dr.Read()
            Dim item = dr.GetString(0)
            ListBox1.Items.Add(item.ToString())
            ComboBox1.Items.Add(item.ToString())
        End While
        dr.Close()
        If ListBox1.SelectedIndex = -1 And ListBox1.Items.Count = 1 Then
            ListBox1.SelectedIndex = 0
        End If
        query = "select habitname from badhabits where userid =" + userid1
        sqlcmd = New MySqlCommand(query, conn)
        dr = sqlcmd.ExecuteReader()
        While dr.Read()
            Dim item = dr.GetString(0)
            ListBox2.Items.Add(item.ToString())
            ComboBox2.Items.Add(item.ToString())
        End While
        dr.Close()
        If ListBox2.SelectedIndex = -1 And ListBox2.Items.Count = 1 Then
            ListBox2.SelectedIndex = 0
        End If

        query = "select username from users where userid =" + userid1
        sqlcmd = New MySqlCommand(query, conn)
        username1 = sqlcmd.ExecuteScalar()
        Label10.Text = "You've got to keep going! " + username1 + "  *(@-@)*"
        query = "select usermsg from users where userid =" + userid1
        sqlcmd = New MySqlCommand(query, conn)
        usermsg1 = sqlcmd.ExecuteScalar()
        TextBox3.Text = usermsg1
        changeimg()

        query = "select entry from user_journal where userid = " + userid1 + " and date = " + System.DateTime.Today.ToShortDateString()
        sqlcmd = New MySqlCommand(query, conn)
        dr = sqlcmd.ExecuteReader()
        If Not dr.Read() Then
            RichTextBox2.Text = "Journal your thoughts to gain clarity..."
        Else
            RichTextBox2.Text = sqlcmd.ExecuteScalar()
        End If
        dr.Close()

        TextBox4.Text = username1

    End Sub
    Public Sub TabPage1_Click(sender As Object, e As EventArgs) Handles TabPage1.Click
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim goodhabit As String
        Dim dr As String
        If TextBox2.Text = "" Then
            Label4.Text = "Please enter any habit!"
            Return
        End If
        goodhabit = TextBox2.Text
        ListBox1.Items.Add(goodhabit.ToString())
        ComboBox1.Items.Add(goodhabit.ToString())
        query = "select goodcount from users where userid =" + userid1
        sqlcmd = New MySqlCommand(query, conn)
        dr = sqlcmd.ExecuteScalar()
        Console.WriteLine(dr)
        dr = dr + 1
        query = "insert into goodhabits (habitid, userid, habitname) values (" + dr + "," + userid1 + "," + "'" + goodhabit + "'" + ")"
        habitid1 = dr
        sqlcmd = New MySqlCommand(query, conn)
        sqlcmd.ExecuteNonQuery()
        query = "update users set goodcount = goodcount + 1 where userid=" + userid1
        sqlcmd = New MySqlCommand(query, conn)
        sqlcmd.ExecuteNonQuery()
        TextBox2.Text = ""
        Label4.Text = $"A new habit is a new {Environment.NewLine}doorway to better life! {Environment.NewLine} *(^o^)*"
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim dr As String
        Dim badhabit As String
        If TextBox1.Text = "" Then
            Label11.Text = "Please enter any habit!"
        End If
        badhabit = TextBox1.Text
        ListBox2.Items.Add(badhabit.ToString())
        ComboBox2.Items.Add(badhabit.ToString())
        query = "select badcount from users where userid =" + userid1
        sqlcmd = New MySqlCommand(query, conn)
        dr = sqlcmd.ExecuteScalar()
        dr = dr + 1
        query = "insert into badhabits (habitid, userid, habitname) values (" + dr + "," + userid1 + "," + "'" + badhabit + "'" + ")"
        sqlcmd = New MySqlCommand(query, conn)
        sqlcmd.ExecuteNonQuery()
        habitid1 = dr
        query = "update users set badcount = badcount + 1 where userid=" + userid1
        sqlcmd = New MySqlCommand(query, conn)
        sqlcmd.ExecuteNonQuery()
        TextBox1.Text = ""
        Label11.Text = $"First step to a better {Environment.NewLine}lifestyle is admitting flaws! {Environment.NewLine} (^o^)"
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If ListBox1.SelectedIndex = -1 Then
            Label4.Text = "Please select a habit!"
            Return
        End If
        habitname1 = ListBox1.SelectedItem.ToString()
        Try
            ListBox1.SelectedIndex = 0
        Catch ex As Exception
            Console.WriteLine("Nothing")
        End Try
        ListBox1.Items.Remove(habitname1)
        ComboBox1.Items.Remove(habitname1)
        query = "delete from goodhabits where userid = " + userid1 + " and habitname = " + "'" + habitname1 + "'"
        sqlcmd = New MySqlCommand(query, conn)
        sqlcmd.ExecuteNonQuery()
        Label4.Text = $"A good habit is removed!{Environment.NewLine} (o_O)"
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If ListBox2.SelectedIndex = -1 Then
            Label11.Text = "Please select a habit!"
        End If
        habitname1 = ListBox2.SelectedItem.ToString()
        Try
            ListBox2.SelectedIndex = 0
        Catch ex As Exception
            Console.WriteLine("Nothing")
        End Try
        ListBox2.Items.Remove(habitname1)
        ComboBox2.Items.Remove(habitname1)
        query = "delete from badhabits where userid = " + userid1 + " and habitname = " + "'" + habitname1 + "'"
        sqlcmd = New MySqlCommand(query, conn)
        sqlcmd.ExecuteNonQuery()
        Label11.Text = $"A bad habit is removed!{Environment.NewLine} (*_*)"
    End Sub


    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        Label4.Text = ""
        If ListBox1.SelectedItem = "" Then
            Return
        End If
        Label1.Text = ListBox1.SelectedItem.ToString()
    End Sub

    Private Sub ListBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox2.SelectedIndexChanged
        Label11.Text = ""
        If ListBox2.SelectedItem = "" Then
            Return
        End If
        Label2.Text = ListBox2.SelectedItem.ToString()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If ListBox1.SelectedIndex = -1 Then
            Label4.Text = "Please select a habit"
            Return
        End If
        habitname1 = ListBox1.SelectedItem.ToString()
        Dim a As String
        Dim dc As Integer
        Dim dr As Object
        Dim jarray1 As New JArray

        query = "select json_keys(habit) from goodhabits where habitname =" + "'" + habitname1 + "'" + "and userid = " + userid1
        sqlcmd = New MySqlCommand(query, conn)
        a = DateTime.Today().ToShortDateString
        dc = 1
        If IsDBNull(sqlcmd.ExecuteScalar()) Then
            query = "update goodhabits set habit = '{""" + a + """:" + dc.ToString() + "}' where userid = " + userid1 + " and habitname = " + "'" + habitname1 + "'"
            sqlcmd = New MySqlCommand(query, conn)
            sqlcmd.ExecuteNonQuery()
            Label4.Text = $"That's a good step (*-*){Environment.NewLine}" + habitname1 + " " + dc.ToString() + " time(s)!"
            Return
        End If
        sqlcmd = New MySqlCommand(query, conn)
        dr = sqlcmd.ExecuteScalar()
        jarray1 = JArray.Parse(dr)
        For Each item As JValue In jarray1
            If item = a Then
                a = DateTime.Today().ToShortDateString
                query = "select json_extract(habit, '$.""" + a + """') from goodhabits where userid = " + userid1 + " and habitname = " + "'" + habitname1 + "'"
                sqlcmd = New MySqlCommand(query, conn)
                dc = sqlcmd.ExecuteScalar()
                Convert.ToInt32(dc)
                dc = dc + 1
                Console.WriteLine(item)
                query = "update goodhabits set habit = json_set( habit, '$.""" + a + """', " + dc.ToString() + ") where userid =" + userid1 + " and habitname = " + "'" + habitname1 + "'"
                Console.WriteLine(query)
                sqlcmd = New MySqlCommand(query, conn)
                sqlcmd.ExecuteNonQuery()
                Label4.Text = $"That's a good step (*-*){Environment.NewLine}" + habitname1 + " " + dc.ToString() + " time(s)!"
                Return
            End If
        Next

        a = DateTime.Today().ToShortDateString
        dc = 1
        query = "update goodhabits set habit = json_merge(habit, '{""" + a + """:" + dc.ToString() + "}') where userid = " + userid1 + " and habitname = " + "'" + habitname1 + "'"
        sqlcmd = New MySqlCommand(query, conn)
        sqlcmd.ExecuteNonQuery()
        Label4.Text = $"That's a good step (*-*){Environment.NewLine}" + habitname1 + " " + dc.ToString() + " time(s)!"

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If ListBox2.SelectedIndex = -1 Then
            Label11.Text = "Please enter a habit!"
            Return
        End If
        habitname1 = ListBox2.SelectedItem.ToString()
        Dim a As String
        Dim dc As Integer
        Dim dr As Object
        Dim jarray1 As New JArray
        query = "select json_keys(habit) from badhabits where habitname =" + "'" + habitname1 + "'" + "and userid = " + userid1
        sqlcmd = New MySqlCommand(query, conn)
        a = DateTime.Today().ToShortDateString
        dc = 1
        If IsDBNull(sqlcmd.ExecuteScalar()) Then
            query = "update badhabits set habit = '{""" + a + """:" + dc.ToString() + "}' where userid = " + userid1 + " and habitname = " + "'" + habitname1 + "'"
            sqlcmd = New MySqlCommand(query, conn)
            sqlcmd.ExecuteNonQuery()
            Label11.Text = $"That's okay! (o_o){Environment.NewLine}" + habitname1 + " " + dc.ToString() + " time(s)!"
            Return
        End If
        sqlcmd = New MySqlCommand(query, conn)
        dr = sqlcmd.ExecuteScalar()
        jarray1 = JArray.Parse(dr)
        For Each item As JValue In jarray1
            If item = a Then
                query = "select json_extract(habit, '$.""" + a + """') from badhabits where userid = " + userid1 + " and habitname = " + "'" + habitname1 + "'"
                sqlcmd = New MySqlCommand(query, conn)
                dc = sqlcmd.ExecuteScalar()
                Convert.ToInt32(dc)
                dc = dc + 1
                query = "update badhabits set habit = json_set( habit, '$.""" + a + """', " + dc.ToString() + ") where userid =" + userid1 + " and habitname = " + "'" + habitname1 + "'"
                sqlcmd = New MySqlCommand(query, conn)
                sqlcmd.ExecuteNonQuery()
                Label11.Text = $"That's okay! (o_o){Environment.NewLine}" + habitname1 + " " + dc.ToString() + " time(s)!"
                Return
            End If
        Next
        a = DateTime.Today().ToShortDateString
        dc = 1
        query = "update badhabits set habit = json_merge(habit, '{""" + a + """:" + dc.ToString() + "}') where userid = " + userid1 + " and habitname = " + "'" + habitname1 + "'"
        sqlcmd = New MySqlCommand(query, conn)
        sqlcmd.ExecuteNonQuery()
        Label11.Text = $"That's okay! (o_o){Environment.NewLine}" + habitname1 + " " + dc.ToString() + " time(s)!"
    End Sub

    Private Sub TabPage2_Click(sender As Object, e As EventArgs) Handles TabPage2.Click
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Dim d_start As New Date
        Dim d_end As New Date
        Dim d_check As Object
        Dim habit As String
        Dim count As Integer
        Dim cnt As String
        Dim max As Integer = 0
        Dim max_ondate As New Date
        Dim diff As TimeSpan
        Dim r As New List(Of DateTime)
        Dim d As Integer
        Chart1.Series("Series1").Points.Clear()
        If ComboBox1.SelectedIndex = -1 Then
            Label8.Text = "Please select a habit!"
            Label9.Text = " "
            Return
        End If
        habit = ComboBox1.SelectedItem.ToString()
        d_start = DateTimePicker1.Value().ToShortDateString()
        d_end = d_start.Date.AddDays(7).ToShortDateString
        query = "select json_keys(habit) from goodhabits where userid = " + userid1 + " and habitname = " + "'" + habit + "'"
        sqlcmd = New MySqlCommand(query, conn)
        d_check = sqlcmd.ExecuteScalar()
        If d_check.ToString = "" Then
            Label8.Text = "Update the habit atleast once"
            Return
        End If
        Chart1.Series("Series1").LegendText = habit
        diff = d_end - d_start

        For d = 0 To diff.Days
            r.Add(d_start.Date.AddDays(d))
        Next

        Dim jarray1 As JArray = JArray.Parse(d_check)
        Dim j = jarray1.ToArray()
        d = 0
        Dim i = 0
        While i < r.Count
            cnt = 0
            Console.WriteLine(d)
            If d > j.Count - 1 Then
                Chart1.Series("Series1").Points.AddXY(r(i).ToShortDateString(), 0)
                i = i + 1
                Continue While
            End If
            d_check = j(d).ToString
            Console.WriteLine(d_check.ToString)
            If d_check >= d_start AndAlso d_check <= d_end Then
                query = "select json_extract(habit, '$.""" + d_check + """') from goodhabits where userid = " + userid1 + " and habitname = " + "'" + habit + "'"
                sqlcmd = New MySqlCommand(query, conn)
                cnt = sqlcmd.ExecuteScalar()
                Convert.ToInt32(cnt)
                If r(i).ToShortDateString = d_check.ToString() Then
                    Chart1.Series("Series1").Points.AddXY(d_check.ToString(), cnt)
                    count = cnt + count
                Else
                    Chart1.Series("Series1").Points.AddXY(r(i).ToShortDateString(), 0)
                End If
                If max < cnt Then
                    max = cnt
                    max_ondate = d_check
                End If
            End If
            If r(i).ToShortDateString > j(d).ToString Then
                d = d + 1
            ElseIf r(i).ToShortDateString = j(d).ToString Then
                i = i + 1
                d = d + 1
            Else
                i = i + 1
            End If
        End While
        Label8.Text = "Total " + count.ToString() + " time(s) in this week!!"
        Label9.Text = "You have the highest streak of " + max.ToString() + $"{Environment.NewLine}on " + max_ondate.Date() + " *(O_O)*"

    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Dim d_start As New Date
        Dim d_end As New Date
        Dim d_check As Object
        Dim habit As String
        Dim count As Integer
        Dim cnt As String
        Dim max As Integer = 0
        Dim max_ondate As New Date
        Dim diff As TimeSpan
        Dim i As Integer
        Dim d As Integer

        Chart1.Series("Series1").Points.Clear()
        If ComboBox2.SelectedIndex = -1 Then
            Label8.Text = "Please select a habit!"
            Label9.Text = " "
            Return
        End If
        habit = ComboBox2.SelectedItem.ToString()
        d_start = DateTimePicker1.Value().ToShortDateString()
        d_end = d_start.Date.AddDays(7)
        Console.WriteLine(d_end)
        diff = d_end - d_start

        Dim r As New List(Of DateTime)
        For d = 0 To diff.Days
            Try
                r.Add(d_start.Date.AddDays(d))
            Catch ex As Exception
                Console.WriteLine(ex.ToString)
            End Try

        Next
        query = "select json_keys(habit) from badhabits where userid = " + userid1 + " and habitname = " + "'" + habit + "'"
        sqlcmd = New MySqlCommand(query, conn)
        d_check = sqlcmd.ExecuteScalar()
        If d_check.ToString = "" Then
            Label8.Text = "Update the habit atleast once"
            Return
        End If
        Chart1.Series("Series1").LegendText = habit
        Dim jarray1 As JArray = JArray.Parse(d_check)
        Dim j = jarray1.ToArray()
        d = 0
        i = 0

        While i < r.Count
            cnt = 0
            If d > j.Count - 1 Then
                Chart1.Series("Series1").Points.AddXY(r(i).ToShortDateString(), 0)
                i = i + 1
                Continue While
            End If
            d_check = j(d).ToString
            If d_check >= d_start AndAlso d_check <= d_end Then
                query = "select json_extract(habit, '$.""" + d_check + """') from badhabits where userid = " + userid1 + " and habitname = " + "'" + habit + "'"
                sqlcmd = New MySqlCommand(query, conn)
                cnt = sqlcmd.ExecuteScalar()
                Convert.ToInt32(cnt)
                If r(i).ToShortDateString = j(d).ToString Then
                    Chart1.Series("Series1").Points.AddXY(d_check.ToString(), cnt)
                    count = cnt + count
                Else
                    Chart1.Series("Series1").Points.AddXY(r(i).ToShortDateString(), 0)
                End If
                If max < cnt Then
                    max = cnt
                    max_ondate = d_check
                End If

            End If
            If r(i).ToShortDateString > j(d).ToString Then
                d = d + 1
            ElseIf r(i).ToShortDateString = j(d).ToString Then
                d = d + 1
                i = i + 1
            Else
                i = i + 1
            End If
        End While
        Label8.Text = "Total " + count.ToString() + " time(s) in this week!!"
        Label9.Text = $"You have committed this sin{Environment.NewLine}" + max.ToString() + " times on " + max_ondate.Date() + " (-_-)"
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Form1.Show()
        Me.Close()
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Dim usermsg1 As String
        usermsg1 = TextBox3.Text
        sqlcmd = New MySqlCommand("update users set usermsg = """ + usermsg1.ToString() + """ where userid =" + userid1, conn)
        sqlcmd.ExecuteNonQuery()
        changeimg()
    End Sub

    Public Function changeimg()
        Dim num As Integer
        num = GetRandom(1, 5)
        Select Case num
            Case 1
                PictureBox1.Image = Image.FromFile("D:\Nagraj\VB\demo1\motivation.png")
                Label12.Text = $"You are the only constant hope{Environment.NewLine}for yourself, never give up!"
            Case 2
                PictureBox1.Image = Image.FromFile("D:\Nagraj\VB\demo1\Beige-Motivational-Quote-Dreams-Instagram-Post-1024x1024.png")
                Label12.Text = $"Most problems arise from your{Environment.NewLine}mind and inaction."
            Case 3
                PictureBox1.Image = Image.FromFile("D:\Nagraj\VB\demo1\Habit-Quotes-10-1024x1024.png")
                Label12.Text = $"May you realise the strength{Environment.NewLine}you possess"
            Case 4
                PictureBox1.Image = Image.FromFile("D:\Nagraj\VB\demo1\images.jpeg")
                Label12.Text = $"Why seek from outside when{Environment.NewLine}thy have within self"
            Case 5
                PictureBox1.Image = Image.FromFile("D:\Nagraj\VB\demo1\Thought-provoking-quotes-on-getting-rid-of-bad-habits.webp")
                Label12.Text = $"Let Gods be witness{Environment.NewLine}to your progress!"
        End Select
    End Function
    Public Function GetRandom(ByVal Min As Integer, ByVal Max As Integer) As Integer
        Static Generator As System.Random = New System.Random()
        Return Generator.Next(Min, Max)
    End Function

    Private Sub Datetimepicker3_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker3.ValueChanged
        RichTextBox1.Text = " "
        RichTextBox2.Text = " "
        Label15.Text = " "
        Dim d_val As Date = DateTimePicker3.Value().ToShortDateString
        If d_val > DateTime.Today().ToShortDateString Then
            RichTextBox1.Text = "Let's leave the future to your future self!"
            Return
        End If
        query = "select entry from user_journal where userid =" + userid1 + " and date = '" + d_val + "'"
        sqlcmd = New MySqlCommand(query, conn)
        Console.WriteLine(query)
        Dim dr As MySqlDataReader = sqlcmd.ExecuteReader()
        If Not dr.Read() Then
            RichTextBox1.Text = "No entry on this date!"
        Else
            If d_val = DateTime.Today().ToShortDateString Then
                sqlcmd = New MySqlCommand(query, conn)
                RichTextBox2.Text = dr.GetString(0)
            Else
                sqlcmd = New MySqlCommand(query, conn)
                RichTextBox1.Text = dr.GetString(0)
            End If
        End If
        dr.Close()
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Dim d_val = DateTime.Today.ToShortDateString()
        If Not DateTimePicker3.Value.ToShortDateString = d_val Then
            Return
        End If
        query = "select entry from user_journal where userid =" + userid1 + " and date = '" + d_val + "'"
        sqlcmd = New MySqlCommand(query, conn)
        Dim dr As MySqlDataReader = sqlcmd.ExecuteReader()
        If Not dr.Read() Then
            query = "insert into user_journal(userid, entry, date) values (" + userid1 + ",""" + RichTextBox2.Text + """,'" + DateTime.Today.ToShortDateString + "')"
        Else
            query = "update user_journal set entry = """ + RichTextBox2.Text + """ where userid = " + userid1 + " and date = '" + d_val + "'"
        End If
        dr.Close()
        Console.WriteLine(query)
        sqlcmd = New MySqlCommand(query, conn)
        sqlcmd.ExecuteNonQuery()
        Label15.Text = "Saved!"
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = 140
        Dim ask = MsgBox("Delete your account?", MsgBoxStyle.YesNoCancel)
        If ask = MsgBoxResult.Yes Then
            Timer1.Enabled = True
            Timer1.Interval = 2000
        Else
            Return
        End If
    End Sub

    Private Sub ticker1_tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        ProgressBar1.Increment(20)
        If ProgressBar1.Value >= 120 Then
            query = "delete from users where userid = " + userid1
            sqlcmd = New MySqlCommand(query, conn)
            sqlcmd.ExecuteNonQuery()
            query = "delete from goodhabits where userid = " + userid1
            sqlcmd = New MySqlCommand(query, conn)
            sqlcmd.ExecuteNonQuery()
            query = "delete from badhabits where userid = " + userid1
            sqlcmd = New MySqlCommand(query, conn)
            sqlcmd.ExecuteNonQuery()
            query = "delete from user_journal where userid = " + userid1
            sqlcmd = New MySqlCommand(query, conn)
            sqlcmd.ExecuteNonQuery()

            Timer1.Enabled = False
            Form1.Show()
            Me.Close()
            Return
        End If
        Dim a = ProgressBar1.Value
        Select Case a
            Case 20
                Label19.Text = "Deleting good habits (!_!)"
            Case 40
                Label19.Text = "Deleting bad habits (!_!)"
            Case 60
                Label19.Text = "Deleting your progress (!_!)"
            Case 80
                Label19.Text = "Deleting your journal entries *(!_!)*"
            Case 100
                ProgressBar1.Increment(20)
                Label19.Text = "Your account is deleted! (*o*)"
        End Select
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        If TextBox5.Text = "" Or TextBox6.Text = "" Then
            query = "update users set username = """ + TextBox4.Text.ToString + """ where userid = " + userid1
        ElseIf Not TextBox5.Text = TextBox6.Text Then
            Label22.Text = "Please recheck your password"
        Else
            query = "update users set username = """ + TextBox4.Text.ToString + """, userpwd = """ + TextBox5.Text.ToString + """ where userid = " + userid1
        End If
        username1 = TextBox4.Text
        sqlcmd = New MySqlCommand(query, conn)
        sqlcmd.ExecuteNonQuery()
        Label22.Text = "Changed succesfully!"
        Label10.Text = "You've got to keep going! " + username1 + "  (<*_*>)"
        Label3.Text = "Hii " + username1 + "  (n_n)"
    End Sub

End Class