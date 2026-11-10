using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class BuildScript
{
    public static void BuildMac()
    {
        Build(BuildTarget.StandaloneOSX, "Builds/Mac/Game.app");
    }

    public static void BuildAndroid()
    {
        EditorUserBuildSettings.buildAppBundle = false;
        Build(BuildTarget.Android, "Builds/Android/Game.apk");
    }

    static void Build(BuildTarget target, string defaultPath)
    {
        string outputPath = GetArg("-outputPath") ?? defaultPath;

        string buildNumber = Environment.GetEnvironmentVariable("BUILD_NUMBER");
        if (!string.IsNullOrEmpty(buildNumber))
        {
            PlayerSettings.bundleVersion = "1.0." + buildNumber;
        }

        string[] scenes = EditorBuildSettings.scenes
            .Where(s => s.enabled)
            .Select(s => s.path)
            .ToArray();

        if (scenes.Length == 0)
        {
            Debug.LogError("[BuildScript] Build Settings has no enabled scene.");
            EditorApplication.Exit(1);
            return;
        }

        BuildPlayerOptions options = new BuildPlayerOptions();
        options.scenes = scenes;
        options.locationPathName = outputPath;
        options.target = target;
        options.options = BuildOptions.None;

        BuildReport report = BuildPipeline.BuildPlayer(options);
        BuildSummary summary = report.summary;

        Debug.Log("[BuildScript] target=" + target
            + " result=" + summary.result
            + " output=" + outputPath
            + " size=" + summary.totalSize
            + " time=" + summary.totalTime);

        if (summary.result != BuildResult.Succeeded)
        {
            EditorApplication.Exit(1);
        }
    }

    static string GetArg(string name)
    {
        string[] args = Environment.GetCommandLineArgs();
        int index = Array.IndexOf(args, name);
        if (index >= 0 && index + 1 < args.Length)
        {
            return args[index + 1];
        }
        return null;
    }
}