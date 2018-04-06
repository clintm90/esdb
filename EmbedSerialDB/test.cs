using System;
using System.Collections.Generic;
using System.Text;
using EmbedSerialDB;

namespace EmbedSerialDB
{
    class Test
    {
        public void lol()
        {
            using (var test = new ESDB())
            {
                var x = test.LoadDatabase("voitures");
                x.Search(new { x = "salut" });
            }
        }
    }
}
