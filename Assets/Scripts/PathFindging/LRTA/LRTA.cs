using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace LRTA {
    public class LRTA
    {
        private Map map;
        private List<Node> grid;
        private int rows, cols;

        private Vector2Int current;
        private Vector2Int goal;
        private Vector3 goalV3;

        private bool manhattan;

        public LRTA(Map m, Vector3 start, Vector3 goal, bool manhattan) {
            rows = m.Rows;
            cols = m.Columns;
            map = m;
            grid = new List<Node>(rows * cols);

            this.goal = map.World2Map(goal);
            goalV3 = goal;
            current = map.World2Map(start);

            this.manhattan = manhattan;

            for(int y = 0; y < rows; y++) {
                for(int x = 0; x < cols; x++) {
                    bool wall = !m.CanPass(new Vector2Int(x, y));
                    Node n = new Node(x, y, 0, wall);
                    n.h = Heuristic(n);
                    grid.Add(n);
                }
            }
        }

        public bool GetNextMove(out Vector3 position, out bool end) {
            Node n = grid[current.y * cols + current.x];
            List<Node> local = GetLocalSpace(n, 10);

            if (local.Count == 0) {
                position = Vector3.zero;
                end = false;
                return false;
            }

            bool success = UpdateLocal(local);
            if (!success) {
                position = Vector3.zero;
                end = false;
                return false;
            } else {
                Node next = null;
                foreach(var neighbor in GetNeighbors(n)) {
                    float min = 1 + neighbor.h;
                    if (next == null || 1 + next.h > min) next = neighbor;
                }

                if (IsGoal(next)) {
                    position = goalV3;
                    end = true;
                } else {
                    position = map.Map2World(new Vector2Int(next.x, next.y));
                    end = false;
                }

                current = new Vector2Int(next.x, next.y);
                return true;
            }
        }

        private float minH(List<Node> nodes) {
            float min = Mathf.Infinity;
            foreach(var n in nodes) {
                if (n.h < min) min = n.h;
            }

            return min;
        }

        private bool UpdateLocal(List<Node> local) {
            foreach(Node n in local) {
                n.temp = n.h;
                n.h = Mathf.Infinity;
            }
            int i = 0;
            List<Node> inf = new List<Node>(local);
            while(inf.Count > 0) {
                i++;
                Node v = null;
                float newH = 0;
                foreach(Node n in inf) {
                    if (v == null) {
                        v = n;
                        newH = Mathf.Max(n.temp, 1 + minH(GetNeighbors(n)));
                    } else {
                        float h = Mathf.Max(n.temp, 1 + minH(GetNeighbors(n)));
                        if (h < newH) {
                            newH = h;
                            v = n;
                        }
                    }
                }

                v.h = newH;

                if (v.h == Mathf.Infinity) return false;
                inf.Remove(v);
            }

            return true;
        }

        private List<Node> GetLocalSpace(Node start, int depth) {
            List<Node> local = new List<Node>();
            List<Node> open = GetNeighbors(start);
            for(int i = 0; open.Count > 0 && i < depth; i++) {
                List<Node> copyOpen = new List<Node>(open);
                open.Clear();
                foreach(Node oN in copyOpen) {
                    if (IsGoal(oN)) continue;
                    local.Add(oN);

                    foreach(var neighbor in GetNeighbors(oN)) {
                        if (!local.Contains(neighbor) && !open.Contains(neighbor)) {
                            open.Add(neighbor);
                        }
                    }
                }
            }
            return local;
        }

        private float Heuristic(Node n) {
            if (manhattan) {
                return Math.Abs(n.x - goal.x) + Math.Abs(n.y - goal.y);
            } else {
                return Math.Max(Math.Abs(n.x - goal.x), Math.Abs(n.y - goal.y));
            }
        }

        private bool IsGoal(Node n) {
            return n.x == goal.x && n.y == goal.y;
        }

        private List<Node> GetNeighbors(Node n) {
            List<Node> neighbors = new List<Node>();

            for(int y = -1; y <= 1; y++) {
                for(int x = -1; x <= 1; x++) {
                    if (manhattan && (x == y || x == -y)) continue;
                    if (!manhattan && (x == 0 || y == 0)) continue;
                    int realX = Math.Max(0, Math.Min(cols - 1, x + n.x));
                    int realY = Math.Max(0, Math.Min(rows - 1, y + n.y));
                    Node neighbor = grid[realY * cols + realX];
                    if (!neighbor.wall){
                        neighbors.Add(neighbor);
                    }
                }
            }

            return neighbors;
        }
    }
}
