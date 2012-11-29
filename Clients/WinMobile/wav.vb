Public Class Sound

    Private m_soundBytes() As Byte
    Private m_fileName As String

    Public Declare Function WCE_PlaySound Lib "CoreDll.dll" Alias "PlaySound" (ByVal szSound As String, ByVal hMod As IntPtr, ByVal flags As Integer) As Integer
    Public Declare Function WCE_PlaySoundBytes Lib "CoreDll.dll" Alias "PlaySound" (ByVal szSound() As Byte, ByVal hMod As IntPtr, ByVal flags As Integer) As Integer

    Private Enum Flags

        SND_SYNC = &H0 ' play synchronously (default)
        SND_ASYNC = &H1 ' play asynchronously
        SND_NODEFAULT = &H2 ' silence (!default) if sound not found
        SND_MEMORY = &H4 ' pszSound points to a memory file
        SND_LOOP = &H8 ' loop the sound until next sndPlaySound
        SND_NOSTOP = &H10 ' don't stop any currently playing sound
        SND_NOWAIT = &H2000 ' don't wait if the driver is busy
        SND_ALIAS = &H10000 ' name is a registry alias
        SND_ALIAS_ID = &H110000 ' alias is a predefined ID
        SND_FILENAME = &H20000 ' name is file name
        SND_RESOURCE = &H40004 ' name is resource name or atom

    End Enum

    ' Construct the Sound object to play sound data from the specified file.

    Public Sub New(ByVal fileName As String)

        m_fileName = fileName

    End Sub

    ' Construct the Sound object to play sound data from the specified stream.

    Public Sub New(ByVal stream As System.IO.Stream)

        ' read the data from the stream

        m_soundBytes = New Byte(stream.Length) {}

        stream.Read(m_soundBytes, 0, Fix(stream.Length))

    End Sub 'New

    ' Play the sound

    Public Sub Play()

        ' If a file name has been registered, call WCE_PlaySound,

        ' otherwise call WCE_PlaySoundBytes.

        If Not (m_fileName Is Nothing) Then

            WCE_PlaySound(m_fileName, IntPtr.Zero, Fix(Flags.SND_SYNC Or Flags.SND_FILENAME))

        Else

            WCE_PlaySoundBytes(m_soundBytes, IntPtr.Zero, Fix(Flags.SND_ASYNC Or Flags.SND_MEMORY))

        End If

    End Sub

End Class
