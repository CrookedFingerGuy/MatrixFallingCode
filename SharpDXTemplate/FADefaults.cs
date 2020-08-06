namespace MatrixFallingCode
{
    public class FADefaults
    {
        public bool screenPaused;
        public bool isSettingMenuVisible;
        public float redValue;
        public float greenValue;
        public float blueValue;
        public int fontSize;
        public int numberOfDrops;
        public int minDropLength;
        public int maxDropLength;

        public FADefaults()
        {
            screenPaused = false;
            isSettingMenuVisible = false;
            redValue = 0.0f;
            greenValue = 1f;
            blueValue = 0.0f;
            fontSize = 36;
            numberOfDrops = 250;
            minDropLength = 4;
            maxDropLength = 16;
        }
    }
}
