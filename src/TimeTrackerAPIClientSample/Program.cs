using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DensoCreate.TimeTracker.ApiClient.Net.WebApi;
using DensoCreate.TimeTracker.ApiClient.Net.Services;
using DensoCreate.TimeTracker.ApiClient.Net.Services.Auth;
using DensoCreate.TimeTracker.ApiClient.Net.Services.Project;
using DensoCreate.TimeTracker.ApiClient.Net.WebApi.Contracts.ProjectStructures;

namespace TimeTrackerAPIClientSample
{
    class Program
    {
        // プロジェクト一覧を取得するサンプル
        public static async Task Main(string[] args)
        {
            // 事前準備
            // TimeTrackerApiClietのサービス作成ファクトリを用意
            var timeTrackerConnection = new TimeTrackerConnection();
            timeTrackerConnection.ServerUrl = "http://www.example.com/TimeTrackerNX/";
            var factory = new TimeTrackerServiceFactory()
            {
                Connection = timeTrackerConnection
            };
            // ユーザー/パスワードで認証し、取得したトークンをファクトリに設定
            var token = await factory.Create<ITokenService>().PublishToken("userName", "password");
            factory.Connection.Credentials.Token = token;

            // プロジェクトを検索するフィルターを設定
            var filter = new ProjectFilterState
            {
                Keyword = "開発", // キーワードに「開発」を指定
                IsFinished = false  // 稼働プロジェクトのみ
            };
            // プロジェクトを検索
            var projects = await factory.Create<IProjectService>().GetFilteredProjects(filter);

            // 検索フィルターの条件に一致するプロジェクトの件数を出力
            Console.WriteLine($"TotalCount={projects.TotalCount}");

            // 取得したプロジェクトの情報を出力（プロジェクトの計画開始日順に出力）
            foreach (var project in projects.Data.OrderBy(p => p.PlannedStartDate))
            {
                Console.WriteLine($"期間：{project.PlannedStartDate.Date.ToString("yyyy/MM/dd")}～{project.PlannedFinishDate.Date.ToString("yyyy/MM/dd")} [{project.Code}]{project.Name}");
            }
        }
    }
}
