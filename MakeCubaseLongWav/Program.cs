using System;
using System.IO;

namespace MakeCubaseLongWav
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length > 0)
            {
                string inputFile = args[0];
                SuperWAV reader = new SuperWAV(inputFile);
                string outputFile = GetUnusedFilename(inputFile + ".cubase.wav");
                SuperWAV.WavInfo wavInfo = reader.getWavInfo();
                SuperWAV writer = new SuperWAV(outputFile, SuperWAV.WavFormat.CUBASE_BIGFILE, wavInfo.sampleRate, wavInfo.channelCount, wavInfo.audioFormat, wavInfo.bitsPerSample);

                UInt64 batchSize = 100000;
                UInt64 currentPosition = 0;
                UInt64 samplesToGet = wavInfo.dataLength/wavInfo.bytesPerTick;
                while(currentPosition < samplesToGet)
                {
                    float[] dataIn = reader.getAs32BitFloatFast(currentPosition,currentPosition+10000);
                    writer.writeFloatArrayFast(dataIn, currentPosition);
                    currentPosition += 10000;
                }

            }


            Console.ReadKey();
        }


        public static string GetUnusedFilename(string baseFilename)
        {
            if (!File.Exists(baseFilename))
            {
                return baseFilename;
            }
            string extension = Path.GetExtension(baseFilename);

            int index = 1;
            while (File.Exists(Path.ChangeExtension(baseFilename, "." + (++index) + extension))) ;

            return Path.ChangeExtension(baseFilename, "." + (index) + extension);
        }
    }


}
