using Microsoft.AspNet.Http;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GHUtils.Services
{
    public class GitHubService
    {
        private GitHubClient _githubClient;

        public GitHubService()
        {
            _githubClient = new GitHubClient(new ProductHeaderValue("GHUtils"));
            
        }

        public async Task CreateLabelAsync(string repositories, string label, string color, ClaimsPrincipal user)
        {
            _githubClient.Credentials = new Credentials(user.Claims.First(x => x.Type == "access_token").Value);

            var userRepos = await _githubClient.Repository.GetAllForCurrent();

            var repositoryList = repositories.Split(',').Select(x=>new Wildcard(x));

            foreach(var repo in userRepos)
            {
                foreach (var repoPattern in repositoryList)
                {
                    if (repoPattern.IsMatch(repo.FullName))
                    {
                        var result = await _githubClient.Issue.Labels.Create(repo.Owner.Login, repo.Name, new NewLabel(label, color));
                    }
                }
            }
        }
    }
}
