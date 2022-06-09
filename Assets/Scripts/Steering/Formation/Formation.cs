using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public  class Formation
{
    public enum Status {
        InFormation, OffsetMove, LeaderFollowing
    };
    public class Assigment {
        public AgentNPC character;
        public int number;
        public bool onGoal;
    }

    private Static moveOffsetGoal;
    private bool leaderOnGoal;

    private Status estado;
    private Pattern pattern;
    private AgentNPC leader;
    private IEnumerable<Assigment> assigments;

    public Status Estado {get => estado;}

    public Formation(AgentNPC leader, Pattern pattern) {
        this.leader = leader;
        this.pattern = pattern;
        leader.OnEnterFormation(this);

        estado = Status.InFormation;
        moveOffsetGoal.position = leader.Position;
        moveOffsetGoal.orientation = leader.Orientation;

        assigments = new List<Assigment>();

        Debug.Log("Nueva formación");
    }

    public int GetSize() {
        if (leader == null) return 0;
        return 1 + GetNumberAssigments();
    }

    public int GetNumberAssigments() {
        return assigments.Count();
    }

    public bool AddCharacter(AgentNPC c) {
        if (pattern.SupportSlots(GetNumberAssigments() + 1)) {
            Assigment a = new Assigment();
            a.character = c;
            assigments = assigments.Concat(new[] {a});
            UpdateSlots();
            c.OnEnterFormation(this);
            return true;
        }

        return false;
    }

    public void UpdateSlots() {
        for(int i = 0; i < assigments.Count(); i++) {
            assigments.ElementAt(i).number = i;
        }
    }

    public void RemoveCharacter(AgentNPC c) {
        if (c == leader) {
            DissolveFormation();
        } else if (assigments.Any(a => a.character == c)) {
            assigments = assigments.Where(a => a.character != c);
            UpdateSlots();
            c.OnExitFormation(this);
        }
    }

    public Static GetSlotPosition(AgentNPC a) {
        if (a == leader) {
            return moveOffsetGoal;
        }

        Assigment assig = assigments.Where(assig => assig.character == a).First();

        Static slotRelative = pattern.GetSLocation(assig.number);

        Static slotAbsolute;

        slotAbsolute.position = Quaternion.Euler(0, moveOffsetGoal.orientation, 0) * slotRelative.position + moveOffsetGoal.position;
        slotAbsolute.orientation = Bodi.WrapAngle(slotRelative.orientation + moveOffsetGoal.orientation);
        return slotAbsolute;
    }

    public void MoveOffset(Vector3 target) {
        if (leader.MoveLeaderToTarget(target)) {
            moveOffsetGoal.position = target;
            moveOffsetGoal.orientation = leader.Orientation;
            leaderOnGoal = false;
            foreach(var a in assigments) {
                a.onGoal = false;
                a.character.MoveToNewSlot(GetSlotPosition(a.character).position);
            }
            estado = Status.OffsetMove;
        }
    }

    public void ReachedGoal(Agent a) {
        if (IsLeader(a)) {
            leaderOnGoal = true;
        } else if (IsPart(a)) {
            Assigment assig = assigments.Where(assig => assig.character == a).First();
            assig.onGoal = true;
        }

        if (leaderOnGoal && assigments.All(assig => assig.onGoal)) {
            estado = Status.InFormation;
            Debug.Log("All in formation");
        }
    }

    public bool IsPart(Agent a) {
        return IsLeader(a) || (assigments.Where(assig => assig.character == a).Count() > 0);
    }

    public bool IsLeader(Agent a) {
        return a == leader;
    }

    public void DissolveFormation() {
        foreach(Assigment a in assigments) {
            a.character.OnExitFormation(this);
        }
        leader.OnExitFormation(this);

        assigments = null;
        leader = null;

        Debug.Log("Se rompió la formación");
    }
}
