using System;
namespace Tetris
{
    //Den här klassen kommer kombinera alla funktioner i de andra .cs klasserna för att hantera hur spelet ändras medans man spelar det.//
    public class Game_State
    {
        private Block currentBlock;
        public Block CurrentBlock
        {
            get => currentBlock;
            private set
            {
                currentBlock = value;
                currentBlock.Reset();
                for (int i = 0; i < 2; i++)
                {
                    currentBlock.Move_Block(1, 0);
                    if (!BlockFits())
                    {
                        currentBlock.Move_Block(-1, 0);
                    }
                }
            }
        }
        public Grid Grid { get; }
        public Block_Queue Block_Queue { get; }
        public MainWindow mainWindow { get; }
        public bool YouLose { get; private set; }
        public int Score { get; private set; }
        public Block? Held_Block { get; private set; }
        public bool CanHold { get; private set; }
        public bool CanHold2 { get; private set; }
        public Block? Held_Block2 { get; private set; }
       
        //Här skapas variabler och booler som kommer hantera poängsystemet, förlust, sparade block och om man kan spara ett block eller inte.//  



        public Game_State()
        {
            Grid = new Grid(22, 10);
            Block_Queue = new Block_Queue();
            CurrentBlock = Block_Queue.Generate_Block();
            CanHold = true;
            CanHold2 = true;
       
        }

        //Att veta om ett block får plats är viktigt för programet när spelaren ska placera blocken. Boolen kommer gå i genom alla rutor under blocket för att se om de 
        // är tomma eller inte. Är minst en full kommer boolen retuneras som falsk annars retuneras den som san.//
        private bool BlockFits()
        {
            foreach (Position p in CurrentBlock.TilePosistion())
            {
                if (!Grid.CellEmpty(p.Row, p.Column))
                {
                    return false;
                }
            }
            return true;
        }
        //de två kommande funktionerna hanterar sparandet av block.
        //Båda dessa fungerar på samma sätt men refererar till två olikan objekt//
        public void Hold_Block()
        {
            //Om boolen CanHold är falsk eller om ditt nuvarande block är ett combo block dvs 
            // blockets Id är över 7 händer inget.
            if (!CanHold || CurrentBlock.Id > 7)
            {
                return;
            }
            //Om du inte har ett ssparat block kommer det sparade blocket sättas som ditt nuvarande och ditt 
            // nuvarande kommer sättas som ett nytt block.
            if (Held_Block == null)
            {
                Held_Block = CurrentBlock;
                CurrentBlock = Block_Queue.Generate_Block();
            }
            //Men om du redan har ett sparat block kommer de byta plats.
            
            else
            {
                (Held_Block, CurrentBlock) = (CurrentBlock, Held_Block);
            }

            CanHold = false;

        }
        public void Hold_Block2()
        {
            if (!CanHold2 || CurrentBlock.Id > 7)
            {
                return;
            }

            if (Held_Block2 == null)
            {
                Held_Block2 = CurrentBlock;
                CurrentBlock = Block_Queue.Generate_Block();
            }
            else
            {
                (Held_Block2, CurrentBlock) = (CurrentBlock, Held_Block2);
            }

            CanHold2 = false;

        }
        public void Block_Rotate_CW()
        {
            CurrentBlock.Rotate_CW();
            if (!BlockFits())
            {
                CurrentBlock.Rotate_CounterCW();
            }
        }
        public void Block_Rotate_CounterCW()
        {
            CurrentBlock.Rotate_CounterCW();
            if (!BlockFits())
            {
                CurrentBlock.Rotate_CW();
            }
        }
        public void Block_Move_Left()
        {
            CurrentBlock.Move_Block(0, -1);
            if (!BlockFits())
            {
                CurrentBlock.Move_Block(0, 1);
            }
        }
        public void Block_Move_Right()
        {
            CurrentBlock.Move_Block(0, 1);
            if (!BlockFits())
            {
                CurrentBlock.Move_Block(0, -1);
            }
        }
        //Dessa funktioner kommer att kopplas till vissa knappar på ditt tangentbord i en annan fil och kommer att styra ditt block.//
        //När dessa kallas kommer de utföras så länge blocket inte får plats. Om detta är fallet kommer den motsats rörelsen ske. 
        //Tex om du vill flytta ditt block åt höger men det får 
        //inte plats kommer det flyttas åt vänster för att motverka detta.
        private bool Lose()
        {
            return !(Grid.RowEmpty(0) && Grid.RowEmpty(1));
        }
        //Denna funktionen  används för att placera ett block
        private void Place_Block()
        {
            foreach (Position p in CurrentBlock.TilePosistion())
            {
                Grid[p.Row, p.Column] = CurrentBlock.Id;
            }
            if(!CanHold)
            {
                CanHold = true;
            }
            if(!CanHold2)
            {
                CanHold2 = true;
            }
            Score = Score + Grid.ClearFullRows();

            if (Lose())
            {
                YouLose = true;
            }
           
            else
            {
                CurrentBlock = Block_Queue.Generate_Block();
                

            }
        }
        //Denna funktionen kommer att flytta ditt block nedåt och kommer både styras med knapptryck samt hända konstant under spelets gång.
        //Precis som i de andra funktionerna som hanterade rörelse kommer den motsatta rörelsen ske om blocket inte får plats dvs det flyttas uppåt.
        public void Block_Move_Down()
        {
            CurrentBlock.Move_Block(1, 0);
            if (!BlockFits())
            {
                CurrentBlock.Move_Block(-1, 0);
                Place_Block();
            }
        }
        //Denna funktionen räknar ut hur stort mellan rum det är mellan en ruta i ditt nuvarande block och en cell som är 
        //full. Funktionenv retunerar detta som ett heltal.

