using System;

namespace Tetris
{
    public class Block_Queue
    {
        private readonly Block[] blocks = new Block[]
        {
            new I_Block(), 
            new O_Block(),
            new J_Block(),
            new L_Block(),
            new T_Block(),
            new Z_Block(),
            new S_Block()

        };
        private readonly Random random = new Random();
        public Block Next_Block { get; private set; }
        public Block_Queue()
        {
            Next_Block = RandomBlock();
        }
        private Block RandomBlock()
        {
            return blocks[random.Next(blocks.Length)];
        }
        public Block Generate_Block()
        {
            Block block = Next_Block;
            do
            {
                Next_Block = RandomBlock();
            }
            while (block.Id == Next_Block.Id);
            return block;
        }

    }

}
