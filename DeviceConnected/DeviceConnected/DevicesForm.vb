Imports System
Imports System.IO.Ports
Imports System.Threading
Imports System.Runtime.InteropServices
Imports System.Management

Public Class DevicesForm

    'Private connected_devices As List(Of String) = New List(Of String)
    Private devices As ControlesXbox = New ControlesXbox
    Private indice As Integer = 0
    Private WithEvents m_MediaConnectWatcher As ManagementEventWatcher
    Private port As String

#Region "Detect"
    Public Sub StartDetection()
        ' __InstanceOperationEvent will trap both Creation and Deletion of class instances
        Dim query2 As String = _
            "SELECT * FROM __InstanceOperationEvent WITHIN 10 WHERE TargetInstance ISA ""Win32_PnPEntity"""
        '"Select * from Win32_PnPEntity "
        m_MediaConnectWatcher = New ManagementEventWatcher(query2)
        m_MediaConnectWatcher.Start()
    End Sub

    Private Sub Arrived(ByVal sender As Object, ByVal e As System.Management.EventArrivedEventArgs) Handles m_MediaConnectWatcher.EventArrived
        Try
            Dim mbo, obj As ManagementBaseObject

            'is  it a creation or deletion event
            mbo = CType(e.NewEvent, ManagementBaseObject)
            ' is it either created or deleted
            obj = CType(mbo("TargetInstance"), ManagementBaseObject)

            Select Case mbo.ClassPath.ClassName
                Case "__InstanceCreationEvent"

                    If obj("Description").ToLower.Contains(" via play & charge kit") Then
                        Dim device_ID As String = obj("DeviceID")
                        'MsgBox("Conectado " & device_ID)

                        devices.addDevice(device_ID)
                        serialPort.Write(devices.getLed(device_ID))

                    End If
                Case "__InstanceDeletionEvent"
                    If obj("Description").ToLower.Contains(" via play & charge kit") Then
                        Dim device_ID As String = obj("DeviceID")
                        'MsgBox("Removido " & obj("DeviceID"))

                        serialPort.Write(devices.getLed(device_ID) + 4)
                        devices.removeDevice(device_ID)

                    End If
                    'Case Else
                    ' MsgBox(mbo.ClassPath.ClassName)
            End Select
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try

    End Sub
#End Region

    Private Sub DevicesForm(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        serialPort.Close()
        port = "COM3"
        serialPort.PortName = port 'change com port to match your Arduino port
        serialPort.BaudRate = 11500
        serialPort.DataBits = 8
        serialPort.Parity = Parity.None
        serialPort.StopBits = StopBits.One
        serialPort.Handshake = Handshake.None
        serialPort.Encoding = System.Text.Encoding.Default 'very important!
        ' serialPort.Open()

        StartDetection()
    End Sub

    'Private Function GetDriveLetterFromDisk(ByVal Name As String) As String
    '    Dim oq_part, oq_disk As ObjectQuery
    '    Dim mos_part, mos_disk As ManagementObjectSearcher
    '    Dim obj_part, obj_disk As ManagementObject
    '    Dim ans As String

    '    ' WMI queries use the "\" as an escape charcter
    '    Name = Replace(Name, "\", "\\")

    '    ' First we map the Win32_DiskDrive instance with the association called
    '    ' Win32_DiskDriveToDiskPartition.  Then we map the Win23_DiskPartion
    '    ' instance with the assocation called Win32_LogicalDiskToPartition

    '    oq_part = New ObjectQuery("ASSOCIATORS OF {Win32_DiskDrive.DeviceID=""" & Name & """} WHERE AssocClass = Win32_DiskDriveToDiskPartition")
    '    mos_part = New ManagementObjectSearcher(oq_part)
    '    For Each obj_part In mos_part.Get()

    '        oq_disk = New ObjectQuery("ASSOCIATORS OF {Win32_DiskPartition.DeviceID=""" & obj_part("DeviceID") & """} WHERE AssocClass = Win32_LogicalDiskToPartition")
    '        mos_disk = New ManagementObjectSearcher(oq_disk)
    '        For Each obj_disk In mos_disk.Get()
    '            ans &= obj_disk("Name") & ","
    '        Next
    '    Next

    '    Return ans.Trim(","c)
    'End Function

    'Public Shared Function getDevices()

    '    Dim devicesIDs As ArrayList

    '    Dim objWMIService, objItem, colItems
    '    Dim strComputer

    '    'On Error Resume Next
    '    strComputer = "."

    '    ' WMI Connection to the object in the CIM namespace
    '    objWMIService = GetObject("winmgmts:\\" & strComputer & "\root\cimv2")

    '    ' WMI Query to the Win32_OperatingSystem
    '    colItems = objWMIService.ExecQuery("Select * from Win32_PnPEntity ")

    '    ' For Each... In Loop (Next at the very end)
    '    For Each objItem In colItems
    '        If objItem.Name.ToString.ToLower.Contains("controller gioco compatibile hid") Then
    '            MsgBox("Device ID: " & objItem.DeviceID & _
    '            " Description: " & objItem.Description & _
    '            " Name: " & objItem.Name)
    '            'devicesIDs.Add(objItem.DeviceID)
    '        End If
    '    Next

    '    'Return devicesIDs
    'End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        serialPort.Write("1")
    End Sub

End Class
