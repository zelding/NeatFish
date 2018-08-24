using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NeatFish.Simulation.NEAT;

namespace NeatFish.Simulation.Utilities
{

    public class ConnectionHelper
    {

        public static Connection FindBetween(Node i, Node o, List<Connection> l)
        {
            foreach( Connection c in l ) {
                if ( c.Input.Id == i.Id && c.Output.Id == o.Id) {
                    return c;
                }
            }

            return null;
        }
    }
}