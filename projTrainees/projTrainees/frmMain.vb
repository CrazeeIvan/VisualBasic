﻿Imports System.Data.Sql
Imports System.Data.SqlClient

Public Class frmMain
    Dim connectionString As String
    Dim cnn As New SqlConnection
    Dim dt As New DataTable
    Dim ds As New DataSet("Details")
    Dim intRecCount As Integer
    Dim intIndex As Integer = 0
    Dim newline As String = vbNewLine.ToString()
    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DisableInput()
        ConnectionToDatabase()
        DisplayRecord(intIndex)
    End Sub

    Private Sub ConnectionToDatabase()
        connectionString = "Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\blue20\Documents\VSprojects\VisualBasic\projTrainees\projTrainees\bin\Debug\dbTrainees.mdf;Integrated Security=True"
        cnn = New SqlConnection(connectionString)
        Try
            cnn.Open()
            Dim str As String = "SELECT * FROM tblDetails"
            Dim da As New SqlDataAdapter(str, cnn)
            da.MissingSchemaAction = MissingSchemaAction.AddWithKey
            da.Fill(ds, "tblDetails")
            dt = ds.Tables("tblDetails")
            intRecCount = dt.Rows.Count
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try
    End Sub

    Private Sub DisableInput()
        Try
            For Each Ctrl As Control In Me.Controls
                If TypeOf Ctrl Is TextBox Then
                    Ctrl.Enabled = False
                Else
                    Ctrl.Enabled = True
                End If
            Next
            For Each GroupBoxControl As Control In Me.Controls
                If TypeOf GroupBoxControl Is GroupBox Then
                    For Each Ctrl As Control In GroupBoxControl.Controls
                        If TypeOf Ctrl Is TextBox Then
                            Ctrl.Enabled = False
                        Else
                            Ctrl.Enabled = True
                        End If
                    Next
                End If
            Next
            dtDoB.Visible = True
            txtGender.Visible = True
            txtDoB.Visible = True
            dtDOB.Visible = False
            radMale.Visible = False
            radFemale.Visible = False
            dtDoB.Visible = False
        Catch ex As Exception
            MessageBox.Show("Unable to set up display! ")
        End Try
    End Sub

    Private Sub EnableInput()
        Try
            For Each Ctrl As Control In Me.Controls
                If TypeOf Ctrl Is TextBox Then
                    Ctrl.Enabled = True
                End If
            Next
            For Each GroupBoxControl As Control In Me.Controls
                If TypeOf GroupBoxControl Is GroupBox Then
                    For Each Ctrl As Control In GroupBoxControl.Controls
                        If TypeOf Ctrl Is TextBox Then
                            Ctrl.Enabled = True
                        End If
                    Next
                End If
            Next
            dtDoB.Visible = False
            txtGender.Visible = False
            txtDoB.Visible = False
            dtDOB.Visible = True
            radFemale.Visible = True
            radMale.Visible = True
            dtDoB.Visible = True
        Catch ex As Exception
            MessageBox.Show("Unable to connect to database!" & newline & "Original Error:" & newline & ex.ToString())
        End Try
    End Sub
    Private Sub DisplayRecord(Index As Integer)
        Try
            txtID.Text = dt.Rows(Index)("ID").ToString()
            txtFirstName.Text = dt.Rows(Index)("First Name").ToString()
            txtLastName.Text = dt.Rows(Index)("Last Name").ToString()
            txtAddress1.Text = dt.Rows(Index)("Address1").ToString()
            txtAddress2.Text = dt.Rows(Index)("Address2").ToString()
            txtDoB.Text = dt.Rows(Index)("DOB").ToString()
            dtDoB.Value = dt.Rows(Index)("DOB")


            txtCounty.Text = dt.Rows(Index)("County").ToString()
            txtCountry.Text = dt.Rows(Index)("Country").ToString()
            txtPhone.Text = dt.Rows(Index)("Phone").ToString()
            txtEmail.Text = dt.Rows(Index)("Email").ToString()
            txtDoB.Text = dt.Rows(Index)("DOB").ToString()
            If (dt.Rows(Index)("Gender").ToString()) = "True" Then
                txtGender.Text = "Male"
                radMale.Checked = True
                radFemale.Checked = False
            Else
                txtGender.Text = "Female"
                radFemale.Checked = True
                radMale.Checked = False
            End If
            txtNotes.Text = dt.Rows(Index)("Notes").ToString()
        Catch ex As Exception
            MessageBox.Show("Unable to connect to database!" & newline & "Original Error:" & newline & ex.ToString())
        End Try
    End Sub

    Function CheckForAlphaCharacters(ByVal StringToCheck As String)
        For i = 0 To StringToCheck.Length - 1
            If Not Char.IsLetter(StringToCheck.Chars(i)) Then
                Return False
            End If
        Next
        Return True
    End Function
    Private Sub btnFirst_Click(sender As Object, e As EventArgs) Handles btnFirst.Click
        intIndex = 0
        DisplayRecord(intIndex)
    End Sub
    Private Sub btnLast_Click(sender As Object, e As EventArgs) Handles btnLast.Click
        intIndex = intRecCount - 1
        DisplayRecord(intIndex)
    End Sub
    Private Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        If intIndex > 0 Then
            intIndex -= 1
            DisplayRecord(intIndex)
        End If
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        If intIndex < intRecCount - 1 Then
            intIndex += 1
            DisplayRecord(intIndex)
        End If
    End Sub
    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Try
            Dim result As Integer = MessageBox.Show("Warning. There may be unsaved changes pending, are you sure you wish to exit and discard these changes?", "caption", MessageBoxButtons.YesNo)
            If result = DialogResult.No Then
                MessageBox.Show("Exiting has been cancelled by the user.")
            ElseIf result = DialogResult.Yes Then
                MessageBox.Show("Changes have been discarded and the program will now exit.")
                If cnn.State = ConnectionState.Open Then
                    cnn.Close()
                End If
                Me.Close()
                End If
                'MessageBox.Show("You have unsaved changes, are you sure you with to exit?" & newline)


        Catch ex As Exception
            MessageBox.Show("There was an error while trying to close the application." & newline & "Original error:" & newline & ex.ToString())
        End Try
        

    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        EnableInput()
        btnFirst.Enabled = False
        btnPrevious.Enabled = False
        btnNext.Enabled = False
        btnLast.Enabled = False
        radMale.Checked = True
        Try
            Dim row As DataRow = ds.Tables("Details").NewRow()
            row("FirstName") = "First Name"
            row("LastName") = "Last Name"
            row("Address1") = "Address1"
            row("Address2") = "Address2"
            row("County") = "County"
            row("Country") = "Country"
            row("Phone") = "Phone"
            row("Email") = "Email"
            row("DOB") = dtDoB.Value()
            row("Gender") = "True"
            row("Notes") = "Notes"

            ds.Tables("tblDetails").Rows.Add(row)
            intRecCount = dt.Rows.Count

            intIndex = intRecCount - 1

            DisplayRecord(intIndex)

        Catch ex As Exception
            MessageBox.Show("Unable to add new entry." & newline & "Original Error:" & newline & ex.ToString())
        End Try

    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        EnableInput()
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click


    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        DisableInput()

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        ds.RejectChanges()
        intRecCount = dt.Rows.Count

        intIndex = intRecCount - 1

        DisplayRecord(intIndex)

        'DisableInput()
        btnFirst.Enabled = True
        btnPrevious.Enabled = True
        btnNext.Enabled = True
        btnLast.Enabled = True

    End Sub
End Class