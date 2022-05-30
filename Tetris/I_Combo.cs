using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class I_Combo : Block
    {
        private readonly Position[][] tiles = new Position[][]
        {
            new Position[]{new(3,0),new(3,1),new(3,2),new(3,3),new(3,4),new(3,5),new(3,6),new(3,7)},
            new Position[]{new(0,4),new(1,4),new(2,4),new(3,4),new(4,4),new(5,4),new(6,4),new(7,4)},
            new Position[]{new(4,0),new(4,1),new(4,2),new(4,3),new(4,4),new(4,5),new(4,6),new(4,7)},
            new Position[]{new(0,3),new(1,3),new(2,3),new(3,3),new(4,3),new(5,3),new(6,3),new(7,3)}
        };
         protected override Position StartOffset => new Position(0, 0);
        protected override Position[][] Tiles => tiles;
        public override int Id => 8;

    }
}