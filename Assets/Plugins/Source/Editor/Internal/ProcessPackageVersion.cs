/*
* Copyright (c) 2021 PlayEveryWare
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/

using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.IO;
using System.Text.RegularExpressions;

namespace PlayEveryWare.EpicOnlineServices
{
    public class PreprocessPackageVersion : IPreprocessBuildWithReport
    {
        public const string packageInfoPath = "Assets/Plugins/Essential/EOSPackageInfo.cs";
        public const string versionRegexString = @"const string buildVersion = [^\s;]*;";

        public int callbackOrder { get { return 0; } }
        public void OnPreprocessBuild(BuildReport report)
        {
            string packageVersion = EOSPackageInfo.GetPackageVersion();
            var versionRegex = new Regex(versionRegexString);
            var packageInfoContents = File.ReadAllText(packageInfoPath);
            packageInfoContents = versionRegex.Replace(packageInfoContents, $"const string buildVersion = \"{packageVersion}\";");
            File.WriteAllText(packageInfoPath, packageInfoContents);
        }
    }

    public class PostprocessPackageVersion : IPostprocessBuildWithReport
    {
        public int callbackOrder { get { return 0; } }

        public void OnPostprocessBuild(BuildReport report)
        {
            var versionRegex = new Regex(PreprocessPackageVersion.versionRegexString);
            var packageInfoContents = File.ReadAllText(PreprocessPackageVersion.packageInfoPath);
            packageInfoContents = versionRegex.Replace(packageInfoContents, "const string buildVersion = UnknownVersion;");
            File.WriteAllText(PreprocessPackageVersion.packageInfoPath, packageInfoContents);
        }
    }
}