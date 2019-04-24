using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kNN
{

    public class Iris
    {
        public string[] classes = { "Iris-setosa", "Iris-versicolor", "Iris-virginica" };
        /*
         * 1. sepal length in cm
         * 2. sepal width in cm
         * 3. petal length in cm
         * 4. petal width in cm
         */
        
        public double _sepalLength;
        public double _sepalWidth;
        public double _petalLength;
        public double _petalWidth;

        public int _classification;

        public Iris(double sepalLength, double sepalWidth, double petalLength, double petalWidth)
        {
            _sepalLength = sepalLength;
            _sepalWidth = sepalWidth;
            _petalLength = petalLength;
            _petalWidth = petalWidth;
        }

        public Iris(double sepalLength, double sepalWidth, double petalLength, double petalWidth, string classification) : this(sepalLength, sepalWidth, petalLength, petalWidth)
        {
            int index = Array.IndexOf(classes, classification);
            _classification = index;
        }

        public Iris(string[] parameter) : this(double.Parse(parameter[0], System.Globalization.CultureInfo.InvariantCulture)
            , double.Parse(parameter[1], System.Globalization.CultureInfo.InvariantCulture)
            , double.Parse(parameter[2], System.Globalization.CultureInfo.InvariantCulture)
            , double.Parse(parameter[3], System.Globalization.CultureInfo.InvariantCulture)
            , parameter[4])
        {
        }
    }

    public class IndexAndDistance : IComparable<IndexAndDistance>
    {
        public int index;
        public double dist;

        public int CompareTo(IndexAndDistance other)
        {
            if (this.dist < other.dist) return -1;
            else if (this.dist > other.dist) return +1;
            else return 0;
        }
    }
}
