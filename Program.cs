/*
 
Det er håndboldsæson i fjernsynet, og løsningen på aftenens opgave ville godt kunne bruges i en håndboldturnering.
I skal lave et program, der tilfældigt kan fordele 6 hold i 2 puljer med 3 hold i hver pulje.
Det er ikke nødvendigt, at brugeren skal indtaste navnene på de 6 hold hver gang - I må gerne bare skrive dem i koden (det kaldes hard coding af navnene)
Grupperne skal dannes tilfældigt, og programmet skal udskrive hvilke hold der er i de to puljer. Hvert hold kan naturligvis kun være i én pulje.
Hvert hold skal møde de to andre hold i pulje én gang. Det giver tre kampe per pulje. Programmet skal udskrive kampene for hver af de to puljer.
Hvis en pulje fx består af Danmark, Sverige og Norge giver det følgende kampe (rækkefølgen er ligegyldig)

Danmark - Sverige

Danmark - Norge

Sverige - Norge

Ekstra-udvidelse

Hvis I har tid til overs, så udvid programmet så det bliver mere generelt. Lad brugeren indtaste, hvor mange grupper han ønsker, og hvor mange hold der skal være i hver gruppe. 
Sørg for at udskriften af kampene stadig virker.
God fornøjelse

Som altid håber jeg, at én af jer vil dele jeres løsning i forummet efterfølgende 
 
*/

using System;
using System.Collections;
using System.Collections.Generic;

// Create teams
TeamsWithGroups teams = new TeamsWithGroups();
teams.AddTeam(new Team("Denmark"));
teams.AddTeam(new Team("Norway"));
teams.AddTeam(new Team("Sweden"));
teams.AddTeam(new Team("Russia"));
teams.AddTeam(new Team("Germany"));
teams.AddTeam(new Team("Iceland"));
teams.AddTeam(new Team("Poland"));
teams.AddTeam(new Team("China"));

// Assign groups
teams.AssignGroupsRandomly(2);

// Print groups
teams.PrintAllGroups();

// Print matches
teams.PrintMatches();


class TeamsWithGroups {
    // A class that will hold groups of teams
    private TeamCollection all_teams = new();
    private TeamCollection unassigned_teams = new();
    private List<TeamCollection> groups = new();
    private int number_of_groups = 0;
    private Random random = new();

    public void AddTeam(Team _team) {
        all_teams.AddTeam(_team);
        unassigned_teams.AddTeam(_team);
    }
    public void PrintTeams() {
        all_teams.PrintTeams();
    }

    public void PrintGroup(int _group) {
        int group_off_by_one = _group - 1;

        if (group_off_by_one > groups.Count || group_off_by_one < 0) {
            Console.WriteLine("PrintGroup(): Invalid number of groups");
            return;
        }

        Console.WriteLine($"Teams in group {_group}:");
        groups[group_off_by_one].PrintTeams();
    }

    public void PrintAllGroups() {
        if (number_of_groups <= 0) {
            Console.WriteLine("PrintAllGroups(): There are no groups!");
            return;
        }

        else {
            for (int i = 1; i <= number_of_groups; i++) {
                PrintGroup(i);
                Console.WriteLine();
            }
        }
    }

    public void AssignGroupsRandomly(int _number_of_groups) {

        if (_number_of_groups <= 0) {
            Console.WriteLine("AssignGroupsRandomly(): Invalid number of groups");
            return;
        }

        if (all_teams.Length <= 0) {
            Console.WriteLine("AssignGroupsRandomly(): No teams!");
            return;
        }


        number_of_groups = _number_of_groups;

        // How many teams pr group?
        int no_of_teams_pr_group = all_teams.Length / number_of_groups;

        // Clear groups to start over
        ClearGroups();

        // Do this for each team:
        // 1. Assign to group
        // 2. Remove from unassigned list        
        for (int group = 1; group < number_of_groups; group++) { // Less than because the last group will not need to use the random function                                                                 // 
            TeamCollection group_teams = new();
            for (int teams_assigned = 0; teams_assigned < no_of_teams_pr_group; teams_assigned++) {
                int random_team = random.Next(1, unassigned_teams.Length + 1);
                Team actual_team = unassigned_teams.GetTeam(random_team);
                actual_team.Group = group;
                group_teams.AddTeam(actual_team);
                unassigned_teams.RemoveTeam(actual_team); // remove team from list of unassigned teams
            }
            // Add group
            groups.Add(group_teams);
        }

        // Assign the remaining teams to the last group and remove from unassigned
        TeamCollection group_teams_last = new();
        foreach (Team team in unassigned_teams) {
            team.Group = number_of_groups;
            group_teams_last.AddTeam(team);
        }
        groups.Add(group_teams_last);
        unassigned_teams.Clear();
    }

    public void ClearGroups() {
        all_teams.ClearGroups();
        groups.Clear();
    }

    public void PrintMatches() {
        int off_by_one;
        for (int i = 0; i < groups.Count; i++) {
            off_by_one = i + 1;
            Console.WriteLine($"Matches for group {off_by_one} :");
            groups[i].PrintMatches();
            Console.WriteLine();
        }        
    }
}

class Team {
    private string name;
    private int group;

    public string Name {
        get { return name; }
        set { name = value; }
    }

    public int Group {
        get { return group; }
        set { group = value; }
    }


    public Team(string _name) {
        name = _name;
        group = 0;
    }

    public Team(string _name, int _group) {
        name = _name;
        group = _group;
    }

    public bool HasGroup() {
        return group > 0;
    }


    public void PrintTeamAndGroup() {
        Console.WriteLine(name + ", Group " + group);
    }

    public void PrintTeam() {
        Console.WriteLine(name);
    }

}

class TeamCollection : IEnumerator, IEnumerable {
    private List<Team> teams = new();
    private int position = -1;

    public int Length { get { return teams.Count; } }

    public void AddTeam(Team _team) {
        teams.Add(_team);
    }

    public void RemoveTeam(string _team) {
        teams.RemoveAll(x => x.Name == _team);
    }

    public void RemoveTeam(Team _team) {
        teams.Remove(_team);
    }

    public void PrintTeamsWithGroup() {
        foreach (Team team in teams) {
            team.PrintTeamAndGroup();
        }
    }
    public void PrintTeams() {
        foreach (Team team in teams) {
            team.PrintTeam();
        }
    }

    public void PrintMatches() {
        // 1. Print first team vs the rest
        // 2. Print second team vs the rest
        // 3...

        // Team ....
        for (int team = 0; team < teams.Count; team++) {
            // vs the rest
            for (int the_rest = team + 1; the_rest < teams.Count; the_rest++) {
                Console.WriteLine(teams[team].Name + " - " + teams[the_rest].Name);
            }
        }
    }

    public void ClearGroups() {
        foreach (Team team in teams) {
            team.Group = 0;
        }
    }

    public void Clear() {
        teams.Clear();
        position = -1;
    }

    public Team GetTeam(int _teamno) {
        // _teamno starts with 1
        if (_teamno > teams.Count + 1) {
            Console.WriteLine("GetTeam(): Team number is too high");
            return null;
        }

        if (_teamno < 1) {
            Console.WriteLine("GetTeam(): Team number is too low");
            return null;
        }

        return teams[_teamno - 1];
    }

    // Foreach loop requirements:
    public IEnumerator GetEnumerator() { return (IEnumerator)this; }
    public bool MoveNext() { position++; return position < teams.Count;  }
    public void Reset() { position = -1;}
    public object Current { get { return teams[position]; } }
}
