		/// <summary>
        /// Loads and returns *.png files
        /// </summary>
        private String[] LoadFiles()
        {
            //Used to store file paths
            String[] Files = new String[0];

            try
            {
                //List all files in the selected directory
                Files = Directory.GetFiles(Properties.Settings.Default.Line4_Path, "*.*");

                //Initialise File Creation-Date Array (in clock ticks)
                long[] TickArray = new long[Files.Length];
                int tickSentinel = 0;

                //Determine file creation time
                foreach (var File in Files)
                {
                    //Append each file details
                    TickArray[tickSentinel] = System.IO.File.GetCreationTimeUtc(File).Ticks;
                    tickSentinel++;
                }

                //Sort files by creation time
                return sortFiles(Files, TickArray);
            }
            //Exceptions
            catch (IOException except)
            {
                new ErrorForm("[IOException] " + "Path not found: Incorrect directory location.", except.ToString()).ShowDialog();
            }
            catch (UnauthorizedAccessException except)
            {
                new ErrorForm("[UnauthorizedAccessException] ", except.ToString()).ShowDialog();
            }
            catch (ArgumentException except)
            {
                new ErrorForm("[ArgumentException] ", except.ToString()).ShowDialog();
            }
            
            return Files;
        }

        /// <summary>
        /// Sorts a string array containing file paths using a parameter array containing file creation information in ticks
        /// </summary>
        private String[] sortFiles(String[] Files, long[] TickArray)
        {
            //Keeps track of the position of each file 
            int[] Mapper = linSpace(TickArray.Length);

            //Function output
            String[] sortedFiles = new String[Files.Length];

            //Bubble Sort
            bool sorted;
            do
            {
                //Flag keeps track of early finish
                sorted = true;

                for (int i = 0; i < TickArray.Length-1; i++)
                {
                    //Largest number (oldest file) first
                    if (TickArray[i] < TickArray[i + 1])
                    {
                        //Swap values
                        long tickBuffer = TickArray[i];
                        TickArray[i] = TickArray[i + 1];
                        TickArray[i + 1] = tickBuffer;
                        
                        //Update mapper
                        int mapBuffer = Mapper[i];
                        Mapper[i] = Mapper[i + 1];
                        Mapper[i + 1] = mapBuffer;

                        sorted = false;
                    }
                }
            } while (!sorted);

            //Map files to sorted list
            for(int i = 0; i < Files.Length; i++)
            {
                sortedFiles[i] = Files[Mapper[i]];
            }

            return sortedFiles;
        }

        /// <summary>
        /// Returns an array of linearly separated integer values
        /// </summary>
        private int[] linSpace(int Length)
        {
            //Holds target values
            int[] linearArray = new int[Length];

            //Assign target values
            for(int i = 0; i < Length; i++)
            {
                linearArray[i] = i;
            }
            return linearArray;
        }