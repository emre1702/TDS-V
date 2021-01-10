using Octokit;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Models.Changelogs;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Server;
using TDS.Shared.Core;
using TDS.Shared.Default;

namespace TDS.Server.Handler
{
    public class ChangelogsHandler
    {
        private string _json = string.Empty;

        public ChangelogsHandler(AppConfigHandler appConfigHandler, ISettingsHandler settingsHandler, ServerStartHandler serverStartHandler, RemoteBrowserEventsHandler remoteBrowserEventsHandler)
        {
            remoteBrowserEventsHandler.Add(ToServerEvent.LoadChangelogs, (_) => _json);

            LoadChangelogs(appConfigHandler, settingsHandler, serverStartHandler);
        }

        private async void LoadChangelogs(AppConfigHandler appConfigHandler, ISettingsHandler settingsHandler, ServerStartHandler serverStartHandler)
        {
            if (string.IsNullOrWhiteSpace(appConfigHandler.GitHubToken))
                return;
            if (string.IsNullOrWhiteSpace(settingsHandler.ServerSettings.GitHubRepoOwnerName))
                return;
            if (string.IsNullOrWhiteSpace(settingsHandler.ServerSettings.GitHubRepoRepoName))
                return;

            var client = CreateGitHubClient(appConfigHandler);
            var commits = await GetCommits(client, settingsHandler).ConfigureAwait(false);
            var commitsToSync = Map(commits);
            _json = HttpUtility.JavaScriptStringEncode(Serializer.ToBrowser(commitsToSync));
            serverStartHandler.LoadedChangelogs = true;
        }

        private GitHubClient CreateGitHubClient(AppConfigHandler appConfigHandler)
            => new GitHubClient(new ProductHeaderValue("TDS-V_Commits_Getter"))
            {
                Credentials = new Credentials(appConfigHandler.GitHubToken)
            };

        private async Task<IEnumerable<GitHubCommit>> GetCommits(GitHubClient client, ISettingsHandler settingsHandler)
        {
            var commitRequest = new CommitRequest { Since = DateTimeOffset.Now.AddDays(-30) };

            var commits = await client.Repository
                .Commit
                .GetAll(settingsHandler.ServerSettings.GitHubRepoOwnerName, settingsHandler.ServerSettings.GitHubRepoRepoName, commitRequest)
                .ConfigureAwait(false);
            return commits.Where(ShouldShowCommit);
        }

        private IEnumerable<ChangelogsGroup> Map(IEnumerable<GitHubCommit> commits)
        {
            return commits.GroupBy(c => c.Commit.Author.Date.Date)
                .Select(g => new ChangelogsGroup
                {
                    Date = g.Key,
                    Changes = g
                        .OrderBy(c => c.Commit.Message, new ChangeLogsComparer(this))
                        .Select(c => GetCommitMessageToShow(c.Commit.Message))
                })
                .OrderByDescending(g => g.Date);
        }

        private bool ShouldShowCommit(GitHubCommit commit)
            => commit.Commit.Message.StartsWith("[") && !commit.Commit.Message.StartsWith("[code]", StringComparison.OrdinalIgnoreCase);

        private string GetPrefix(string commitMessage)
        {
            if (commitMessage[0] != '[')
                return string.Empty;
            var bracketsCloseIndex = commitMessage.IndexOf(']');
            if (bracketsCloseIndex < 0)
                return string.Empty;
            return commitMessage[0..(bracketsCloseIndex + 1)];
        }

        private string GetType(string commitMessage)
        {
            var prefix = GetPrefix(commitMessage);
            if (prefix.Length == 0)
                return string.Empty;

            var origType = prefix[1..^1];
            return origType.ToLower() switch
            {
                "neu" => "NEW",
                "änderung" => "CHANGE",
                "fix" => "BUG",

                _ => origType.ToUpper()
            };
        }

        private string GetWithoutPrefix(string commitMessage)
        {
            var prefix = GetPrefix(commitMessage);
            if (prefix.Length == 0)
                return commitMessage.Trim();
            return commitMessage[(prefix.Length + 1)..^0].Trim();
        }

        private string GetCommitMessageToShow(string commitMessage)
        {
            var type = GetType(commitMessage);
            var message = GetWithoutPrefix(commitMessage);
            return $"[{type}] {message.Replace("\"", "[ESCSTR]")}";
        }

        private class ChangeLogsComparer : IComparer<string>
        {
            private readonly ChangelogsHandler _changelogsHandler;

            public ChangeLogsComparer(ChangelogsHandler changelogsHandler)
                => _changelogsHandler = changelogsHandler;

            public int Compare([AllowNull] string x, [AllowNull] string y)
            {
                if (x is null)
                    return 1;
                if (y is null)
                    return -1;
                var xPrefix = _changelogsHandler.GetType(x).ToLower();
                var yPrefix = _changelogsHandler.GetType(y).ToLower();

                return xPrefix switch
                {
                    "new" => yPrefix == "new" ? x.ToLower().CompareTo(y.ToLower()) : -1,
                    "change" => yPrefix == "new" ? 1 : (yPrefix == "change" ? x.ToLower().CompareTo(y.ToLower()) : -1),
                    "bug" => yPrefix == "bug" ? x.ToLower().CompareTo(y.ToLower()) : 1,

                    _ => x.ToLower().CompareTo(y.ToLower())
                };
            }
        }
    }
}