namespace ProjectSpy
{
    class Program
    {
        public static float GlobalScale = 1f;

        public static int ScreenWidth = 256;
        public static int ScreenHeight = 240;
        
        public static void Main()
        {
            using var game = new ProjectSpy.Game1();
            game.Run();
        }
    }
}

