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

    Public Shared Sub LoadPartToCache(
                                      ByVal jde As String,
                                      ByVal revision As String,
                                      ByVal filename As String,
                                      ByVal cachePath As String
                                      )

        Dim application As SolidEdgeFramework.Application
        Dim solidEdgeTce As SolidEdgeFramework.SolidEdgeTCE
        Dim temp(0, 0) As Object

        'Get Active session of Solid Edge
        application = Marshal.GetActiveObject("SolidEdge.Application")

        ' Teamcenter Mode
        solidEdgeTce = application.SolidEdgeTCE

        ' Get default cache path
        solidEdgeTce.GetPDMCachePath(cachePath)


        'Download the file to cache
        Dim fileInCache As String
        fileInCache = Path.Combine(cachePath, filename)
        If FileSystem.FileExists(fileInCache) Then
            Console.WriteLine("File found in your cache.")

            If GetUserTcMode() Then
                solidEdgeTce.DownladDocumentsFromServerWithOptions(jde, revision, filename,
                                                        SolidEdgeConstants.RevisionRuleType.LatestRevision, "", True,
                                                        False, SolidEdgeConstants.TCDownloadOptions.COImplicit, temp)
                Console.WriteLine("File loaded in your cache.")
            Else
                Console.WriteLine("Your are not logged to Teamcenter. Please Login before running the macro.")
            End If
        End If


    End Sub


End Class