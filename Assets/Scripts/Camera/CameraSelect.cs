using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraSelect : MonoBehaviour
{
    [SerializeField] protected Bando bandoJugador;
    [SerializeField] protected Camera _camera;

    [SerializeField] private Static[] formation1;

    public Bando BandoJugador { get => bandoJugador; }

    private Pattern pattern1;

    protected HashSet<AgentNPC> _select;
    // Start is called before the first frame update
    void Awake()
    {
        _select = new HashSet<AgentNPC>();
        pattern1 = new Pattern(formation1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        AgentNPC obj = GetObjectAtCursor();

        if (Input.GetMouseButtonDown(0)) LeftClick(obj);
        if (Input.GetMouseButtonDown(1)) RightClick(obj);
        if (Input.GetKeyDown(KeyCode.Escape)) UnselectAll();
        if (Input.GetKeyDown(KeyCode.Alpha1)) MakeFormation(pattern1);
    }


    AgentNPC GetObjectAtCursor() {
        Vector3 origin = _camera.transform.position;

        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.1f);
        Vector3 direction = (_camera.ScreenToWorldPoint(mousePosition) - origin).normalized;

        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(origin, direction, out hitInfo, Mathf.Infinity, 64);
        if (hit) {
            return hitInfo.collider.GetComponent<AgentNPC>();
        } else {
            return null;
        }
    }

    void SelectNPC(AgentNPC npc) {
        if (npc.Bando == BandoJugador && npc.TrySelect()) {
            _select.Add(npc);
        }
    }

    void UnselectNPC(AgentNPC npc) {
        if (npc && _select.Contains(npc)) {
            npc.Unselect();
            _select.Remove(npc);
        }
    }

    void UnselectAll() {
        foreach(AgentNPC npc in _select) {
            npc.Unselect();
        }
        _select = new HashSet<AgentNPC>();
    }

    void LeftClick(AgentNPC obj) {
        if (obj == null) return;
        bool selected = _select.Contains(obj);
        if (!Input.GetKey(KeyCode.LeftControl)) UnselectAll();
        if (!selected) SelectNPC(obj);
    }

    void SetTarget(Vector3 target) {
        if (SelectIsFormation()) {
            _select.First().Formation.MoveOffset(target);
        } else {
            foreach(AgentNPC npc in _select) {
                npc.MoveToTarget(target);
            }
        }
        
    }

    void RightClick(AgentNPC obj) {
        if (Input.GetKey(KeyCode.LeftControl)) {
            AgentNPC enemy = GetObjectAtCursor();
            if (enemy != null) {
                foreach(var s in _select) {
                    s.Atacar(enemy);
                }
            }
        } else {
            Vector3 origin = _camera.transform.position;

            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.1f);
            Vector3 direction = (_camera.ScreenToWorldPoint(mousePosition) - origin).normalized;

            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(origin, direction, out hitInfo, Mathf.Infinity, (1<<11));
            if (hit) {
                SetTarget(hitInfo.point);
            }
        }
    }

    void MakeFormation(Pattern p) {
        IEnumerable<AgentNPC> members = _select;
        AgentNPC leader = members.First();
        members = members.Skip(1);
        Formation f = new Formation(leader, p);

        foreach(var a in members) {
            f.AddCharacter(a);
        }
    }

    bool SelectIsFormation() {
        if (_select.Count() == 0) return false;
        Formation f = null;
        foreach(var agent in _select) {
            if (agent.Formation == null) return false;
            if (f == null) f = agent.Formation;
            if (f != agent.Formation) return false;
        }

        return _select.Count() == f.GetSize();
    }
}
