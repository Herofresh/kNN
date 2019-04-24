using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace kNN
{
    class KNN
    {
        static Random random = new Random();
        [STAThread]
        static void Main(string[] args)
        {
            string[] classes = { "Iris-setosa", "Iris-versicolor", "Iris-virginica" };
            int correct = 0;
            int wrong = 0;
            int[,] confusionMatrix = new int[3, 3];
            List<List<Iris>> allFolds = LoadData(false); //true for 10 Fold Cross Validation
            Stopwatch stopWatch = new Stopwatch();
            Iris[] randomIris = GenerateXRandomIris(1500);
            /*for (int i = 0; i < 10; i++)
            {*/
                //List<List<Iris>> fold = generateFold(i, allFolds);
                //foreach (Iris iris in fold[1]) -- for 10 Fold Cross Validation
                stopWatch.Start();
                foreach (Iris iris in randomIris)
                {
                    //int classification = Classify(iris, fold[0], 3); -- for 10 Fold Cross Validation
                    int classification = Classify(iris, allFolds[0], 3);

                    /*Console.WriteLine();
                    Console.WriteLine(classes[classification] == classes[iris._classification]);

                    if (classes[classification] == classes[iris._classification])
                    {
                        correct++;
                    }
                    else
                    {
                        wrong++;
                    }
                    confusionMatrix[iris._classification, classification]++;
                }
                Console.WriteLine();
                Console.WriteLine("----------------------Fold: " + i + "---------------------------");
                Console.WriteLine();*/

                }
                /*int sum = correct + wrong;
                Console.WriteLine(correct + "/" + sum + " Correct");
                Console.WriteLine(wrong + "/" + sum + " Wrong");*/
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;

                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);
                Console.WriteLine("RunTime " + elapsedTime);
                Console.ReadLine();
            //}
        }

        static int Classify(Iris unknown, List<Iris> trainData, int k)
        {
            int n = trainData.Count;
            IndexAndDistance[] info = new IndexAndDistance[n];
            for (int i = 0; i < n; ++i)
            {
                IndexAndDistance curr = new IndexAndDistance();
                double dist = Distance(unknown, trainData[i]);
                curr.index = i;
                curr.dist = dist;
                info[i] = curr;
            }
            Array.Sort(info);
            int count = 0;
            int sum = 0;
            foreach (IndexAndDistance x in info)
            {
                if (trainData[x.index]._classification == 0)
                    count++;
                sum++;
            }
            int result = Vote(info, trainData, k);
            return result;
        }
        static int Vote(IndexAndDistance[] info, List<Iris> trainData, int k)
        {
            int[] votes = new int[3];
            for (int i = 0; i < k; ++i)
            {
                int idx = info[i].index;
                int c = trainData[idx]._classification;
                ++votes[c];
            }
            int mostVotes = 0;
            int classWithMostVotes = 0;
            for (int j = 0; j < 3; ++j)
            {
                if (votes[j] > mostVotes)
                {
                    mostVotes = votes[j];
                    classWithMostVotes = j;
                }
            }
            return classWithMostVotes;
        }
        static double Distance(Iris unknown, Iris data)
        {
            return Math.Sqrt(Math.Pow(unknown._sepalLength - data._sepalLength, 2) +
                Math.Pow(unknown._sepalWidth - data._sepalWidth, 2) +
                Math.Pow(unknown._petalLength - data._petalLength, 2) +
                Math.Pow(unknown._petalWidth - data._petalWidth, 2));
        }
        static List<List<Iris>> LoadData(bool Folds = false)
        {
            List<List<Iris>> returnFolds = new List<List<Iris>>();
            string path = string.Empty;
            List<string> allData = new List<string>();
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    path = ofd.FileName;

                    var fileStream = ofd.OpenFile();
                    string currendLine = "";
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        do
                        {
                            currendLine = reader.ReadLine();
                            if (!currendLine.Equals(""))
                            {
                                allData.Add(currendLine);
                            }
                        } while (!reader.EndOfStream);
                    }
                }
            }

            for (int i = 0; i < 10; i++)
            {
                returnFolds.Add(new List<Iris>());
            }

            if (Folds)
            {
                int numberPerTenFoldCrossFold = allData.Count / 10;
                int[] fullFolds = Enumerable.Repeat(-1, 10).ToArray();
                int indexFullFolds = 0;



                foreach (string iris in allData)
                {
                    string[] values = iris.Split(',');
                    int index = GiveMeANumber(fullFolds);
                    returnFolds[index].Add(new Iris(values));
                    if (returnFolds[index].Count >= numberPerTenFoldCrossFold)
                    {
                        fullFolds[indexFullFolds] = index;
                        indexFullFolds++;
                    }
                }
            }
            else
            {
                foreach (string iris in allData)
                {
                    string[] values = iris.Split(',');
                    returnFolds[0].Add(new Iris(values));
                }
            }


            return returnFolds;
        }

        static Iris[] GenerateXRandomIris(int x)
        {
            Iris[] randomIris = new Iris[x];
            for (int i = 0; i < x; i++)
            {
                randomIris[i] = new Iris(GetRandomNumber(4, 8), GetRandomNumber(2, 5), GetRandomNumber(1, 7), GetRandomNumber(0, 3));
            }
            return randomIris;
        }

        static double GetRandomNumber(double minimum, double maximum)
        {
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

        static private int GiveMeANumber(int[] excludedInt)
        {
            var exclude = new HashSet<int>(excludedInt);
            var range = Enumerable.Range(0, 10).Where(i => !exclude.Contains(i));

            int index = random.Next(0, 10 - exclude.Count);
            return range.ElementAt(index);
        }

        static private List<List<Iris>> generateFold(int testFold, List<List<Iris>> allFolds)
        {
            List<Iris> test = allFolds[testFold];
            int lengthLearnFold = 0;
            for (int i = 0; i < 10; i++)
            {
                if (i != testFold)
                {
                    lengthLearnFold += allFolds[i].Count;
                }
            }
            List<Iris> learn = new List<Iris>(lengthLearnFold);
            for (int i = 0; i < 10; i++)
            {
                if (i != testFold)
                {
                    learn.AddRange(allFolds[i]);
                }
            }
            List<List<Iris>> Fold = new List<List<Iris>>();
            Fold.Add(learn);
            Fold.Add(test);

            return Fold;
        }
    }
}