        private int TileDropDistance(Position p)
        {
            //Heltalet sätts till noll och ökar varje iteration av kommande loop//
            int drop = 0;
            //Programet använder funktionen CellEmpty på varje ruta under ditt blocks ruta tills den stöter på 
            // en som inte är tom. Den gör detta genom att läsa av vilken column och rad som rutan befiner sig på.
            //Genom att öka drop varje iteration och sen addera den med blockets rad nummer samt för att sen öka 
            //summan med ett förflyttar sig programet ned åt i kolumnen. 
            while (Grid.CellEmpty(p.Row + drop + 1, p.Column))
            {
                drop++;
            }
            return drop;
            //I slutet av programet retuneras heltalet drop//
        }
        //Den här funktionen räknar ut samma sak som den förra fast applicerar det till hela ditt block.
        //Funktionen retunerar sin beräning som ett heltal.//
        public int BlockDropDistance()
        {
            //Ett heltal sätts som antalet rader på rutnätet.//
            int drop = Grid.Rows;
            foreach (Position p in CurrentBlock.TilePosistion())
            {
                //Programet jämför detta heltalet drop med TileDropDistancen med varje ruta, det minsta av dessa 
                // två kommer sättas som drop, efter den sista itterationen har programet räknat ut hur långt blocket kan 
                // falla utan att en ruta hamnar i en cell som redan är fylld.//
                drop = System.Math.Min(drop, TileDropDistance(p));
               
            }

            return drop;
        }
        //Denna funktionen gör så att blocket faller ner och placeras direkt utan att behöva använda Move_Block down.//
        //
        public void DropBlock()
        {
            //Blockets rad
            CurrentBlock.Move_Block(BlockDropDistance(), 0);
            Place_Block();
        }
        public void CombineBlock()
        {
            
                switch (Held_Block, Held_Block2)
                {
                    case (I_Block, I_Block):
                        Held_Block = CurrentBlock;
                        CurrentBlock = new I_Combo();
                        Held_Block2 = null;

                        break;
                    case(O_Block, O_Block):
                        Held_Block = CurrentBlock;
                        CurrentBlock = new O_Combo();
                        Held_Block2 = null;

                        break;
                    case (J_Block, J_Block):
                        Held_Block = CurrentBlock;
                        CurrentBlock = new J_Combo();
                        Held_Block2 = null;
                        break;
                    case (L_Block, L_Block):
                    Held_Block = CurrentBlock;
                        CurrentBlock = new L_Combo();
                        Held_Block2 = null;

                        break;
                    case (T_Block, T_Block):
                        Held_Block = CurrentBlock;
                        CurrentBlock = new T_Combo();
                        Held_Block2 = null;
                        break;
                }
            
         
            
        }
        

        

    }
}
