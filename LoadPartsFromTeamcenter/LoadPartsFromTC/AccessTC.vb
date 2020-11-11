﻿Imports System.IO
Imports System.Runtime.InteropServices
Imports Microsoft.VisualBasic.FileIO


Public Class AccessTc

    Public Shared Acronym As String = Environment.GetEnvironmentVariable("USERPROFILE").ToLower()


    Public Shared Function GetUserTcMode()
        Dim application As SolidEdgeFramework.Application
        Dim solidEdgeTce As SolidEdgeFramework.SolidEdgeTCE
        Dim bTeamCenterMode As Boolean

        'Get Active session of Solid Edge
        application = Marshal.GetActiveObject("SolidEdge.Application")

        ' Teamcenter Mode
        solidEdgeTce = application.SolidEdgeTCE
        Call solidEdgeTce.GetTeamCenterMode(bTeamCenterMode)
        Return bTeamCenterMode
    End Function

    Public Shared Sub LoadPartToCache(ByVal file As Convertor.CadPart, ByVal cachePath As String)

        Dim application As SolidEdgeFramework.Application
        Dim solidEdgeTce As SolidEdgeFramework.SolidEdgeTCE
        Dim userName As String
        Dim password As String
        Dim group As String
        Dim role As String
        Dim url As String
        Dim temp(0, 0) As Object

        'Get Active session of Solid Edge
        application = Marshal.GetActiveObject("SolidEdge.Application")

        ' Teamcenter Mode
        solidEdgeTce = application.SolidEdgeTCE

        ' Get default cache path
        solidEdgeTce.GetPDMCachePath(cachePath)

        'Specify Server Credentials
        userName = Acronym
        password = Acronym
        group = "Engineering"
        role = "Designer"
        url = "TC12_PROD"

        Call solidEdgeTce.ValidateLogin(userName, password, group, role, url)


        'Download the file to cache
        Dim fileInCache As String
        fileInCache = Path.Combine(cachePath, file.Filename)
        If FileSystem.FileExists(fileInCache) Then
            Console.WriteLine("File found in your cache.")
        Else
            solidEdgeTce.DownladDocumentsFromServerWithOptions(file.Jde, file.Revision, file.Filename,
                                                           SolidEdgeConstants.RevisionRuleType.LatestRevision, "", True,
                                                           False, SolidEdgeConstants.TCDownloadOptions.COImplicit, temp)
            Console.WriteLine("File loaded in your cache.")
        End If


    End Sub


End Class