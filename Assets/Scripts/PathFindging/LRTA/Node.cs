using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LRTA {
    public class Node
    {
        public int x, y;
        public float h;
        public float temp;
        public bool wall;

        public Node(int x, int y, float h, bool wall) {
            this.x = x;
            this.y = y;
            this.h = h;
            this.wall = wall;
        }
    }
}
