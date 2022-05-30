using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{

    public class Grid
    {
        private readonly int[,] grid;
        public int Rows { get; }
        public int Columns { get; }
        public int this [int r,int c] 
        {
            get => grid[r,c];
            set => grid[r,c] = value;
        }
        public Grid(int rows, int colums) 
        {
            Rows = rows;
            Columns = colums;
            grid = new int[rows, colums];
        }
         //Kollar om en ruta finns med på rutnätet genom att kolla om kolumn och rad indexen finns i den intervall som spelet använder//
        public bool InsideGrid(int r, int c) 
        {
            return r >= 0 && c >= 0 && c < Columns && r < Rows;
        }
        //Kollar om en ruta är tom genom att kolla om den är med på rutnätet och om dess värde är lika med noll.Detta kommer vara användbart för att bestämma om ett block kan placeras.//
        public bool CellEmpty(int r, int c) 
        {
            return InsideGrid(r,c) && grid [r,c] == 0;        
        }
      // Dessa två booler kollar om en rad är full genom att kolla igenom varje cell på en rad. Detta kommer vara användbart när vi sen ska rensa raderna//
        public bool RowFull(int r) 
        {
            for (int c = 0; c < Columns; c++) 
            { 
                if(grid[r,c] == 0) 
                { 
                    return false;
                }           
            }
            return true;
        }
        public bool RowEmpty(int r) 
        {
            for (int c = 0; c < Columns; c++)
            {
                if (grid[r,c] != 0)
                { 
                    return false;
                }
            }
            return true;
        }
        //Denna funktionen rensar en rad genom att nollställe varje cell på den.//
        private void RowClear(int r)
        {
            for(int c = 0; c < Columns; c++)
            {
                grid[r,c] = 0; 
            }
        }
        //Denna funktionen flyttar ner en rad med antalet rensade rader och genom att nollställa radens tidigare plats//

        private void MoveRowDown(int r, int numRows)
        {
            for (int c = 0; c < Columns; c++)
            {
                grid [r+numRows,c] = grid[r,c];
                grid [r,c] = 0;
            }
        }
        //Denna funktionen kombinerar de två sennaste och används för att rensa fulla rader genom att gå igenom varje rad för att kolla om den är full.//
        //Är den full rensas den och antalet rensade rader ökar med ett. Om en rad inte är full kommer den flyttas ner med antalet rensade rader.//

        public int ClearFullRows()
        {
           int cleared = 0;
            for (int r = Rows-1; r >= 0; r--)
            {
                if (RowFull(r))
                {
                    RowClear(r);
                    cleared++;
                }
                else if (cleared>0)
                {
                    MoveRowDown(r, cleared);
                }
            }
            return cleared;
        }




    }
}
