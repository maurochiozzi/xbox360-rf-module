Public Class ControlesXbox
    Private device_ID As List(Of String)
    Private device_pos As List(Of Integer)
    Private posicoes_livres() As Integer = {1, 1, 1, 1}

    Sub New()
        device_ID = New List(Of String)
        device_pos = New List(Of Integer)
    End Sub

    Public Function addDevice(ID As String) As Boolean
        If device_ID.Contains(ID) Then
            Return False
        ElseIf estaCheio() Then
            Return False
        Else
            Dim posicao As Integer = proxPosicaoLivre()

            device_ID.Add(ID)
            device_pos.Add(posicao)
            posicoes_livres(posicao) = 0

            Return True
        End If
    End Function

    Public Function removeDevice(ID As String) As Boolean
        If device_ID.Contains(ID) Then
            Dim index As Integer
            Dim indice As Integer

            index = device_ID.IndexOf(ID)
            device_ID.RemoveAt(index)

            indice = device_pos.Item(index)
            device_pos.RemoveAt(index)

            posicoes_livres(indice) = 1

            Return True
        Else
            Return False
        End If
    End Function

    Public Function getLed(ID As String) As Integer
        If device_ID.Contains(ID) Then
            Dim index As Integer = device_ID.IndexOf(ID)

            Return (device_pos.Item(index))
        Else
            Return -1
        End If

    End Function

    Private Function proxPosicaoLivre() As Integer
        For indice As Integer = 0 To 3 Step 1
            If posicoes_livres(indice) = 1 Then
                Return indice
            End If
        Next indice

        Return -1
    End Function

    Private Function estaCheio() As Boolean
        Dim qnt_ocupado As Integer = 0

        For indice As Integer = 0 To 3 Step 1
            If posicoes_livres(indice) = 0 Then
                qnt_ocupado += 1
            End If
        Next

        If qnt_ocupado = 4 Then
            Return True
        Else
            Return False
        End If
    End Function

End Class
