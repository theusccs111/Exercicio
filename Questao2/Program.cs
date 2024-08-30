using Newtonsoft.Json;
using Questao2;
using System.ComponentModel.DataAnnotations;
using System.Text;

public class Program
{
    public async static Task Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public async static Task<int> getTotalScoredGoals(string team, int year)
    {

        ApiResponse<FootballMatch[]> resultado = await GetDataFromFootball(team, year,1);

        int goals = 0;
        goals = CalculateGoals(team, resultado, goals);

        if (resultado.Total_pages > 1)
        {
            for (int i = 2; i <= resultado.Total_pages; i++)
            {
                resultado = await GetDataFromFootball(team, year,i);
                goals = CalculateGoals(team, resultado, goals);
            }
        }

        return goals;


    }

    private static int CalculateGoals(string team, ApiResponse<FootballMatch[]> resultado, int goals)
    {
        foreach (var item in resultado.Data)
        {
            if (item.Team1.Equals(team))
            {
                goals += Convert.ToInt32(item.Team1goals);
            }

            if (item.Team2.Equals(team))
            {
                goals += Convert.ToInt32(item.Team2goals);
            }
        }

        return goals;
    }

    private static async Task<ApiResponse<FootballMatch[]>> GetDataFromFootball(string team, int year,int page)
    {
        using (var client = new HttpClient())
        {
            team = team.Replace(" ", "%20");
            client.BaseAddress = new Uri("https://jsonmock.hackerrank.com/");
            HttpResponseMessage postTask = await client.GetAsync(string.Concat("api/football_matches?year=", year, "&team1=", team,"&page=",page));

            string ContentReturn = await postTask.Content.ReadAsStringAsync();
            ApiResponse<FootballMatch[]> resultado = JsonConvert.DeserializeObject<ApiResponse<FootballMatch[]>>(ContentReturn);
            return resultado;
        }
    }
}