using System.Collections.Generic;
//Den här klassen hanterar blocken. Varje block kommer ha sin egen klass men den här klassen inefattar mycket av funktionerna som blocken kommer ha och kommer att anropas i de andra klasserna.//
namespace Tetris
{
    public abstract class Block
    {
        //Här anropas bla Position klassen för att göra det möjligt att läsa av blockets position//
        //Blocken representeras i koden med en unik Id siffra som sen kommer användas för att rita upp blocken//
        //Dessa siffror hittas i en annan matris,Tiles. Denna matirs är tillräckligt stor för att få plats med alla rotations möjligheter//
        //När blocket förflyttas är det i själva verket matrisen som flyttas, förflyttningen utgår i från matrisens start hörn.//
        protected abstract Position[][] Tiles { get; }
        protected abstract Position StartOffset { get; }
        public abstract int Id { get; }
        private int RotationState;
        private Position offset;
        //Här hämtas blockens start punkt, alla block kommer ha en unik start punkt pga deras rutnät är olika stora då blocken har olika storlekar.//
        public Block()
        {
            offset = new Position(StartOffset.Row, StartOffset.Column);
        }
        //IEnumerable gör det möjligt att hämta listor från andra klasser, detta är nödvändigt då blockens "rotationssorter" lagras i en subklass av denna klass.//
        public IEnumerable<Position> TilePosistion()
        {
            foreach (Position p in Tiles[RotationState])
            {
                yield return new Position(p.Row + offset.Row, p.Column + offset.Column);
            }
        }
        //Genom att ha en siffra per "rotationssort" kan man enkelt rotera blocket med hjälp av att ändra denna//
        public void Rotate_CW()
        {
            RotationState = (RotationState + 1) % Tiles.Length;
        }
        public void Rotate_CounterCW()
        {
            if (RotationState == 0)
            {
                RotationState = Tiles.Length - 1;
            }
            else
            {
                RotationState = RotationState-1;
            }
        }
        public void Move_Block(int rows, int columns)
        {
            offset.Row += rows;
            offset.Column += columns;
        }
        public void Reset()
        {
            RotationState = 0;
            offset.Row = StartOffset.Row;
            offset.Column = StartOffset.Column;
        }

    }
}