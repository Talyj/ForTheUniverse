using System.Collections.Generic;
using Photon.Realtime;

public class Group
{
    public string Name { get; set; }
    public Player Leader { get; set; }
    public List<Player> Members { get; set; }

    public int version = 0;

    public Group(string name, Player leader)
    {
        this.Name = name;
        this.Leader = leader;
        this.Members = new List<Player>();
        this.Members.Add(leader);
    }
    
    public Group(string name)
    {
        this.Name = name;
        this.Leader = null;
        this.Members = new List<Player>();
    }

    public void AddMember(Player member)
    {
        this.Members.Add(member);
    }

    public void RemoveMember(Player member)
    {
        this.Members.Remove(member);
    }

    public int GetSlotNumber(Player member)
    {
        return this.Members.IndexOf(member);
    }
}
