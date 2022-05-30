using DocumentFormat.OpenXml.Drawing.Diagrams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tetris
{
  //Den här klassen styr det som händer på skärmen med hjälp av de andra klsserna//

    public partial class MainWindow : Window
    {
        private readonly ImageSource[] tileImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/TileEmpty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileCyan.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileYellow.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileBlue.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileOrange.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TilePurple.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileGreen.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileRed.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileCyan.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileYellow.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileBlue.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileOrange.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TilePurple.png", UriKind.Relative))
        };

        private readonly ImageSource[] blockImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/Block-Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-I.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-Z.png", UriKind.Relative))
           
        };
        //De ovanstående arrayerna innehåller det grafiska matrialet som programet använder.//
        //Varje bild plats i arrayen korresponderas till tillhörande block Id dvs bilden till I_blocket har array index ett och den ljus blåa
        // rutan har samma array index i sin array, den finns också med på array index 8 eftersom I_Combo blocket har det Id och har samma färg som I_Blocket,//
        private readonly Image[,] imageControls;
        private readonly int maxDelay = 1000;
        private readonly int minDelay = 75; 
        //Dessa heltal bestämmer största och minsta mellan rummet,i millisekunder, mellan en animation//
        public int delayDecrease  { get;  private set; }
        //Detta heltal används senare för att gå från max till minDelay och varierar beroende på vilken svårighets grad du väljer.//
        private Game_State gameState = new Game_State();

        public MainWindow()
        {
            InitializeComponent();
            imageControls = SetupGameCanvas(gameState.Grid);
        }
       

        private Image[,] SetupGameCanvas(Grid grid)
        {
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25;

            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize
                    };

                    Canvas.SetTop(imageControl, (r - 2) * cellSize + 10);
                    Canvas.SetLeft(imageControl, c * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;
                }
            }

            return imageControls;
        }
       public bool IsPaused()
        
        {
            if (PauseMenu.Visibility == Visibility.Visible)
                return true;
            else
                return false;
        }
        // Denna boolen bverättar om spelet är pausat eller inte genom att läsa av om paus menyn är synlig eller inte.//
        private void DrawGrid(Grid grid)
        {
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    int id = grid[r, c];
                    imageControls[r, c].Opacity = 1;
                    imageControls[r, c].Source = tileImages[id];
                }
            }
        }

        private void DrawBlock(Block block)
        {
            foreach (Position p in block.TilePosistion())
            {
                imageControls[p.Row, p.Column].Opacity = 1;
                imageControls[p.Row, p.Column].Source = tileImages[block.Id];
            }
        }

        private void DrawNextBlock(Block_Queue blockQueue)
        {
            Block next = blockQueue.Next_Block;
            NextImage.Source = blockImages[next.Id];
        }

        private void DrawHeldBlock(Block heldBlock)
        {
            if (heldBlock == null)
            {
                HoldImage.Source = blockImages[0];
            }
            else
            {
                HoldImage.Source = blockImages[heldBlock.Id];
            }
            
        }
        private void DrawHeldBlock2( Block heldBlock2)
        {
            if(heldBlock2 == null)
            {
                HoldImage2.Source = blockImages[0];
            }
            else
            {
                HoldImage2.Source = blockImages[heldBlock2.Id];
            }
        }
        private void Show_Game()
        {
            gameState = new Game_State();
            GameCanvas.Visibility = Visibility.Visible;
            ScoreText.Visibility = Visibility.Visible;
            HoldText.Visibility = Visibility.Visible;
            HoldText2.Visibility = Visibility.Visible;
            HoldImage.Visibility = Visibility.Visible;
            HoldImage2.Visibility = Visibility.Visible;
            NextImage.Visibility = Visibility.Visible;
            NextText.Visibility = Visibility.Visible;
            StartScreen.Visibility = Visibility.Hidden;
        }
        //Denna funktionen ser till att alla spel delar ritas upp när spelet startar.//
        private void Hide_Game()
        {
            GameCanvas.Visibility = Visibility.Hidden;
            ScoreText.Visibility = Visibility.Hidden;
            HoldText.Visibility = Visibility.Hidden;
            HoldText2.Visibility = Visibility.Hidden;
            HoldImage.Visibility = Visibility.Hidden;
            HoldImage2.Visibility = Visibility.Hidden;
            NextImage.Visibility = Visibility.Hidden;
            NextText.Visibility = Visibility.Hidden;
            StartScreen.Visibility = Visibility.Visible;
        }
        //Denna funktionen "gömmer" alla spelets "delar" och kommer användas när man vill gå tillbaka till start skärmen.//
        private void DrawGhostBlock(Block block)
        {
            int dropDistance = gameState.BlockDropDistance();

            foreach (Position p in block.TilePosistion())
            {
                imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25;
                imageControls[p.Row + dropDistance, p.Column].Source = tileImages[block.Id];
            }
        }
        //Denna funktionen ritar upp hur spelet förändras under seplets gång.//
        private void Draw(Game_State gameState)
        {
            DrawGrid(gameState.Grid);
            DrawGhostBlock(gameState.CurrentBlock);
            DrawBlock(gameState.CurrentBlock);
            DrawNextBlock(gameState.Block_Queue);
            DrawHeldBlock(gameState.Held_Block);
            DrawHeldBlock2(gameState.Held_Block2);
            
            ScoreText.Text = $"Score: {gameState.Score}";
        }
        //Denna funktionen styr allt det som spelarn inte kopntrollerar.//
        private async Task GameLoop()
        {
            //Denna if stasen ser till att spelet inte sätts igång medans man är i start skärmen genom att retunera om start skärmen är synlig//
            if (StartScreen.Visibility == Visibility.Visible)
            {
                return;
            }
            //Programet ritar upp spelet i sitt nuvarande skick.//
            Draw(gameState);
            //Denna loopen körs tills spelarn har förlorat//
            while (!gameState.YouLose)
            {  
               int delay = Math.Max(minDelay, maxDelay - (gameState.Score * delayDecrease));
                //Programet börjar med att räkna ut mellan rummet mellan varje "block fall"//
                //Max delayen subtraheras med produkten av poängen och delayDecreasen, på så sätt
                //blir spelet svårare ju mer poäng man får om detta talet är mindre än minDelay kommer 
                //Delayen sättas till minDelay.
                await Task.Delay(delay);
                //Efter detta väntar programet i den tid som oavstående rad räknar ut i milliskeunder//
                if(IsPaused() == false)
                gameState.Block_Move_Down();
                //Sen flyttar programet nuvarande block nedåt så länge pausmenyn inte visas.
                //Detta är för att se till att spelet inte förändras när spelarn har pausat det.//
                Draw(gameState);
                //Slutligen ritar programet upp spelet igen för att få med de förändringar som skett//
            }
            //När spelet är förlorat visas "förlust menyn" och spelarns poäng visas.//
            GameOverMenu.Visibility = Visibility.Visible;
            FinalScoreText.Text = $"Score: {gameState.Score}";
        }
        //Denna funktion hanterar spelets kontroller.//
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
           //Om spelet är pausat retuneras programet för att se till att inget förandras medans det är pausat.//
          
            if (IsPaused() == true)
                return;
            
            // beroende på vilken tangent som trycks in utför programet den tillhörande funktionen.//
            switch (e.Key)
            {
                case Key.Left:
                    gameState.Block_Move_Left();
                    break;
                case Key.Right:
                    gameState.Block_Move_Right();
                    break;
                case Key.Down:
                    gameState.Block_Move_Down();
                    break;
                case Key.Up:
                    gameState.Block_Rotate_CW();
                    break;
                case Key.Z:
                    gameState.Block_Rotate_CounterCW();
                    break;
                case Key.X:
                    gameState.Hold_Block();
                    break;
                case Key.Space:
                    gameState.DropBlock();
                    break;
                case Key.C:
                    gameState.Hold_Block2();
                    break;
                case Key.K:
                    gameState.CombineBlock();
                    break;
                case Key.Escape:
                    PauseMenu.Visibility = Visibility.Visible;
                    break;
                default:
                    return;
            }
            //Islutet av funktionen ritas spelet upp igen för att få med för ändringarna.//
            Draw(gameState);
        }

        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await GameLoop();
        }

        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            gameState = new Game_State();
            GameOverMenu.Visibility = Visibility.Hidden;
            await GameLoop();
        }
        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            gameState = new Game_State();
            PauseMenu.Visibility = Visibility.Hidden;
            
        }
        private void Unpause_Click(object sender, RoutedEventArgs e)
        {
            PauseMenu.Visibility = Visibility.Hidden; 
        }
        private void Return_Click(object sender, RoutedEventArgs e)
        { 
            
            Hide_Game();
            gameState = new Game_State();
            PauseMenu.Visibility=Visibility.Hidden;
            
        }
        private async void One_Click(object sender, RoutedEventArgs e)
        {
            Show_Game();
            delayDecrease = 10;
            await GameLoop();
        }

        private async void Two_Click(object sender, RoutedEventArgs e)
        {
            Show_Game();
            delayDecrease = 20;
            await GameLoop();
        }
        private async void Three_Click(object sender, RoutedEventArgs e)
        {
            Show_Game(); 
            delayDecrease = 30;
            await GameLoop();
        }
        private async void Four_Click(object sender, RoutedEventArgs e)
        {

            Show_Game();
            delayDecrease = 40;
            await GameLoop();
        }
        private async void Five_Click(object sender, RoutedEventArgs e)
        {
            Show_Game();
            delayDecrease = 50;
            await GameLoop();
        }
        private async void Dif_Click(object sender, RoutedEventArgs e)
        {
            Dif_One.Visibility = Visibility.Visible;
            await Task.Delay(200);
            Dif_Two.Visibility = Visibility.Visible;
            await Task.Delay(200);
            Dif_Three.Visibility = Visibility.Visible;
            await Task.Delay(200);
            Dif_Four.Visibility = Visibility.Visible;
            await Task.Delay(200);
            Dif_Five.Visibility = Visibility.Visible;

        }

    }
}