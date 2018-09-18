﻿Imports System.Text


Public Class GIF
    Shared Sub GIFAnimation(inputFile As String, proj As Project)
        If Not File.Exists(inputFile) Then Exit Sub
        If Not Package.ffmpeg.VerifyOK(True) Then Exit Sub

        If proj Is Nothing Then
            proj = New Project
            proj.Init()
            proj.SourceFile = inputFile
        End If

        Dim Rate = s.Storage.GetInt("GifFrameRate", 15)
        Dim cachePath = Folder.Temp + "%03d.png"
        Dim OutPutPath = inputFile + ".gif"
        Dim Seek = s.Storage.GetInt("GifTime", 25)
        Dim Duration = s.Storage.GetInt("GifLength", 4)
        Dim Size = s.Storage.GetInt("GifScale", 480)
        Dim Mode = s.Storage.GetString("PaletteGen", "diff")
        Dim SecondMode = s.Storage.GetString("PaletteUse", "rectangle")
        Dim Dither = s.Storage.GetString("GifDither", "dither=floyd_steinberg")

        g.DefaultCommands.ExecuteCommandLine(Package.Items("ffmpeg").Path.Escape + " -ss " & Seek & ".0 -t " & Duration & ".0 -i " + inputFile + " -vf " + """" + "fps=" & Rate & ",scale=" & Size & ":-1:flags=spline,palettegen=stats_mode=" & Mode & """ -loglevel quiet -y " + cachePath, True, True, False)
        g.DefaultCommands.ExecuteCommandLine(Package.Items("ffmpeg").Path.Escape + " -ss " & Seek & ".0 -t " & Duration & ".0 -i " + inputFile + " -i " + cachePath + " -lavfi " + """" + "fps=" & Rate & ",scale=" & Size & ":-1:flags=spline [x]; [x][1:v] paletteuse=" & Dither & ":diff_mode=" & SecondMode & """ -loglevel quiet -y " + OutPutPath, True, True, False)

        FileHelp.Delete(cachePath)

    End Sub

End Class


Public Class MTN
    Shared Sub Thumbnails(inputFile As String, proj As Project)
        If Not File.Exists(inputFile) Then Exit Sub
        If Not Package.MTN.VerifyOK(True) Then Exit Sub

        If proj Is Nothing Then
            proj = New Project
            proj.Init()
            proj.SourceFile = inputFile
        End If

        Dim Col = s.Storage.GetInt("MTNColumn", 4)
        Dim Rows = s.Storage.GetInt("MTNRow", 6)
        Dim SizeWidth = s.Storage.GetInt("MTNWidth", 1280)
        Dim SizeHeight = s.Storage.GetInt("MTNHeight", 150)
        Dim PictureQuality = s.Storage.GetInt("MTNQuality", 95)
        Dim PictureDepth = s.Storage.GetInt("MTNDepth", 12)

        g.DefaultCommands.ExecuteCommandLine(Package.Items("MTN").Path.Escape + """" + inputFile + """" + " -c " & Col & " -r " & Rows & " -w " & SizeWidth & " -h " & SizeHeight & " -D " & PictureDepth & " -j " & PictureQuality & " -Z ", True, True, False)

    End Sub
End Class
Public Class PNG

    Shared Sub aPNGAnimation(inputFile As String, proj As Project)
        If Not File.Exists(inputFile) Then Exit Sub
        If Not Package.ffmpeg.VerifyOK(True) Then Exit Sub

        If proj Is Nothing Then
            proj = New Project
            proj.Init()
            proj.SourceFile = inputFile
        End If

        Dim Rate = s.Storage.GetInt("PNGFrameRate", 15)
        Dim Path = inputFile + ".apng"
        Dim NewPath = inputFile + ".png"
        Dim Seek = s.Storage.GetInt("PNGTime", 25)
        Dim Duration = s.Storage.GetInt("PNGLength", 4)
        Dim Size = s.Storage.GetInt("PNGScale", 480)

        g.DefaultCommands.ExecuteCommandLine(Package.Items("ffmpeg").Path.Escape + " -ss " & Seek & ".0 -t " & Duration & ".0 -i " + inputFile + " -lavfi " + """" + "fps=" & Rate & ",scale=" & Size & ":-1:flags=spline" + """ -plays 0 -loglevel quiet -y " + Path, True, True, False)

        File.Move(Path, NewPath)


    End Sub

End Class

Public Class MKVInfoLookup
    Shared Sub MetadataInfo(inputFile As String, proj As Project)
        If Not File.Exists(inputFile) Then Exit Sub
        If Not Package.mkvinfo.VerifyOK(True) Then Exit Sub

        If proj Is Nothing Then
            proj = New Project
            proj.Init()
            proj.SourceFile = inputFile
        End If

        g.DefaultCommands.ExecuteCommandLine(Package.Items("mkvinfo").Path.Escape + " " + """" + inputFile + """" + BR + "pause", False, False, True)

    End Sub
End Class

Public Class MediaInfoLookup
    Shared Sub MetadataInfo(inputFile As String, proj As Project)
        If Not File.Exists(inputFile) Then Exit Sub
        If Not Package.mkvinfo.VerifyOK(True) Then Exit Sub

        If proj Is Nothing Then
            proj = New Project
            proj.Init()
            proj.SourceFile = inputFile
        End If

        g.DefaultCommands.ExecuteCommandLine(Package.Items("mkvinfo").Path.Escape + " " + """" + inputFile + """" + BR + "pause", False, False, True)
    End Sub
End Class

Public Class MKVMetaDataHDR
    Shared Sub MetadataHDR(inputFile As String, proj As Project)
        If Not File.Exists(inputFile) Then Exit Sub
        If Not Package.mkvmerge.VerifyOK(True) Then Exit Sub

        g.DefaultCommands.ExecuteCommandLine(Package.Items("mkvmerge").Path.Escape + " " + "-o ""%target_dir%%target_name%_HDR.%muxer_ext%""" + " --colour-matrix 0:9 --colour-range 0:1 --colour-transfer-characteristics 0:16 --colour-primaries 0:9 --max-content-light 0:1000 --max-frame-light 0:300 --max-luminance 0:1000 --min-luminance 0:0.01 --chromaticity-coordinates 0:0.68,0.32,0.265,0.690,0.15,0.06 --white-colour-coordinates 0:0.3127,0.3290 " + """" + inputFile + """", True, True, False)

    End Sub
End Class

Public Class UpdaterCaller
    Shared Sub CheckforUpdate()
        If Not Package.Update.VerifyOK(True) Then Exit Sub

        Dim UpdateCode = ""
        Dim Path = Folder.Startup + "Update" + ".bat"

        UpdateCode += "@echo OFF" + BR
        UpdateCode += "pushd %~dp0" + BR
        UpdateCode += "if exist ""%~dp0\Apps\Update\update.ps1"" (" + BR
        UpdateCode += "set update_script=""Apps\Update\update.ps1""" + BR
        UpdateCode += ") else (" + BR
        UpdateCode += "set update_script=""Apps\update.ps1""" + BR
        UpdateCode += ")" + BR
        UpdateCode += "powershell -noprofile -nologo -noexit -executionpolicy bypass -File %update_script%" + BR
        UpdateCode += "@exit"

        File.WriteAllText(Path, UpdateCode, Encoding.Default)
        g.DefaultCommands.ExecuteCommandLine(Path, False, False, False)

    End Sub
End Class


