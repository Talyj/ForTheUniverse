using System.Collections.Generic;
using Photon.Realtime;

public class Group
{
    public string Name { get; set; }
    public Player Leader { get; set; }
    public List<Player> Members { get; set; }

    public Group(string name, Player leader)
    {
        this.Name = name;
        this.Leader = leader;
        this.Members = new List<Player>();
        this.Members.Add(leader);
    }

    public void AddMember(Player member)
    {
        this.Members.Add(member);
    }

    public void RemoveMember(Player member)
    {
        this.Members.Remove(member);
    }
}
